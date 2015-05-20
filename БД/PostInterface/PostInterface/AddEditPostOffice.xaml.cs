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
    /// Interaction logic for AddEditPostOffice.xaml
    /// </summary>
    /// 

    public partial class AddEditPostOffice : Window
    {
        public Post_Office PO;
        public AddEditPostOffice(Post_Office po=null)
        {
            InitializeComponent();
            AreaComboBox.ItemsSource = DataBase.GetAllAreas();
            AreaComboBox.SelectedIndex = 0;
            Dispatcher.Hooks.DispatcherInactive += MyIdle;
            AreaComboBox.Focus();
            if (po != null)
            {
                IndexTextBox.Text = po.Post_Index.ToString();
                City c = DataBase.GetCityById(po.ID_City);
                Region r = DataBase.GetRegionById(c.ID_Region);
                Area a = DataBase.GetAreaById(r.ID_Area);
                AreaComboBox.SelectedItem = a;
                RegionComboBox.SelectedItem = r;
                foreach (City ci in CityComboBox.ItemsSource)
                {
                    if (ci.ID_City == c.ID_City)
                    {
                        CityComboBox.SelectedItem = ci;
                        break;
                    }
                }
            }

        }

        public void MyIdle(object sender, EventArgs e)
        {
            OkButton.IsEnabled = AreaComboBox.SelectedIndex != 0 && AreaComboBox.SelectedItem != null &&
                                 RegionComboBox.SelectedIndex != 0 && RegionComboBox.SelectedItem != null &&
                                 CityComboBox.SelectedIndex != 0 && CityComboBox.SelectedItem != null &&
                                 IndexTextBox.Text.Trim().Length > 0;

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataBase.GetPostOfficeByPostIndex(Int32.Parse(IndexTextBox.Text.Trim())) != null)
            {
                Message msg = new Message();
                msg.Show("Данное почтовое отделение уже существет");
                DialogResult = false;
            }
            else
            {
                PO = new Post_Office() { Post_Index = Int32.Parse(IndexTextBox.Text), ID_City = ((City)CityComboBox.SelectedItem).ID_City };
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AreaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AreaComboBox.SelectedItem != null)
            {
                if (AreaComboBox.SelectedIndex != 0)
                {
                    RegionComboBox.ItemsSource = DataBase.GetAllRegionsByAreaId((int)((Area)AreaComboBox.SelectedItem).ID_Area);
                }
                else RegionComboBox.ItemsSource = null;
                RegionComboBox.SelectedIndex = 0;
            }
        }

        private void RegionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RegionComboBox.SelectedItem != null)
            {
                if (RegionComboBox.SelectedIndex != 0)
                {
                    CityComboBox.ItemsSource = DataBase.GetAllCitiesByRegionId((int)((Region)RegionComboBox.SelectedItem).ID_Region);
                }
                else CityComboBox.ItemsSource = null;
                CityComboBox.SelectedIndex = 0;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                CancelButton_Click(null, null);
            if (e.Key == Key.Enter && OkButton.IsEnabled)
                OkButton_Click(null, null);
        }

        private void HouseSenderInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) || (e.Text[0] == ' ') || ((TextBox)e.Source).Text.Trim().Length >= 6)
                e.Handled = true;
        }

        private void IndexSenderTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}
