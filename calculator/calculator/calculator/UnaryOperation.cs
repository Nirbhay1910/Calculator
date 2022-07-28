using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculatorLibrary
{
    public abstract class UnaryOperation : Operation
    {
        public UnaryOperation()
        {
            OperandCount = 1;
        }
    }
}