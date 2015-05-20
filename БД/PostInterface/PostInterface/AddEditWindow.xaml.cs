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
    /// Interaction logic for AddEditWindow.xaml
    /// </summary>
    /// 
    public partial class AddEditWindow : Window
    {
        public Consignment Letter { get; set; }
        public Client Sender  { get; set; }
        public Client Reciever { get; set; }
        public Area Area { get; set; }
        public Region Region { get; set; }
        public City City { get; set; }
        public Street Street { get; set; }

        bool Servise = false;
        bool CompanyRemoved = false;
        bool IsAdd;

        public AddEditWindow(Consignment item, bool service = false)
        {
            InitializeComponent();
            AreaSenderComboBox.ItemsSource = DataBase.GetAllAreas();
            AreaRecieverComboBox.ItemsSource = DataBase.GetAllAreas();
            CompanyComboBox.ItemsSource = DataBase.GetAllCompanies();
            IndexSenderComboBox.ItemsSource = DataBase.GetAllPostOfficies();
            IndexRecieverComboBox.ItemsSource = DataBase.GetAllPostOfficies();
            IndexSenderComboBox.ItemsSource = DataBase.GetAllPostOfficies();
            LetterTypeComboBox.ItemsSource = DataBase.GetAllConsigmentTypes();
            Servise = service;
            GetComboBox.ItemsSource = DataBase.GetAllWorkers();
            GetComboBox.IsEnabled = false;
            GiveComboBox.ItemsSource = DataBase.GetAllWorkers();
            GiveComboBox.SelectedIndex = 0;
            GetComboBox.SelectedIndex = 0;
            IndexSenderComboBox.SelectedIndex = 0;
            IndexRecieverComboBox.SelectedIndex = 0;
            SenderNameTextBox.Focus();
            //
            Dispatcher.Hooks.DispatcherInactive += MyIdle;
            if (item == null)
            {
                Title = "Добавление";
                Letter = new Consignment();
                Sender = new Client();
                Reciever = new Client();
                IsAdd = true;
            }
            else
            {
                IsAdd = false;
                Title = "Редактирование";
                Letter = item;


                if (Letter.ID_Transport_Company == 0)
                {
                    CompanyRemoved = true;
                    CompanyComboBox.ItemsSource = DataBase.GetAllCompanies();
                }
                else
                {
                    CompanyRemoved = false;
                    Transport_Company CC = new Transport_Company();
                    CC = DataBase.GetCompanyById(Letter.ID_Transport_Company);
                    CompanyComboBox.SelectedItem = CC;
                }

                LetterTypeComboBox.SelectedItem = DataBase.GetConsigmentTypeById(Letter.ID_Consignment_Type);
                WeightTextBox.Text = Letter.Weight.ToString();
                PriceTextBox.Text = Letter.Worth.ToString();
                CostTextBox.Text = Letter.Total_Cost.ToString();

                Sender = DataBase.GetClientById(Letter.ID_Sender);
                Street = DataBase.GetStreetById(Sender.ID_Street);
                City = DataBase.GetCityById(Street.ID_City);
                Region = DataBase.GetRegionById(City.ID_Region);
                Area = DataBase.GetAreaById(Region.ID_Area);

                SenderNameTextBox.Text = Sender.Name;
                IndexSenderComboBox.SelectedValue = DataBase.GetPostOfficeByPostIndex(Sender.Index);
                AreaSenderComboBox.SelectedItem = Area;
                RegionSenderComboBox.SelectedItem = Region;
                foreach (City c in CitySenderComboBox.ItemsSource)
                {
                    if (c.ID_City == City.ID_City)
                    {
                        CitySenderComboBox.SelectedItem = c;
                        break;
                    }
                }

                foreach (Street s in StreetSenderComboBox.ItemsSource)
                {
                    if (s.ID_Street == Street.ID_Street)
                    {
                        StreetSenderComboBox.SelectedItem = s;
                        break;
                    }
                }
               // StreetSenderComboBox.SelectedValue = Street;
                
                HouseSenderInput.Text = Sender.House_Number.ToString();
                BuildingSenderTextBox.Text = Sender.Building == null ? "" : Sender.Building;
                FlatSenderTextBox.Text = Sender.Flat == null ? "" : Sender.Flat.ToString();

                Reciever = DataBase.GetClientById(Letter.ID_Reciever);
                Street = DataBase.GetStreetById(Reciever.ID_Street);
                City = DataBase.GetCityById(Street.ID_City);
                Region = DataBase.GetRegionById(City.ID_Region);
                Area = DataBase.GetAreaById(Region.ID_Area);

                RecieverNameTextBox.Text = Reciever.Name;
                IndexRecieverComboBox.SelectedItem = DataBase.GetPostOfficeByPostIndex(Reciever.Index);
                AreaRecieverComboBox.SelectedItem = Area;
                RegionRecieverComboBox.SelectedItem = Region;

                foreach (City c in CityRecieverComboBox.ItemsSource)
                {
                    if (c.ID_City == City.ID_City)
                    {
                        CityRecieverComboBox.SelectedItem = c;
                        break;
                    }
                }

                foreach (Street s in StreetRecieverComboBox.ItemsSource)
                {
                    if (s.ID_Street == Street.ID_Street)
                    {
                        StreetRecieverComboBox.SelectedItem = s;
                        break;
                    }
                }
                
                HouseRecieverInput.Text = Reciever.House_Number.ToString();
                BuildingRecieverTextBox.Text = Reciever.Building == null ? "" : Reciever.Building;
                FlatRecieverTextBox.Text = Reciever.Flat == null ? "" : Reciever.Flat.ToString();

                if (service)
                {
                    GetLabel.Visibility = System.Windows.Visibility.Visible;
                    GetComboBox.Visibility = System.Windows.Visibility.Visible;
                    GiveComboBox.Visibility = System.Windows.Visibility.Visible;
                    GiveLabel.Visibility = System.Windows.Visibility.Visible;

                    Worker WC = DataBase.GetWorkerByID(Letter.ID_Office_Worker);
                    GetComboBox.SelectedItem = WC;
                    WC = DataBase.GetWorkerByID(Letter.ID_Courier);
                    if (WC==null)
                    {
                        GiveComboBox.ItemsSource = DataBase.GetWorkersWithoutRemoved();
                        GiveComboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        GiveComboBox.SelectedItem = WC;
                    }
                    

                    CompanyComboBox.IsEnabled = false;
                    LetterTypeComboBox.IsEnabled = false;
                    WeightTextBox.IsEnabled = false;
                    PriceTextBox.IsEnabled = false;
                    CostTextBox.IsEnabled = false;

                    SenderNameTextBox.IsEnabled = false;
                    IndexSenderComboBox.IsEnabled = false;
                    AreaSenderComboBox.IsEnabled = false;
                    RegionSenderComboBox.IsEnabled = false;
                    CitySenderComboBox.IsEnabled = false;
                    StreetSenderComboBox.IsEnabled = false;
                    HouseSenderInput.IsEnabled = false;
                    BuildingSenderTextBox.IsEnabled = false;
                    FlatSenderTextBox.IsEnabled = false;

                    RecieverNameTextBox.IsEnabled = false;
                    IndexRecieverComboBox.IsEnabled = false;
                    AreaRecieverComboBox.IsEnabled = false;
                    RegionRecieverComboBox.IsEnabled = false;
                    CityRecieverComboBox.IsEnabled = false;
                    StreetRecieverComboBox.IsEnabled = false;
                    HouseRecieverInput.IsEnabled = false;
                    BuildingRecieverTextBox.IsEnabled = false;
                    FlatRecieverTextBox.IsEnabled = false;
                }
            }
            
        }

        public void MyIdle(object sender, EventArgs e)
        {

            if (!Servise)
            {
                RegionSenderComboBox.IsEnabled = AreaSenderComboBox.SelectedIndex > 0;
                CitySenderComboBox.IsEnabled = RegionSenderComboBox.SelectedIndex > 0;
                StreetSenderComboBox.IsEnabled = CitySenderComboBox.SelectedIndex > 0;
                HouseSenderInput.IsEnabled = StreetSenderComboBox.SelectedIndex > 0;
                BuildingSenderTextBox.IsEnabled = HouseSenderInput.Text.Length > 0;
                FlatSenderTextBox.IsEnabled = BuildingSenderTextBox.IsEnabled;
                RegionRecieverComboBox.IsEnabled = AreaRecieverComboBox.SelectedIndex > 0;
                CityRecieverComboBox.IsEnabled = RegionRecieverComboBox.SelectedIndex > 0;
                StreetRecieverComboBox.IsEnabled = CityRecieverComboBox.SelectedIndex > 0;
                HouseRecieverInput.IsEnabled = StreetRecieverComboBox.SelectedIndex > 0;
                BuildingRecieverTextBox.IsEnabled = HouseRecieverInput.Text.Length > 0;
                FlatRecieverTextBox.IsEnabled = BuildingRecieverTextBox.IsEnabled;

            }
            OKButton.IsEnabled = !(AreaSenderComboBox.SelectedIndex == 0 ||
                                    AreaRecieverComboBox.SelectedIndex == 0 ||
                                    RegionSenderComboBox.SelectedIndex == 0 ||
                                    RegionRecieverComboBox.SelectedIndex == 0 ||
                                    CityRecieverComboBox.SelectedIndex == 0 ||
                                    CitySenderComboBox.SelectedIndex == 0 ||
                                    (CompanyComboBox.SelectedIndex == 0 && !CompanyRemoved) ||
                                    LetterTypeComboBox.SelectedIndex == 0 ||
                                    StreetRecieverComboBox.SelectedIndex == 0 ||
                                    StreetSenderComboBox.SelectedIndex == 0 ||
                                    SenderNameTextBox.Text.Trim().Length == 0 ||
                                    RecieverNameTextBox.Text.Trim().Length == 0 ||
                                    HouseRecieverInput.Text.Trim().Length == 0 ||
                                    HouseSenderInput.Text.Trim().Length == 0 ||
                                    WeightTextBox.Text.Trim().Length == 0 ||
                                    PriceTextBox.Text.Trim().Length == 0 ||
                                    IndexRecieverComboBox.SelectedIndex == 0 || IndexRecieverComboBox.SelectedItem == null||
                                    IndexSenderComboBox.SelectedIndex == 0 || IndexSenderComboBox.SelectedItem == null);
            if (!(((Consignment_Type)(LetterTypeComboBox.SelectedItem)).ID_Consignment_Type == 4 || ((Consignment_Type)(LetterTypeComboBox.SelectedItem)).ID_Consignment_Type == 5))
            {
                PriceTextBox.Text = "0";
                PriceTextBox.IsEnabled = false;
            }
            else PriceTextBox.IsEnabled = true;

            if (OKButton.IsEnabled)
                if ((((Consignment_Type)(LetterTypeComboBox.SelectedItem)).ID_Consignment_Type == 4 || ((Consignment_Type)(LetterTypeComboBox.SelectedItem)).ID_Consignment_Type == 5) && (Int32.Parse(PriceTextBox.Text.Trim()) < 1))
                    OKButton.IsEnabled = false;
            if (OKButton.IsEnabled && Servise)
            {
                OKButton.IsEnabled = GetComboBox.SelectedIndex != 0;
            }

            //////////////////////
            if (CompanyComboBox.SelectedIndex != 0 && CompanyComboBox.SelectedItem != null && 
                CitySenderComboBox.SelectedIndex != 0 && CitySenderComboBox.SelectedItem != null &&
                CityRecieverComboBox.SelectedIndex != 0 && CityRecieverComboBox.SelectedItem != null)
            {
                long tmp = DataBase.GetTransportCost(((Transport_Company)CompanyComboBox.SelectedItem).ID_Transport_Company,
                                                    ((City)CitySenderComboBox.SelectedItem).ID_City,
                                                    ((City)CityRecieverComboBox.SelectedItem).ID_City);
                if (WeightTextBox.Text.Length > 0)
                    tmp += Int32.Parse(WeightTextBox.Text);
                if (((Consignment_Type)(LetterTypeComboBox.SelectedItem)).ID_Consignment_Type == 4 || ((Consignment_Type)(LetterTypeComboBox.SelectedItem)).ID_Consignment_Type == 5)
                {
                    if (PriceTextBox.Text.Trim().Length > 0)
                        tmp += Int32.Parse(PriceTextBox.Text.Trim()) * ((Consignment_Type)(LetterTypeComboBox.SelectedItem)).ID_Consignment_Type;
                }
                else tmp += (((Consignment_Type)(LetterTypeComboBox.SelectedItem)).ID_Consignment_Type - 1) * 10;
                CostTextBox.Text = tmp.ToString();
            }

        }

        private void HouseSenderInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) || (e.Text[0] == ' ') || ((TextBox)e.Source).Text.Trim().Length>=6)
                e.Handled = true;
        }

        private void BuildingSenderTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (((TextBox)e.Source).Text.Trim().Length >= 2)
                e.Handled = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (GiveComboBox.SelectedIndex != 0)
                Letter.ID_Courier = ((Worker)(GiveComboBox.SelectedItem)).ID_Worker;
            Letter.ID_Transport_Company = ((Transport_Company)(CompanyComboBox.SelectedItem)).ID_Transport_Company;

            

            Reciever.Name = RecieverNameTextBox.Text.Trim();
            Reciever.Index = ((Post_Office)(IndexRecieverComboBox.SelectedItem)).Post_Index;
            Reciever.House_Number = Int32.Parse(HouseRecieverInput.Text.Trim());
            if (BuildingRecieverTextBox.Text.Trim().Length > 0)
                Reciever.Building = BuildingRecieverTextBox.Text.Trim();
            else Reciever.Building = null;
            if (FlatRecieverTextBox.Text.Trim().Length > 0)
                Reciever.Flat = Int32.Parse(FlatRecieverTextBox.Text.Trim());
            else Reciever.Flat = null;
            Reciever.ID_Street = ((Street)StreetRecieverComboBox.SelectedItem).ID_Street;

            Sender.Name = SenderNameTextBox.Text.Trim();
            Sender.Index = ((Post_Office)(IndexSenderComboBox.SelectedItem)).Post_Index;
            Sender.House_Number = Int32.Parse(HouseSenderInput.Text.Trim());
            if (BuildingSenderTextBox.Text.Trim().Length > 0)
                Sender.Building = BuildingSenderTextBox.Text.Trim();
            else Sender.Building = null;
            if (FlatSenderTextBox.Text.Trim().Length > 0)
                Sender.Flat = Int32.Parse(FlatSenderTextBox.Text.Trim());
            else Sender.Flat = null;
            Sender.ID_Street = ((Street)StreetSenderComboBox.SelectedItem).ID_Street;
            
            Letter.ID_Consignment_Type = (int)((Consignment_Type)LetterTypeComboBox.SelectedItem).ID_Consignment_Type;

            if (Letter.ID_Consignment_Type != 4 && Letter.ID_Consignment_Type != 5)
                Letter.Worth = 0;
            else Letter.Worth = Int32.Parse(PriceTextBox.Text.Trim());
            Letter.Total_Cost = Int32.Parse(CostTextBox.Text.Trim());
            Letter.Weight = Int32.Parse(WeightTextBox.Text.Trim());

            if (IsAdd)
            {
                DataBase.AddClient(Sender);
                DataBase.AddClient(Reciever);
                Letter.ID_Sender = Sender.ID_Client;
                Letter.ID_Reciever = Reciever.ID_Client;
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

        private void AreaSenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (AreaSenderComboBox.SelectedItem != null)
            {
                if (AreaSenderComboBox.SelectedIndex != 0)
                {
                    RegionSenderComboBox.ItemsSource = DataBase.GetAllRegionsByAreaId((int)((Area)AreaSenderComboBox.SelectedItem).ID_Area);
                }
                else RegionSenderComboBox.ItemsSource = null;
                RegionSenderComboBox.SelectedIndex = 0;
            }
            else RegionSenderComboBox.SelectedIndex = 0;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void RegionSenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RegionSenderComboBox.SelectedItem != null)
            {
                if (RegionSenderComboBox.SelectedIndex != 0)
                {
                    CitySenderComboBox.ItemsSource = DataBase.GetAllCitiesByRegionId((int)((Region)RegionSenderComboBox.SelectedItem).ID_Region);
                }
                else CitySenderComboBox.ItemsSource = null;
                CitySenderComboBox.SelectedIndex = 0;
            }
            else CitySenderComboBox.SelectedIndex = 0;
        }

        private void CitySenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CitySenderComboBox.SelectedItem != null)
            {
                if (CitySenderComboBox.SelectedIndex != 0)
                {
                    StreetSenderComboBox.ItemsSource = DataBase.GetAllStreetsByCityId((int)((City)CitySenderComboBox.SelectedItem).ID_City);
                }
                else StreetSenderComboBox.ItemsSource = null;
                StreetSenderComboBox.SelectedIndex = 0;
            }
            else StreetSenderComboBox.SelectedIndex = 0;
        }

        private void AreaRecieverComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AreaRecieverComboBox.SelectedItem != null)
            {
                if (AreaRecieverComboBox.SelectedIndex != 0)
                {
                    RegionRecieverComboBox.ItemsSource = DataBase.GetAllRegionsByAreaId((int)((Area)AreaRecieverComboBox.SelectedItem).ID_Area);
                }
                else RegionRecieverComboBox.ItemsSource = null;
                RegionRecieverComboBox.SelectedIndex = 0;
            }
            else RegionRecieverComboBox.SelectedIndex = 0;
        }

        private void RegionRecieverComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RegionRecieverComboBox.SelectedItem != null)
            {
                if (RegionRecieverComboBox.SelectedIndex != 0)
                {
                    CityRecieverComboBox.ItemsSource = DataBase.GetAllCitiesByRegionId((int)((Region)RegionRecieverComboBox.SelectedItem).ID_Region);
                }
                else CityRecieverComboBox.ItemsSource = null;
                CityRecieverComboBox.SelectedIndex = 0;
            }
            else CityRecieverComboBox.SelectedIndex = 0;
        }

        private void CityRecieverComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CityRecieverComboBox.SelectedItem != null)
            {
                if (CityRecieverComboBox.SelectedIndex != 0)
                {
                    StreetRecieverComboBox.ItemsSource = DataBase.GetAllStreetsByCityId((int)((City)CityRecieverComboBox.SelectedItem).ID_City);
                }
                else StreetRecieverComboBox.ItemsSource = null;
                StreetRecieverComboBox.SelectedIndex = 0;
            }
            else StreetRecieverComboBox.SelectedIndex = 0; ;
        }

        private void IndexSenderTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void IndexSenderComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.Text, 0);
        }

        private void IndexSenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IndexSenderComboBox.SelectedValue != null)
            {
                if (IndexSenderComboBox.SelectedIndex != 0)
                {
                    City City = DataBase.GetCityById(((Post_Office)IndexSenderComboBox.SelectedItem).ID_City);
                    Region Region = DataBase.GetRegionById(City.ID_Region);
                    Area Area = DataBase.GetAreaById(Region.ID_Area);

                    AreaSenderComboBox.SelectedItem = Area;
                    RegionSenderComboBox.SelectedItem = Region;
                    foreach (City c in CitySenderComboBox.ItemsSource)
                    {
                        if (c.ID_City == City.ID_City)
                        {
                            CitySenderComboBox.SelectedItem = c;
                            break;
                        }
                    }
                }
                else AreaSenderComboBox.SelectedIndex = 0;
            }
            else
            {
                AreaSenderComboBox.SelectedIndex = 0;
            }
        }

        private void IndexRecieverComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IndexRecieverComboBox.SelectedValue != null)
            {
                if (IndexRecieverComboBox.SelectedIndex != 0)
                {
                    City City = DataBase.GetCityById(((Post_Office)IndexRecieverComboBox.SelectedItem).ID_City);
                    Region Region = DataBase.GetRegionById(City.ID_Region);
                    Area Area = DataBase.GetAreaById(Region.ID_Area);
                    AreaRecieverComboBox.SelectedItem = Area;
                    RegionRecieverComboBox.SelectedItem = Region;
                    foreach (City c in CityRecieverComboBox.ItemsSource)
                        if (c.ID_City == City.ID_City)
                        {
                            CityRecieverComboBox.SelectedItem = c;
                            break;
                        }
                }
                else AreaRecieverComboBox.SelectedIndex = 0;
            }
            else
            {
                AreaRecieverComboBox.SelectedIndex = 0;
            }
        }

    }
}
