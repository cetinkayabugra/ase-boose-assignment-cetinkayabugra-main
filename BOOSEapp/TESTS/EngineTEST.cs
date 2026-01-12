using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSEapp;

namespace BOOSEappTest
{
    [TestClass]
    public class ExpressionEngineTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Ensure clean state before each test
            VariableStore.Clear();
        }

        // TEST 1: Basic arithmetic
        [TestMethod]
        public void EvalDouble_BasicArithmetic_ReturnsCorrectResult()
        {
            double result = ExpressionEngine.EvalDouble("2 + 3 * 4");
            Assert.AreEqual(14.0, result, 0.0001);
        }

        // TEST 2: Real number multiplication
        [TestMethod]
        public void EvalDouble_RealValues_ReturnsCorrectResult()
        {
            double result = ExpressionEngine.EvalDouble("15.5 * 10.0");
            Assert.AreEqual(155.0, result, 0.0001);
        }

        // TEST 3: Variables in expression
        [TestMethod]
        public void EvalDouble_WithVariables_ReturnsCorrectResult()
        {
            VariableStore.Set("length", 15.5);
            VariableStore.Set("width", 10.0);

            double result = ExpressionEngine.EvalDouble("length * width");
            Assert.AreEqual(155.0, result, 0.0001);
        }

        // TEST 4: Complex expression with constants
        [TestMethod]
        public void EvalDouble_ComplexExpression_ReturnsCorrectResult()
        {
            VariableStore.Set("pi", 3.14159);
            VariableStore.Set("radius", 27.7);

            double result = ExpressionEngine.EvalDouble("2 * pi * radius");
            Assert.AreEqual(174.049, result, 0.001);
        }

        // TEST 5: Integer evaluation with rounding
        [TestMethod]
        public void EvalInt_RoundsCorrectly_ReturnsInteger()
        {
            int result = ExpressionEngine.EvalInt("10 / 3");
            Assert.AreEqual(3, result);
        }
    }
}
