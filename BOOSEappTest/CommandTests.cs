using BOOSE;
using BOOSEapp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOOSEappTest
{
    /// <summary>
    /// Unit tests for the BOOSE drawing application.
    /// Covers MoveTo, DrawTo and a multiline program, as required.
    /// </summary>
    [TestClass]
    public sealed class CommandTests
    {
        /// <summary>
        /// Unit Test #1 – checks that a single moveto command
        /// puts the pen at the expected coordinates.
        /// </summary>
        [TestMethod]
        public void MoveTo_Updates_Pen_Position()
        {
            // Arrange
            var canvas = new AppCanvas(500, 500);
            var factory = new AppCommandFactory();
            var program = new StoredProgram(canvas);
            var parser = new Parser(factory, program);

            string code = "moveto 200,150";

            // Act
            parser.ParseProgram(code);
            program.Run();

            // Assert
            Assert.AreEqual(200, canvas.Xpos, "X position was not updated correctly by moveto.");
            Assert.AreEqual(150, canvas.Ypos, "Y position was not updated correctly by moveto.");
        }

        /// <summary>
        /// Unit Test #2 – checks that a moveto followed by drawto
        /// leaves the pen at the expected coordinates.
        /// </summary>
        [TestMethod]
        public void DrawTo_Updates_Pen_Position()
        {
            // Arrange
            var canvas = new AppCanvas(500, 500);
            var factory = new AppCommandFactory();
            var program = new StoredProgram(canvas);
            var parser = new Parser(factory, program);

            string code = @"
moveto 100,100
drawto 300,250
";

            // Act
            parser.ParseProgram(code);
            program.Run();

            // Assert – final command is drawto 300,250
            Assert.AreEqual(300, canvas.Xpos, "X position was not updated correctly by drawto.");
            Assert.AreEqual(250, canvas.Ypos, "Y position was not updated correctly by drawto.");
        }

        /// <summary>
        /// Unit Test #3 – runs a multiline BOOSE program containing
        /// several moveto and drawto commands and checks the final pen position.
        /// </summary>
        [TestMethod]
        public void MultiLineProgram_Final_Pen_Position_Is_Correct()
        {
            // Arrange
            var canvas = new AppCanvas(500, 500);
            var factory = new AppCommandFactory();
            var program = new StoredProgram(canvas);
            var parser = new Parser(factory, program);

            string code = @"
moveto 100,100
drawto 200,200
moveto 50,300
drawto 400,300
";

            // Act
            parser.ParseProgram(code);
            program.Run();

            // Assert – last line puts pen at (400,300)
            Assert.AreEqual(400, canvas.Xpos, "Final X position is not correct after multiline program.");
            Assert.AreEqual(300, canvas.Ypos, "Final Y position is not correct after multiline program.");
        }
    }
}
