using Calculator.Logic;
using Calculator.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Calculator.App
{
    public partial class MainWindow : Window
    {
        private static readonly CalcModel _model = new();
        private readonly CalculationService _service = new(_model);
        public MainWindow()
        {
            InitializeComponent();
            CalcModel.OutputChanged += RefreshOutputField;
        }

        private void RefreshOutputField() => textDisplay.Text = _service.GetResultString();

        private void NumberNutton_Click(object sender, RoutedEventArgs e)
        {
            _service.NumberButtonPressed((string)((Button)sender).Content);
        }

        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            _service.OperationButtonPressed((string)((Button)sender).Content);

            textDisplay.Text = string.Empty;
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            _service.GetResult();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _service.ClearOutputField();
        }
        public void OutputBox_TextChanged(object sender, EventArgs e)
        {
            string output = textDisplay.Text;

            if (output.Length == 0)
            {
                ButtonResult.IsEnabled = false;
                return;
            }

            if (char.IsPunctuation(output[^1]) || char.IsSymbol(output[^1]))
            {
                ButtonResult.IsEnabled = false;
                return;
            }


            ButtonResult.IsEnabled = true;
        }
        private void ButtonNegation_Click(object sender, EventArgs e) => _service.NegateNubmer();

    }
}
