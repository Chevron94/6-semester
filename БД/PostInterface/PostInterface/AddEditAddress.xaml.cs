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
    /// Interaction logic for AddEditAddress.xaml
    /// </summary>
    /// 
    public enum Address_States  
    {
        Add_Area,
        Edit_Area,
        Add_Region,
        Edit_Region,
        Add_City,
        Edit_City,
        Add_Street,
        Edit_Street
    }

    public partial class AddEditAddress : Window
    {
        Address_States State;
        public Street S;
        public Area A;
        public Region R;
        public City C;
        public AddEditAddress(Address_States state)
        {
            InitializeComponent();
            State = state;
            AreaComboBox.ItemsSource = DataBase.Areas.GetAllAreas();
            Dispatcher.Hooks.DispatcherInactive += MyIdle;
            switch (state)
            {
                case Address_States.Add_Area:
                    {
                        InputNewValue.Focus();
                        Title = "Добавление области";
                        AreaComboBox.IsEnabled = false;
                        RegionComboBox.IsEnabled = false;
                        CityComboBox.IsEnabled = false;
                        StreetComboBox.IsEnabled = false;
                        TypeComboBox.IsEnabled = false;
                        A = new Area();
                        break;
                    };
                case Address_States.Edit_Area:
                    {
                        AreaComboBox.Focus();
                        Title = "Переименование области";
                        AreaComboBox.IsEnabled = true;
                        RegionComboBox.IsEnabled = false;
                        CityComboBox.IsEnabled = false;
                        StreetComboBox.IsEnabled = false;
                        TypeComboBox.IsEnabled = false;
                        break;
                    };
                case Address_States.Add_Region:
                    {
                        AreaComboBox.Focus();
                        Title = "Добавление района";
                        AreaComboBox.IsEnabled = true;
                        RegionComboBox.IsEnabled = false;
                        CityComboBox.IsEnabled = false;
                        StreetComboBox.IsEnabled = false;
                        TypeComboBox.IsEnabled = false;
                        R = new Region();
                        break;
                    };
                case Address_States.Edit_Region:
                    {
                        AreaComboBox.Focus();
                        Title = "Правка района";
                        AreaComboBox.IsEnabled = true;
                        RegionComboBox.IsEnabled = true;
                        CityComboBox.IsEnabled = false;
                        StreetComboBox.IsEnabled = false;
                        TypeComboBox.IsEnabled = false;
                        break;
                    };
                case Address_States.Add_City:
                    {
                        AreaComboBox.Focus();
                        Title = "Добавление населенного пункта";
                        AreaComboBox.IsEnabled = true;
                        RegionComboBox.IsEnabled = true;
                        CityComboBox.IsEnabled = false;
                        StreetComboBox.IsEnabled = false;
                        TypeComboBox.IsEnabled = true;
                        TypeComboBox.ItemsSource = DataBase.Cities.GetAllCityTypes();
                        TypeComboBox.SelectedIndex = 0;
                        C = new City();
                        break;
                    };
                case Address_States.Edit_City:
                    {
                        AreaComboBox.Focus();
                        Title = "Переименование населенного пункта";
                        AreaComboBox.IsEnabled = true;
                        RegionComboBox.IsEnabled = true;
                        CityComboBox.IsEnabled = true;
                        StreetComboBox.IsEnabled = false;
                        TypeComboBox.IsEnabled = true;
                        TypeComboBox.ItemsSource = DataBase.Cities.GetAllCityTypes();
                        
                        break;
                    };
                case Address_States.Add_Street:
                    {
                        AreaComboBox.Focus();
                        Title = "Добавление улицы";
                        AreaComboBox.IsEnabled = true;
                        RegionComboBox.IsEnabled = true;
                        CityComboBox.IsEnabled = true;
                        StreetComboBox.IsEnabled = false;
                        TypeComboBox.IsEnabled = true;
                        TypeComboBox.ItemsSource = DataBase.Streets.GetAllStreetTypes();
                        S = new Street();
                        break;
                    };
                case Address_States.Edit_Street:
                    {
                        AreaComboBox.Focus();
                        Title = "Переименование улицы";
                        AreaComboBox.IsEnabled = true;
                        RegionComboBox.IsEnabled = true;
                        CityComboBox.IsEnabled = true;
                        StreetComboBox.IsEnabled = true;
                        TypeComboBox.IsEnabled = true;
                        TypeComboBox.ItemsSource = DataBase.Streets.GetAllStreetTypes();
                        break;
                    };
                    
            }
        }
        public void MyIdle(object sender, EventArgs e)
        {
            switch (State)
            {
                case Address_States.Add_Area:
                    {
                        OKButton.IsEnabled = InputNewValue.Text.Trim().Length != 0;
                        TypeComboBox.IsEnabled = false;
                        break;
                    };
                case Address_States.Edit_Area:
                case Address_States.Add_Region:
                    {
                        OKButton.IsEnabled = InputNewValue.Text.Trim().Length != 0 &&
                                             AreaComboBox.SelectedIndex != 0;
                        TypeComboBox.IsEnabled = false;
                        break;
                    };
                case Address_States.Edit_Region:
                case Address_States.Add_City:
                    {
                        OKButton.IsEnabled = InputNewValue.Text.Trim().Length != 0 &&
                                             AreaComboBox.SelectedIndex != 0 &&
                                             RegionComboBox.SelectedIndex != 0;
                        RegionComboBox.IsEnabled = AreaComboBox.SelectedIndex != 0 && AreaComboBox.SelectedItem != null;
                        if (Address_States.Add_City == State)
                        {
                            TypeComboBox.IsEnabled = RegionComboBox.SelectedIndex != 0 && RegionComboBox.SelectedItem != null;
                        }
                        break;
                    };

                case Address_States.Edit_City:
                case Address_States.Add_Street:
                    {
                        OKButton.IsEnabled = InputNewValue.Text.Trim().Length != 0 &&
                                             AreaComboBox.SelectedIndex != 0 &&
                                             RegionComboBox.SelectedIndex != 0 &&
                                             CityComboBox.SelectedIndex != 0;
                        RegionComboBox.IsEnabled = AreaComboBox.SelectedIndex != 0 && AreaComboBox.SelectedItem != null;
                        CityComboBox.IsEnabled = RegionComboBox.SelectedIndex != 0 && RegionComboBox.SelectedItem != null;
                        TypeComboBox.IsEnabled = CityComboBox.SelectedIndex != 0 && CityComboBox.SelectedItem != null;
                        break;
                    };
                case Address_States.Edit_Street:
                    {
                        OKButton.IsEnabled = InputNewValue.Text.Trim().Length != 0 &&
                                             AreaComboBox.SelectedIndex != 0 &&
                                             RegionComboBox.SelectedIndex != 0 &&
                                             CityComboBox.SelectedIndex != 0 &&
                                             StreetComboBox.SelectedIndex != 0;
                        RegionComboBox.IsEnabled = AreaComboBox.SelectedIndex != 0 && AreaComboBox.SelectedItem != null;
                        CityComboBox.IsEnabled = RegionComboBox.SelectedIndex != 0 && RegionComboBox.SelectedItem != null;
                        StreetComboBox.IsEnabled = CityComboBox.SelectedIndex != 0 && CityComboBox.SelectedItem != null;
                        TypeComboBox.IsEnabled = StreetComboBox.SelectedIndex != 0 && StreetComboBox.SelectedItem != null;
                        break;
                    };

            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            switch (State)
            {
                case Address_States.Add_Street:
                case Address_States.Edit_Street:
                    {
                        S.Name = InputNewValue.Text.Trim();
                        S.ID_Street_Type = ((Street_Type)TypeComboBox.SelectedItem).ID_Street_Type;
                        break;
                    }
                case Address_States.Add_Area:
                case Address_States.Edit_Area:
                    {
                        A.Name = InputNewValue.Text.Trim();
                        break;
                    }
                case Address_States.Add_Region:
                case Address_States.Edit_Region:
                    {
                        R.Name = InputNewValue.Text.Trim();
                        break;
                    }
                case Address_States.Edit_City:
                case Address_States.Add_City:
                    {
                        C.Name = InputNewValue.Text.Trim();
                        C.ID_City_Type = ((City_Type)TypeComboBox.SelectedItem).ID_City_Type;
                        break;
                    }
            }
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

        private void AreaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AreaComboBox.SelectedItem != null)
            {
                if (AreaComboBox.SelectedIndex != 0)
                {
                    A = (Area)AreaComboBox.SelectedItem;
                    RegionComboBox.ItemsSource = DataBase.Regions.GetAllRegionsByAreaId((int)((Area)AreaComboBox.SelectedItem).ID_Area);
                    if (State == Address_States.Edit_Area)
                        InputNewValue.Text = A.Name;
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
                    R = (Region)RegionComboBox.SelectedItem;
                    CityComboBox.ItemsSource = DataBase.Cities.GetAllCitiesByRegionId((int)((Region)RegionComboBox.SelectedItem).ID_Region);
                    if (State == Address_States.Edit_Region)
                        InputNewValue.Text = R.Name;
                }
                else CityComboBox.ItemsSource = null;
                CityComboBox.SelectedIndex = 0;
            }
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CityComboBox.SelectedItem != null)
            {
                if (CityComboBox.SelectedIndex != 0)
                {
                    StreetComboBox.ItemsSource = DataBase.Streets.GetAllStreetsByCityId((int)((City)CityComboBox.SelectedItem).ID_City);
                    C = (City)CityComboBox.SelectedItem;
                    if (State == Address_States.Edit_City)
                    {
                        InputNewValue.Text = C.Name;
                        TypeComboBox.SelectedItem = DataBase.Cities.GetCityTypeById(((City)(CityComboBox.SelectedItem)).ID_City_Type);
                    }
                }
                else StreetComboBox.ItemsSource = null;
                StreetComboBox.SelectedIndex = 0;
            }
        }

        private void StreetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StreetComboBox.SelectedItem != null)
            {
                if (StreetComboBox.SelectedIndex != 0)
                {
                    S = (Street)StreetComboBox.SelectedItem;
                    if (State == Address_States.Edit_Street)
                    {
                        InputNewValue.Text = S.Name;
                        TypeComboBox.SelectedItem = DataBase.Streets.GetStreetTypeById(((Street)(StreetComboBox.SelectedItem)).ID_Street_Type);
                    }
                    
                }
            }
        }

        
    }
}
