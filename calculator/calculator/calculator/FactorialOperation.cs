using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculatorLibrary
{
    public class FactorialOperation : UnaryOperation
    {
        public override double Evaluate(double[] values)
        {
            double result = 1;
            for (int i = 1; i <= values[0]; i++)
            {
                if (result > double.MaxValue || result < double.MinValue) throw new RangeOverflowException(); 
                result *= i;
            }
            return result;
        }
    }
}