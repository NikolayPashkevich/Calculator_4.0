using Calculator.Logic;
using Calculator.Model;

namespace Calculator.Tests
{
    public class CalculatorTests
    {
        private static readonly CalcModel _model = new();
        private readonly CalculationService _service = new(_model);

        public CalculatorTests()
        {
            CalcModel.OutputChanged += SomeEventAction;
        }

        private void SomeEventAction() { }

        /// <summary>
        /// Tests the result of calculating arithmetic expressions.
        /// </summary>
        /// <param name="expression"> Input arithmetic expression. </param>
        /// <param name="expectedResult"> Exprected result of calculating an arithmetic expression </param>
        [Theory]
        [InlineData("2+2*2", "8")]
        [InlineData("5*4", "20")]
        [InlineData("20/2+9", "19")]
        [InlineData("12*5", "60")]
        [InlineData("2+2/2", "2")]
        [InlineData("9/0", "NOT ÷ 0")]
        [InlineData("999999999+1", "EXCEEDED")]
        public void CalculateArithmeticExpression_ResultCalculation(string expression, string expectedResult)
        {
            // Arrange
            _model.Output = expression;

            // Act
            _service.GetResult();

            // Assert
            Assert.Equal(_model.Output, expectedResult);
        }


        [Theory]
        [InlineData("12+3")]
        public void ClearButton_Click_EqualsToEmptyString(string expression)
        {
            // Arrange
            _model.Output = expression;

            // Act
            _service.ClearOutputField();

            // Assert
            Assert.Empty(_model.Output);
        }

       
        [Theory]
        [InlineData("12", "/", "12/")]
        [InlineData("0", "*", "0*")]
        [InlineData("0.4", "-", "0.4-")]
        [InlineData("13+2", "+", "13+2+")]
        public void OperationButton_Click_EndsWithEnteredOperation(string expression, string operation, string expected)
        {
            // Arrange
            _model.Output = expression;

            // Act
            _service.OperationButtonPressed(operation);

            // Assert
            Assert.EndsWith(expected, _model.Expression);
        }

        
        
    }
}