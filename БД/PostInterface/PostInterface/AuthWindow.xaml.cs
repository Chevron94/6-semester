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
    /// Interaction logic for auth.xaml
    /// </summary>
    public partial class auth : Window
    {
        public Worker worker;
        public auth()
        {
            InitializeComponent();
            Title = "Авторизация";
            LoginTextBox.Focus();
            //DialogResult = false;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            worker = DataBase.GetWorkerByLoginAndPassword(LoginTextBox.Text, PasswordPasswordBox.Password);
            if (worker == null)
                (new Message()).Show("Неверный логин/пароль");
            else (new Message()).Show("Авторизация прошла успешно");
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                OkButton_Click(null, null);
            if (e.Key == Key.Escape)
                CancelButton_Click(null, null);
        }

    }
}
