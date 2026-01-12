using BOOSE;
using BOOSEapp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOOSEappTest
{
    /// <summary>
    /// Tests for UnrestrictedReal variable behaviour.
    /// Demonstrates removal of BOOSE real variable restrictions.
    /// </summary>
    [TestClass]
    public class UnrestrictedRealTests
    {
        [TestMethod]
        public void RealVariable_Can_Be_Declared()
        {
            var canvas = new AppCanvas(500, 500);
            var factory = new AppCommandFactory();
            var program = new StoredProgram(canvas);
            var parser = new Parser(factory, program);

            string code = @"
real x = 5.5
moveto 10,10
";

            // Act
            parser.ParseProgram(code);
            program.Run();

            // Assert
            // If execution reaches here, declaration succeeded
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(true);
        }
    }
}