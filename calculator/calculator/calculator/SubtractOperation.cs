using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculatorLibrary
{
    public class SubtractOperation : BinaryOperation
    {
        public override double Evaluate(double[] values)
        {
            double result = values[1] - values[0];
            if (result < double.MinValue || result > double.MaxValue) throw new RangeOverflowException();
            return result;
        }
    }
}