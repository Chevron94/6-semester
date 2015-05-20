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
    /// Interaction logic for EditCostRegions.xaml
    /// </summary>
    public partial class EditCostRegions : Window
    {
        bool first = true;
        
        public EditCostRegions()
        {
            InitializeComponent();
            FromAreaComboBox.ItemsSource = DataBase.GetAllAreas();
            FromAreaComboBox.SelectedIndex = 0;
            ToAreaComboBox.ItemsSource = DataBase.GetAllAreas();
            ToAreaComboBox.SelectedIndex = 0;
            CompanyComboBox.ItemsSource = DataBase.GetAllCompanies();
            CompanyComboBox.SelectedIndex = 0;
            FromAreaComboBox.Focus();
            Dispatcher.Hooks.DispatcherInactive += MyIdle;
        }

        public void MyIdle(object sender, EventArgs e)
        {
            OKButton.IsEnabled = FromCityComboBox.SelectedIndex != 0 &&
                                 ToCityComboBox.SelectedIndex != 0 &&
                                 CompanyComboBox.SelectedIndex != 0 &&
                                 CostTextBox.Text.Length != 0;
            if (first)
            {
                if (FromCityComboBox.SelectedIndex != 0 &&
                    ToCityComboBox.SelectedIndex != 0 &&
                    CompanyComboBox.SelectedIndex != 0)
                {
                    CostTextBox.Text = DataBase.GetTransportCost(((Transport_Company)CompanyComboBox.SelectedItem).ID_Transport_Company, ((City)ToCityComboBox.SelectedItem).ID_City, ((City)FromCityComboBox.SelectedItem).ID_City).ToString();
                    first = false;
                }

            }
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

        private void CostTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) || ((TextBox)e.Source).Text.Length >= 6)
                e.Handled = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                OKButton_Click(null, null);
            if (e.Key == Key.Escape)
                CancelButton_Click(null, null);
        }

        private void CostTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private void FromAreaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FromAreaComboBox.SelectedItem != null)
            {
                first = true;
                if (FromAreaComboBox.SelectedIndex != 0)
                {
                    FromRegionComboBox.ItemsSource = DataBase.GetAllRegionsByAreaId((int)((Area)FromAreaComboBox.SelectedItem).ID_Area);
                }
                else FromRegionComboBox.ItemsSource = null;
                FromRegionComboBox.SelectedIndex = 0;
            }
        }

        private void FromRegionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FromRegionComboBox.SelectedItem != null)
            {
                first = true;
                if (FromRegionComboBox.SelectedIndex != 0)
                {
                    FromCityComboBox.ItemsSource = DataBase.GetAllCitiesByRegionId((int)((Region)FromRegionComboBox.SelectedItem).ID_Region);
                }
                else FromCityComboBox.ItemsSource = null;
                FromCityComboBox.SelectedIndex = 0;
            }
        }

        private void ToAreaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ToAreaComboBox.SelectedItem != null)
            {
                first = true;
                if (ToAreaComboBox.SelectedIndex != 0)
                {
                    ToRegionComboBox.ItemsSource = DataBase.GetAllRegionsByAreaId((int)((Area)ToAreaComboBox.SelectedItem).ID_Area);
                }
                else ToRegionComboBox.ItemsSource = null;
                ToRegionComboBox.SelectedIndex = 0;
            }
        }

        private void ToRegionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ToRegionComboBox.SelectedItem != null)
            {
                first = true;
                if (ToRegionComboBox.SelectedIndex != 0)
                {
                    ToCityComboBox.ItemsSource = DataBase.GetAllCitiesByRegionId((int)((Region)ToRegionComboBox.SelectedItem).ID_Region);
                }
                else ToCityComboBox.ItemsSource = null;
                ToCityComboBox.SelectedIndex = 0;
            }
        }

        private void CompanyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)e.Source).SelectedItem != null)
            {
                first = true;
            }
        }
    }
}
