using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculatorLibrary
{
    public class AddOperation : BinaryOperation
    {
        public override double Evaluate(double[] values)
        {
            double result = values[0] + values[1];
            if (result > double.MaxValue || result < double.MinValue) throw new RangeOverflowException();
            return result;
        }
    }
}