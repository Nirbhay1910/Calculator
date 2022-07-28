using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary
{
    public class ModulusOperation : BinaryOperation
    {
        public override double Evaluate(double[] values)
        {
            return values[1] % values[0];
        }
    }
}
