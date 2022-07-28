using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CalculatorLibrary;

namespace UnitTestCalculatorLibrary
{
    [TestClass]
    public class UnitTest1
    {
        ExpressionEvaluator evaluator = new ExpressionEvaluator();

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(evaluator.Evaluate("0.2+5"),5.2);
            Assert.AreEqual(evaluator.Evaluate("2*log1000"), 6);
            Assert.AreEqual(evaluator.Evaluate("10+10%"), 11);
            Assert.AreEqual(evaluator.Evaluate("(10mod5)"), 0);
            Assert.AreEqual(evaluator.Evaluate("5!+(log(1000)* -5)"), 105);
            Assert.AreEqual(evaluator.Evaluate("4 + -5 *-3"), 19);
            Assert.AreEqual(evaluator.Evaluate("( 4 + -5) *-3"), 3);
        }
    }
}
