using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculatorLibrary
{
    public abstract class BinaryOperation : Operation
    {
        public BinaryOperation()
        {
            OperandCount = 2;
        }
    }
}