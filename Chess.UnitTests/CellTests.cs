using NUnit.Framework;
using Chess.Models;
using NUnit.Framework.Legacy;

namespace Chess.UnitTests
{
    public class CellTests
    {
        [Test]
        public void Test_CellColorChange()
        {
            // Arrange
            var cell = new Cell(0, 0, Color.White);

            // Act
            cell.FieldColor = "#FFC300";

            // Assert
            ClassicAssert.AreEqual("#FFC300", cell.FieldColor);
        }
    }
}