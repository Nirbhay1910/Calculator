using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary
{
    public class OperatorProperties
    {
        public Operation operation { get; set; }
        public string symbol { get; set; }
        public double precedence { get; set; }
        public bool customOperator { get; set; }
        public OperatorProperties(Operation op, double pre, bool custom=false)
        {
            operation = op;
            precedence = pre;
            customOperator = custom;
        }
    }
}
