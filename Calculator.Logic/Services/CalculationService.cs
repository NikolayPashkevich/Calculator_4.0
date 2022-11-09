using Calculator.Model;
using System.Data;

namespace Calculator.Logic
{
    public class CalculationService
    {
        private readonly CalcModel _model;

        public CalculationService(CalcModel model)
        {
            _model = model;
        }

        public string GetResultString() => _model.Output;

        
        public void GetResult()
        {
            _model.Expression = _model.Output;

            EditExpression(_model.Expression);

            CalculateArithmeticExpression(_model.Expression);
        }

        public void CalculateArithmeticExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            if (char.IsSymbol(expression[^1]) || char.IsPunctuation(expression[^1]))
            {
                return;
            }

            if (expression.StartsWith('-'))
            {
                expression = "0-" + expression[1..];
            }

            if (expression.Contains("/0"))
            {
                EditResult("NOT ÷ 0");
                return;
            }

            string result;

            try
            {
                if (expression.Contains('*')||expression.Contains('/'))
                {
                    string temp = string.Empty;
                    if (expression.Contains('*'))
                    {
                        temp = expression.Split('*')[0];
                    }
                    else
                    {
                        temp = expression.Split('/')[0];
                    }
                    if (temp.Contains('+')||temp.Contains('-'))
                    {
                        expression = expression.Replace(temp, $"({temp})");
                    }
                }
                result = new DataTable()
                            .Compute(expression, "")
                            .ToString()!
                            .Replace(',', '.');
            }
            catch
            {
                result = "EXCEEDED";
            }

            if ((result.Length >= 9 ) || result == "∞" || result == "не число")
            {
                result = "EXCEEDED";
            }

            EditResult(result);

            _model.Expression = string.Empty;
        }

        public void ClearOutputField()
        {
            if (_model.Output.Length == 0)
            {
                return;
            }

            _model.Output = string.Empty;
        }

        public void NumberButtonPressed(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            if (!char.IsDigit(input[0]) || _model.Output.Length > 8)
            {
                return;
            }

            if (_model.Output == "EXCEEDED" || _model.Output == "NOT ÷ 0")
            {
                EditResult(input);
                return;
            }

            _model.Output = input;
        }

        public void OperationButtonPressed(string input)
        {
            if (_model.Output == "EXCEEDED" || _model.Output == "NOT ÷ 0")
            {
                return;
            }

            if (_model.Output.Length == 0)
            {
                _model.Output.Append('0');
                return;
            }

            if ((char.IsSymbol(_model.Output[^1]) || char.IsPunctuation(_model.Output[^1])) && input != "-")
            {
                _model.Output.Remove(_model.Output.Length - 1, 1);
            }

            _model.Output = input;

            SaveAndClearOutputString();
        }

        public void NegateNubmer()
        {
            if (_model.Output == "EXCEEDED" || _model.Output == "NOT ÷ 0")
            {
                return;
            }

            double number = double.Parse(_model.Output);

            EditResult((number * (-1)).ToString());
        }

        private void SaveAndClearOutputString()
        {
            if (string.IsNullOrEmpty(_model.Output))
            {
                return;
            }

            _model.Expression = _model.Output;
            _model.Output = string.Empty;
        }

        private void EditResult(string value)
        {
            _model.Output = string.Empty;
            _model.Output = value;
        }

        private void EditExpression(string expression)
        {
            _model.Expression = string.Empty;
            _model.Expression = expression;
        }
    }
}
