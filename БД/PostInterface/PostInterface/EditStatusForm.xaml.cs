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
    /// Interaction logic for EditStatusForm.xaml
    /// </summary>
    public partial class EditStatusForm : Window
    {
        public Track_List TEC { get; set; }
        public EditStatusForm(int ID_Letter)
        {
            InitializeComponent();
            TEC = new Track_List();
            TEC.ID_Consignment = ID_Letter;
            Dispatcher.Hooks.DispatcherInactive += MyIdle;
            IdLetterTextBox.Text = ID_Letter.ToString();

            IndexComboBox.ItemsSource = DataBase.GetAllPostOfficies();
            IndexComboBox.SelectedIndex = 0;
            StatusComboBox.SelectedIndex = 0;
            FullStatusComboBox.SelectedIndex = 0;
            StatusComboBox.ItemsSource = DataBase.GetAllLetterStatuses();
        }

        public void MyIdle(object sender, EventArgs e)
        {
            OkButton.IsEnabled = IndexComboBox.SelectedItem!= null && IndexComboBox.SelectedIndex > 0 &&
                                    StatusComboBox.SelectedIndex != 0 &&
                                    FullStatusComboBox.SelectedIndex !=0;
            FullStatusComboBox.IsEnabled = StatusComboBox.SelectedItem != null && StatusComboBox.SelectedIndex != 0;
        }

        private void IndexTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
                e.Handled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            TEC.Post_Index = ((Post_Office)IndexComboBox.SelectedValue).Post_Index;
            TEC.Date = DateTime.Now;
            //TEC.Status = StatusComboBox.SelectedItem.ToString();
            TEC.ID_Full_Letter_Status = ((Full_Letter_Status)FullStatusComboBox.SelectedItem).ID_Full_Letter_Status;
            DialogResult = true;
            Close();
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusComboBox.SelectedItem != null)
            {
                if (StatusComboBox.SelectedIndex != 0)
                {
                    FullStatusComboBox.ItemsSource = DataBase.GetAllFullLetterStatusesByLetterStatusId((int)((Letter_Status)StatusComboBox.SelectedItem).ID_Letter_Status);
                }
                else FullStatusComboBox.ItemsSource = null;
                FullStatusComboBox.SelectedIndex = 0;
            }
        }

        private void IndexTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private void IndexComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IndexComboBox.SelectedValue != null)
            {
                if (IndexComboBox.SelectedIndex != 0)
                {
                    CityTextBox.Text = DataBase.GetCityById(((Post_Office)IndexComboBox.SelectedItem).ID_City).ToString();
                }
                else CityTextBox.Text = "";
            }
        }
    }
}
