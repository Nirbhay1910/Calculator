using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Resources;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class ExpressionEvaluator
    {
        public string myOperatorsString = File.ReadAllText("./OperatorsResources.json");
        public Dictionary<string, OperatorProperties> myOperators; 
        // dictionary to store operators and their properties
        private Dictionary<string, OperatorProperties> _operatorTable;
        // used in evaluator function to handle percent operator
        string percentSymbol;
        public ExpressionEvaluator()
        {
            //initialising dictionary with inbuilt operators
            myOperators = JsonConvert.DeserializeObject<Dictionary<string, OperatorProperties>>(myOperatorsString);
            percentSymbol = myOperators["PERCENT_OPERATOR"].symbol;
            _operatorTable = new Dictionary<string, OperatorProperties>();
            _operatorTable.Add(myOperators["ADD_OPERATOR"].symbol, new OperatorProperties(new AddOperation(), myOperators["ADD_OPERATOR"].precedence));
            _operatorTable.Add(myOperators["SUBTRACT_OPERATOR"].symbol, new OperatorProperties(new SubtractOperation(), myOperators["SUBTRACT_OPERATOR"].precedence));
            _operatorTable.Add(myOperators["MULTIPLY_OPERATOR"].symbol, new OperatorProperties(new MultiplyOperation(), myOperators["MULTIPLY_OPERATOR"].precedence));
            _operatorTable.Add(myOperators["DIVIDE_OPERATOR"].symbol, new OperatorProperties(new DivideOperation(), myOperators["DIVIDE_OPERATOR"].precedence));
            _operatorTable.Add(myOperators["MODULUS_OPERATOR"].symbol, new OperatorProperties(new ModulusOperation(), myOperators["MODULUS_OPERATOR"].precedence));
            _operatorTable.Add(myOperators["SQUARE_OPERATOR"].symbol, new OperatorProperties(new SquareOperation(), myOperators["SQUARE_OPERATOR"].precedence));
            _operatorTable.Add(myOperators["SQUAREROOT_OPERATOR"].symbol, new OperatorProperties(new SquareRootOperation(), myOperators["SQUAREROOT_OPERATOR"].precedence));
            _operatorTable.Add(myOperators["LOG10_OPERATOR"].symbol, new OperatorProperties(new Log10Operation(), myOperators["LOG10_OPERATOR"].precedence));
            _operatorTable.Add(myOperators["PERCENT_OPERATOR"].symbol, new OperatorProperties(new PercentOperation(), myOperators["PERCENT_OPERATOR"].precedence));
            _operatorTable.Add(myOperators["FACTORIAL_OPERATOR"].symbol, new OperatorProperties(new FactorialOperation(), myOperators["FACTORIAL_OPERATOR"].precedence));
        }

        public void AddCustomOperator(string operatorKey, Operation op)
        {
            myOperatorsString = File.ReadAllText("./OperatorsResources.json");
            myOperators = JsonConvert.DeserializeObject<Dictionary<string, OperatorProperties>>(myOperatorsString);
            if (!myOperators.ContainsKey(operatorKey) || _operatorTable.ContainsKey(myOperators[operatorKey].symbol))
            {
                // throws error if a same operator is not available in JSON or already defined
                throw new CannotAddOrDeleteOperatorException(operatorKey);
            }
            else
            {
                //else add the operator in dictionary
                _operatorTable.Add(myOperators[operatorKey].symbol, new OperatorProperties(op,myOperators[operatorKey].precedence, true));
            }
        }


        public void RemoveCustomOperator(string operatorKey, string operatorSymbol)
        {
            myOperatorsString = File.ReadAllText("./OperatorsResources.json");
            myOperators = JsonConvert.DeserializeObject<Dictionary<string, OperatorProperties>>(myOperatorsString);
            // if the operator is present in dictionary and is not inbuilt then remove the operator
            if (!myOperators.ContainsKey(operatorKey) && _operatorTable.ContainsKey(operatorSymbol) && _operatorTable[operatorSymbol].customOperator==true)
            {
                _operatorTable.Remove(operatorSymbol);
            }
            else
            {
                throw new CannotAddOrDeleteOperatorException(operatorSymbol);
            }
        }

        private string[] Tokenize(string input)
        {
            // Method for making tokens from input string

            List<string> tokens = new List<string>();
            
            int i = 0;
            string token = "";
            double temp;
            while(i < input.Length)
            {
                if(input[i] == ' ')
                {
                    if (token.Length > 0)
                    {
                        token.Trim();
                        if (token.Length > 0) tokens.Add(token);
                        token = "";
                    }
                    i++;
                    continue;
                }
                if(input[i]=='(' || input[i] == ')' || _operatorTable.ContainsKey(Convert.ToString(input[i])))
                {
                    if (input[i] == '(' && ((token.Length > 0 && token[token.Length-1]>='0' && token[token.Length - 1] <= '9')||(tokens.Count>0&&tokens[tokens.Count-1]==")")))
                    {
                        if((_operatorTable.ContainsKey(token) && _operatorTable[token].operation.OperandCount > 1))
                        {
                            // do nothing
                        }
                        else tokens.Add("*");
                    }
                    if (token.Length > 0)
                    {
                        token.Trim();
                        if (token.Length > 0) tokens.Add(token);
                    }
                    if ((input[i] == '-' || input[i] == '+') && (tokens.Count == 0 || (_operatorTable.ContainsKey(tokens[tokens.Count - 1])&& _operatorTable[tokens[tokens.Count - 1]].operation.OperandCount!=1)))
                    {
                        token = Convert.ToString(input[i]);
                    }
                    else
                    {
                        token = Convert.ToString(input[i]);
                        tokens.Add(token);
                        token = "";
                    }
                    i++;
                    continue;
                    
                }
                if ((token!="-"&&token!="+"&&(input[i]<'a'||input[i]>'z')&&_operatorTable.ContainsKey(token))||(Double.TryParse(token,out temp)&&(input[i]!='.')&&(input[i]<'0'||input[i]>'9'))){
                    tokens.Add(token);
                    token = "";
                }
                token += Convert.ToString(input[i]);
                i++;
            }
            if (token.Length > 0)
            {
                token.Trim();
                if (token.Length>0)tokens.Add(token);
            }
            return tokens.ToArray();
        }


        // Method to evaluate input string and generating output
        public double Evaluate(string input)
        {
            input = input.Trim();
            string[] tokens = Tokenize(input);
            for (int i = 0; i < tokens.Length; i++)
            if (tokens.Length == 1)
            {
                throw new EmptyExpressionException();
            }
            Stack<double> operands = new Stack<double>();
            Stack<string> operators = new Stack<string>();
            for (int i = 0; i < tokens.Length; i++)
            {
                if(tokens[i] == " ") continue;
                if ((tokens[i][0] >= '0' && tokens[i][0] <= '9') || (tokens[i].Length > 1 && (tokens[i][0] == '+' || tokens[i][0] == '-') && (tokens[i][1] >= '0' && tokens[i][1] <= '9')))
                {
                    operands.Push(Convert.ToDouble(tokens[i]));
                }
                else if (tokens[i] == "(") operators.Push(tokens[i]);
                else if (tokens[i] == ")")
                {
                    if (operators.Count == 0) throw new InvalidBracketException();
                    while (operators.Peek() != "(")
                    {
                        ApplyOperator( operators,  operands);
                        if (operators.Count == 0) throw new InvalidBracketException();
                    }
                    operators.Pop();
                }
                else if (_operatorTable.ContainsKey(tokens[i]))
                {
                    if (percentSymbol == tokens[i])
                    {
                        if (operands.Count == 1 || operators.Peek()==")" || operators.Peek()=="(") operands.Push(1);
                        else if (operands.Count >= 2)
                        {
                            double temp = operands.Pop();
                            operands.Push(operands.Peek());
                            operands.Push(temp);
                        }
                        operators.Push(tokens[i]);
                        ApplyOperator(operators, operands);
                    }
                    else
                    {
                        while (operators.Count() > 0 && (operators.Peek() != "(" && operators.Peek() != ")") && (_operatorTable[operators.Peek()].precedence >= _operatorTable[tokens[i]].precedence))
                        {
                            ApplyOperator(operators, operands);
                        }
                        operators.Push(tokens[i]);
                    }
                }
                else
                {
                    throw new NotFoundOperatorException(tokens[i]);
                }
            }
            while (operators.Count() > 0)
            {
                if(operators.Peek() == "("||operators.Peek() == ")") throw new InvalidOperationException(ErrorMessages.INVALID_OPERATION_BRACKETS);
                ApplyOperator( operators, operands);
            }
            if(operands.Count() > 1)
            {
                throw new ExtraOperandsException();
            }
            return operands.Pop();
        }

        // Method for helping Evaluate function in calculating result from operator at top of the stack "operators"
        private void ApplyOperator(Stack<string> operators,Stack<double> operands)
        {
            double operandCount = _operatorTable[operators.Peek()].operation.OperandCount;
            if (operands.Count() >= operandCount) {
                double[] values = new double[Convert.ToInt32(operandCount)];
                for (int j = 0; j < operandCount; j++)
                {
                    if(operands.Peek() < double.MinValue || operands.Peek()> double.MaxValue) throw new RangeOverflowException();
                    values[j] = operands.Pop();
                }
                operands.Push(_operatorTable[operators.Pop()].operation.Evaluate(values));
            }
            else {
                throw new LessOperandsException(operators.Peek());
            }
        }
    }
}