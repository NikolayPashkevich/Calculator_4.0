using System.Text;

namespace Calculator.Model
{
    public class CalcModel
    {
        private StringBuilder? _expression = new();
        private StringBuilder? _output = new(10);

        public delegate void EventHandler();
        public static event EventHandler? OutputChanged;

        public string Expression
        {
            get => _expression!.ToString();
            set
            {
                if (value != string.Empty)
                {
                    _expression!.Append(value);
                }
                else if (value == string.Empty)
                {
                    _expression = new StringBuilder(value);
                }
            }
        }

        public string Output
        {
            get => _output!.ToString();
            set
            {
                if (value == string.Empty)
                {
                    _output.Clear();

                    OutputChanged!();

                    return;
                }

                if (_output.ToString() == "0" && char.IsDigit(char.Parse(value)))
                {
                    return;
                }

                if (value[1..] == _output!.ToString() && value[0] == '-')
                {
                    _output.Clear();
                    _output.Append(value);

                    OutputChanged!();
                }
                    _output = _output.Append(value);
                    OutputChanged!();
            }
        }
    }
}