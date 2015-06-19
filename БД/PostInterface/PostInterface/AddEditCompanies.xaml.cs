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
    /// Interaction logic for AddEditCompanies.xaml
    /// </summary>
    public partial class AddEditCompanies : Window
    {
        public Transport_Company CC = new Transport_Company();
        bool IsEditł = false;
        public AddEditCompanies(bool Edit, ref Transport_Company cc)
        {
            
            InitializeComponent();
            CC = cc;
            CompanyNameComboBox.ItemsSource = DataBase.Transport_Companies.GetAllCompanies();
            if (Edit)
            {
                IsEditł = true;
                Title = "Транспортная компания";
            }
            else
            {
                Title = "Транспортная компания";
                CompanyNameComboBox.IsEnabled = false;
            }
            Dispatcher.Hooks.DispatcherInactive += MyIdle;
        }

        public void MyIdle(object sender, EventArgs e)
        {
            OkButton.IsEnabled = CompanyNameTextBox.Text.Trim().Length != 0;
            if (IsEditł)
                OkButton.IsEnabled = CompanyNameComboBox.SelectedItem != null;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            CC.Name= CompanyNameTextBox.Text.Trim();
            if (IsEditł)
                CC.ID_Transport_Company = ((Transport_Company)CompanyNameComboBox.SelectedItem).ID_Transport_Company;
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
