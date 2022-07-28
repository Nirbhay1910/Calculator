using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary
{
    public class SquareOperation : UnaryOperation
    {
        public override double Evaluate(double[] values)
        {
            double result = values[0] * values[0];
            if(result < double.MinValue || result > double.MaxValue) throw new RangeOverflowException();
            return result;
        }
    }
}
