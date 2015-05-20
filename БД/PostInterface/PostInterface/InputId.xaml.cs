using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PostInterface
{
    /// <summary>
    /// Interaction logic for InputId.xaml
    /// </summary>
    public partial class InputId : Window
    {
        public InputId(string text)
        {
            InitializeComponent();
            InputIDTextBox.Focus();
            InputIDLabel.Content = text;
            Dispatcher.Hooks.DispatcherInactive += MyIdle;
        }

        public void MyIdle(object sender, EventArgs e)
        {
            OKButton.IsEnabled = InputIDTextBox.Text.Length > 0;
        }

        private void InputIDTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) || ((TextBox)e.Source).Text.Length >= 12)
                e.Handled = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                CancelButton_Click(null, null);
            if (e.Key == Key.Enter && OKButton.IsEnabled)
                OKButton_Click(null, null);
        }
    }
}
