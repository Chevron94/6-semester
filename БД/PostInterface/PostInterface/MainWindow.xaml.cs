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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PostInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool connected = false;     
        Worker Worker;
        int LastSearchId = 0;        

        public MainWindow()
        {
            InitializeComponent();
            DataView.Visibility = System.Windows.Visibility.Hidden;
            Dispatcher.Hooks.DispatcherInactive+=MyIdle;
        }

        public void MyIdle(object sender, EventArgs e)
        {
            ConnectMenuItem.IsEnabled = !connected;
            CloseMenuItem.IsEnabled = connected;
            EditMenuItem.IsEnabled = (Worker != null) && connected;
            SearchMenuItem.IsEnabled = connected;
            AuthMenuItem.IsEnabled = (Worker == null) && connected;
            LogoutMenuItem.IsEnabled = EditMenuItem.IsEnabled;
            AdministrationMenuItem.IsEnabled = EditMenuItem.IsEnabled && Worker != null && Worker.ID_Office == 1;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AuthMenuItem_Click(object sender, RoutedEventArgs e)
        {
            auth auth = new auth();
            Message message = new Message();
            auth.ShowDialog();
            Worker = auth.worker;
        }

        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow AEF = new AddEditWindow(null);
            if (AEF.ShowDialog() == true)
            {
                AEF.Letter.ID_Office_Worker = Worker.ID_Worker;
                DataBase.AddConsignment(AEF.Letter);
                Message msg = new Message();
                msg.Show("Отправление успешно добавлено.\nНомер отправления: " + AEF.Letter.ID_Consignment);
                DataBase.AddTrackElem(new Track_List { Date = DateTime.Now, ID_Consignment = AEF.Letter.ID_Consignment, Post_Index = DataBase.GetClientById(AEF.Letter.ID_Sender).Index, ID_Full_Letter_Status = 1 });
            };
        }

        private void EditInformationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputId input;
            if (LastSearchId == 0)
                input = new InputId("Введите номер отправления");
            else input = new InputId("Введите номер отправления",LastSearchId.ToString());
            Message message = new Message();
            if (input.ShowDialog() == false)
            {
            }
            //MessageBox.Show("Не введен ID");
            else
            {
                Consignment LC = new Consignment();
                LC = DataBase.GetConsigmentById(Int32.Parse(input.InputIDTextBox.Text));
                if (LC != null)
                {
                    AddEditWindow aef = new AddEditWindow(LC);
                    if (aef.ShowDialog() == true)
                    {
                        LC = aef.Letter;

                        DataBase.EditConsigment(LC);
                    }
                }
                else message.Show("Отправление не найдено!");
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputId input;
            if (LastSearchId == 0)
                input = new InputId("Введите номер отправления");
            else input = new InputId("Введите номер отправления", LastSearchId.ToString());
            Message message = new Message();
            if (input.ShowDialog() == false)
            { } //message.Show("Не введен ID");//MessageBox.Show("Не введен ID");
            else
            {
                int id = Int32.Parse(input.InputIDTextBox.Text);
                if (DataBase.GetConsigmentById(id)==null)
                {
                    message.Show("Не наден ID");
                    //MessageBox.Show("Не наден ID");
                }
                else
                {

                    if ((new ConfirmWindow("Удалить выбранное отправление?")).ShowDialog() == true)
                    {
                        DataBase.DeleteConsigment(id);
                        DataBase.DeleteConsignmentFromTrackList(id);
                        if (id == LastSearchId)
                        {
                            DataView.ItemsSource = null;
                        }
                        //MessageBox.Show("Удалено");
                        message.Show("Удалено");
                    }
                }
            }
        }

        private void SearchLetterMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputId input = new InputId("Введите номер отправления");
            Message message = new Message();
            if (input.ShowDialog()==true)
            {
                int id = Int32.Parse(input.InputIDTextBox.Text);
                var res = DataBase.GetTrackListByConsigmentId(id);
                LastSearchId = id;
                if (res.Count == 0)
                {
                    message.Show("Информация не найдена"); //MessageBox.Show("Информация не найдена");
                }
                else DataView.ItemsSource = res;
            }
        }

        private void LogoutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Worker = null;
            DataView.ItemsSource = null;
            LastSearchId = 0;
           // AuthMenuItem.IsEnabled = true;
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Message message = new Message();
            message.Show("Клиент может отправить посылку, либо письмо в любой населенный пункт, при этом воспользоваться различными службами доставки. Цена заказа на пересылку зависит от пункта отправления, пункта назначения, веса, типа отправления и службы доставки. Клиент может отслеживать статус и местоположение своего отправления по уникальному идентификатору заказа.");
        }

        private void EditStatusMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputId input;
            if (LastSearchId == 0)
                input = new InputId("Введите номер отправления");
            else input = new InputId("Введите номер отправления", LastSearchId.ToString());
            Message message = new Message();
            if (input.ShowDialog() == true)
            {
                int id = Int32.Parse(input.InputIDTextBox.Text);
                //LetterClass LC = new LetterClass();
                if (DataBase.GetConsigmentById(id)==null)
                {
                    message.Show("Информация не найдена");
                }
                else
                {
                    EditStatusForm ESF = new EditStatusForm(id);
                    if (ESF.ShowDialog() == true)
                    {
                        DataBase.AddTrackElem(ESF.TEC);
                        if (LastSearchId == id)
                        {
                            DataView.ItemsSource = DataBase.GetTrackListByConsigmentId(LastSearchId);
                        }
                    }
                }
            }
        }

        private void ConnectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DataBase.Init();
            DataView.Visibility = System.Windows.Visibility.Visible;
            Title = "Почтовые отправления [Подключено]";
            connected = true;
            LastSearchId = 0;
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            connected = false;
            Title = "Почтовые отправления [Отключено]";
            DataView.Visibility = System.Windows.Visibility.Hidden;
            Worker = null;
            DataView.ItemsSource = null;
            LastSearchId = 0;
        }

        private void ServiseSearchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputId input;
            if (LastSearchId == 0)
                input = new InputId("Введите номер отправления");
            else input = new InputId("Введите номер отправления", LastSearchId.ToString());
             Message message = new Message();
             if (input.ShowDialog() == true)
             {
                 Consignment LC = new Consignment();
                 LC = DataBase.GetConsigmentById(Int32.Parse(input.InputIDTextBox.Text));
                 if (LC != null)
                 {
                     AddEditWindow aef = new AddEditWindow(LC,true);
                     if (aef.ShowDialog() == true)
                     {
                         LC = aef.Letter;
                         DataBase.EditConsigment(LC);
                     }
                 }
             }
        }

        private void AddAreaMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddEditAddress AEA = new AddEditAddress(Address_States.Add_Area);
            if (AEA.ShowDialog() == true)
            {
                DataBase.AddArea(AEA.A);
            }
        }

        private void EditAreaMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddEditAddress AEA = new AddEditAddress(Address_States.Edit_Area);
            if (AEA.ShowDialog() == true)
            {
                DataBase.EditArea(AEA.A);
            }
        }

        private void EditRegionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddEditAddress AEA = new AddEditAddress(Address_States.Edit_Region);
            if (AEA.ShowDialog() == true)
            {
                DataBase.EditRegion(AEA.R);
            }
        }

        private void AddRegionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddEditAddress AEA = new AddEditAddress(Address_States.Add_Region);
            if (AEA.ShowDialog() == true)
            {
                DataBase.AddRegion(AEA.R);
            }
        }

        private void AddCityMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddEditAddress AEA = new AddEditAddress(Address_States.Add_City);
            if (AEA.ShowDialog() == true)
            {
                DataBase.AddCity(AEA.C);
            }
        }

        private void EditCityMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddEditAddress AEA = new AddEditAddress(Address_States.Edit_City);
            if (AEA.ShowDialog() == true)
            {
                DataBase.EditCity(AEA.C);
            }
        }

        private void AddStreetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddEditAddress AEA = new AddEditAddress(Address_States.Add_Street);
            if (AEA.ShowDialog() == true)
            {
                DataBase.AddStreet(AEA.S);
            }
        }

        private void EditStreetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddEditAddress AEA = new AddEditAddress(Address_States.Edit_Street);
            if (AEA.ShowDialog() == true)
            {
                
                DataBase.EditStreet(AEA.S);
            }
        }

        private void AddWorkerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Worker WC = new Worker();
            AddEditWorker AEW = new AddEditWorker(Worker_States.Add_Worker, ref WC);
            if (AEW.ShowDialog() == true)
            {
                DataBase.AddWorker(WC);
                Message msg = new Message();
                msg.Show("Сотрудник добавлен,\n ID="+WC.ID_Worker);
            }
        }

        private void EditWorkerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Worker WC = null;
            InputId IID = new InputId("Введите ID сотрудника");
            Message msg = new Message();
            if (IID.ShowDialog() == true)
            {
                WC = DataBase.GetWorkerByID(Int32.Parse(IID.InputIDTextBox.Text));
                if (WC != null)
                {
                    AddEditWorker AEW = new AddEditWorker(Worker_States.Edit_Worker, ref WC);
                    if (AEW.ShowDialog() == true)
                    {
                        DataBase.EditWorker(WC);
                    }
                }
                else msg.Show("Сотрудник не найден");
            }
        }

        private void DeleteWorkerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputId IID = new InputId("Введите ID сотрудника");
            Message msg = new Message();
            if (IID.ShowDialog() == true)
            {
                int id = Int32.Parse(IID.InputIDTextBox.Text);
                if (DataBase.GetWorkerByID(id)!=null)
                {
                    if ((new ConfirmWindow("Удалить выбранного сотрудника?")).ShowDialog() == true)
                    {
                        DataBase.DeleteWorker(id);
                        msg.Show("Сотрудник удален");
                    }
                        
                }
                else msg.Show("Сотрудник не найден");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void AddCompanyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Transport_Company CC = new Transport_Company();
            AddEditCompanies AEC = new AddEditCompanies(false,ref CC);
            if (AEC.ShowDialog() == true)
                DataBase.AddCompany(CC);
        }

        private void EditCompanyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Transport_Company CC = new Transport_Company();
            AddEditCompanies AEW = new AddEditCompanies(true, ref CC);
                    if (AEW.ShowDialog() == true)
                    {
                        DataBase.EditCompany(CC);
                    }
            
        }

        private void DeleteCompany_Click(object sender, RoutedEventArgs e)
        {
            InputId IID = new InputId("Введите ID компании");
            Message msg = new Message();
            if (IID.ShowDialog() == true)
            {
                int id = Int32.Parse(IID.InputIDTextBox.Text);

                if (DataBase.GetCompanyById(id) != null)
                {
                    if ((new ConfirmWindow("Удалить выбранную компанию?")).ShowDialog() == true)
                    {
                            DataBase.DeleteCompany(id);
                    }

                }
                else msg.Show("Компания не найдена");
            }
        }

        private void EditCostRegionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditCostRegions ECR = new EditCostRegions();
            if (ECR.ShowDialog()==true)
            {
                DataBase.EditTransportCost(new Transport_Cost() { ID_City_From = ((City)ECR.FromCityComboBox.SelectedItem).ID_City, ID_City_To = ((City)ECR.ToCityComboBox.SelectedItem).ID_City, ID_Transport_Company = ((Transport_Company)ECR.CompanyComboBox.SelectedItem).ID_Transport_Company, Cost=Int32.Parse(ECR.CostTextBox.Text) });
            }
            
        }

        private void AddPostOfficeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddEditPostOffice AEPO = new AddEditPostOffice(null);
            if (AEPO.ShowDialog() == true)
            {
                DataBase.AddPostOffice(AEPO.PO);
            }
        }

        private void EditPostOfficeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputId IID = new InputId("Введите почтовый индекс отделения");
            Message msg = new Message();
            if (IID.ShowDialog() == true)
            {
                long id = Int32.Parse(IID.InputIDTextBox.Text);
                Post_Office po = DataBase.GetPostOfficeByPostIndex(id);
                if (po == null)
                    msg.Show("Не найдено");
                else
                {
                    AddEditPostOffice AEPO = new AddEditPostOffice(po);
                    if (AEPO.ShowDialog() == true)
                    {
                        DataBase.DeletePostOffice(id);
                        DataBase.AddPostOffice(AEPO.PO);
                    }
                }
            }
        }

        private void DeletePostOfficeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputId IID = new InputId("Введите почтовый индекс отделения");
            Message msg = new Message();
            if (IID.ShowDialog() == true)
            {
                DataBase.DeletePostOffice(Int32.Parse(IID.InputIDTextBox.Text));
            }
        }
    }
}
