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
    /// Interaction logic for AddEditWorker.xaml
    /// </summary>
    /// 
    public enum Worker_States
    {
        Add_Worker,
        Edit_Worker,
    }

    public partial class AddEditWorker : Window
    {
        Worker_States State;
        Worker Worker;
        public AddEditWorker(Worker_States state, ref Worker WC)
        {
            InitializeComponent();
            State = state;
            Worker = WC;
            NameTextBox.Focus();
            OfficeComboBox.SelectedIndex = 0;
            OfficeComboBox.ItemsSource = DataBase.GetAllOfficies();
            Dispatcher.Hooks.DispatcherInactive += MyIdle;
            switch (state)
            {
                case Worker_States.Add_Worker:
                    {
                        Title = "Новый сотрудник";
                        break;
                    }
                case Worker_States.Edit_Worker:
                    {
                        Title = "Правка";
                        NameTextBox.Text = WC.FIO;
                        OfficeComboBox.SelectedItem = DataBase.GetOfficeByID(WC.ID_Office);
                        LoginTextBox.Text = WC.Login;
                        PasswordPasswordBox.Password = WC.Password;
                        SerieTextBox.Text = WC.Passport_Serie.ToString();
                        NumberTextBox.Text = WC.Passport_Number.ToString();
                        break;
                    }
            }
        }

        public void MyIdle(object sender, EventArgs e)
        {
            OKButton.IsEnabled = NameTextBox.Text.Trim().Length != 0 &&
                                 LoginTextBox.Text.Trim().Length != 0 &&
                                 PasswordPasswordBox.Password.Length != 0 &&
                                 OfficeComboBox.SelectedIndex != 0;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Message msg = new Message();
            if (DataBase.IsFreeLogin(LoginTextBox.Text.Trim()))
            {
                msg.Show("Логин уже используется");
                LoginTextBox.Focus();
            }
            else
            {
                Worker.FIO = NameTextBox.Text.Trim();
                Worker.ID_Office = OfficeComboBox.SelectedIndex;
                Worker.Login = LoginTextBox.Text.Trim() ;
                Worker.Password = PasswordPasswordBox.Password;
                Worker.Passport_Serie = Int32.Parse(SerieTextBox.Text.Trim());
                Worker.Passport_Number = Int32.Parse(NumberTextBox.Text.Trim());
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SerieTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) || ((TextBox)e.Source).Text.Trim().Length >= 4)
                e.Handled = true;
        }

        private void SerieTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) || ((TextBox)e.Source).Text.Trim().Length >= 6)
                e.Handled = true;
        }
    }
}
