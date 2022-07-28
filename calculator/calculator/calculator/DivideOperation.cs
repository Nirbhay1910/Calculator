using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculatorLibrary
{
    public class DivideOperation : BinaryOperation
    {
        public override double Evaluate(double[] values)
        {
            if (values[0] == 0) throw new DivideByZeroException(ErrorMessages.CANNOT_DIVIDE_ZERO);
            return values[1] / values[0];
        }
    }
}