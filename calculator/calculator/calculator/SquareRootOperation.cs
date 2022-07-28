using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary
{
    public class SquareRootOperation : UnaryOperation
    {
        public override double Evaluate(double[] values)
        {
            return Math.Sqrt(values[0]);
        }
    }
}
