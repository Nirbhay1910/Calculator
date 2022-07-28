using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary
{
    public class InvalidBracketException: Exception
    {
        public InvalidBracketException():base(ErrorMessages.INVALID_OPERATION_BRACKETS){}
    }
    public class NotFoundOperatorException: Exception
    {
        public NotFoundOperatorException(string operatorSymbol) : base(operatorSymbol + ErrorMessages.KEY_NOT_FOUND) { }
    }
    public class ExtraOperandsException: Exception
    {
        public ExtraOperandsException():base(ErrorMessages.MORE_OPERANDS){}
    }
    public class LessOperandsException : Exception
    {
        public LessOperandsException(string operatorSymbol) : base(ErrorMessages.LESS_OPERANDS+operatorSymbol) { }
    }
    public class EmptyExpressionException: Exception
    {
        public EmptyExpressionException() : base(ErrorMessages.INVALID_OPERATION_EMPTY) { }
    }
    public class CannotAddOrDeleteOperatorException : Exception
    {
        public CannotAddOrDeleteOperatorException(string operatorSymbol) : base(operatorSymbol + " : "+ErrorMessages.ADD_DELETE_OPERATOR) { }
    }
    public class RangeOverflowException : Exception
    {
        public RangeOverflowException() : base(ErrorMessages.RANGE_OVERFLOW) { }
    }
}
