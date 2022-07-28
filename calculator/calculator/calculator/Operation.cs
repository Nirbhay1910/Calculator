using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary
{
    public abstract class Operation:IOperation
    {
        public double OperandCount { get; protected set; }
        public abstract double Evaluate(double[] values);
    }
}