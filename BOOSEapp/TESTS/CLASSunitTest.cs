using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSEapp;   // change to your namespace

namespace BOOSEappTest
{
    [TestClass]
    public class ExpressionEngineTests
    {
        [TestMethod]
        public void EvalDouble_ValidExpression_ReturnsCorrectResult()
        {
            // Arrange
            string expression = "2 + 3 * 4";

            // Act
            double result = ExpressionEngine.EvalDouble(expression);

            // Assert
            Assert.AreEqual(14.0, result, 0.0001);
        }
    }
}
