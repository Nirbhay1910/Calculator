using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary
{
    public class Log10Operation : UnaryOperation
    {
        public override double Evaluate(double[] values)
        {
            return Math.Log10(values[0]);
        }
    }
}
