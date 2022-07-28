using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary
{
    public class PercentOperation : BinaryOperation
    {
        public override double Evaluate(double[] values)
        {
            double result = (values[0] * values[1]) / 100;
            if (result > double.MaxValue || result < double.MinValue) throw new RangeOverflowException();
            return result;
        }
    }
}
