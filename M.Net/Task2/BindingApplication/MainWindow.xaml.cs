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
using System.Collections.ObjectModel;

namespace BindingApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Friend> f = new ObservableCollection<Friend>();
        public MainWindow()
        {
            InitializeComponent();
            List<String> Names = new List<string>()
            {
                "Adam",
                "Ivan",
                "Sasha",
                "Anna",
                "Igor",
                "Sergey",
                "Olya",
                "Tom",
                "Tanya",
                "Maxim"
            };
            List<String> Countries = new List<string>()
            {
                "Russia",
                "Belarus",
                "England",
                "Poland",
                "Slovenia",
                "Czech Republic",
                "Slovakia",
                "Serbia",
                "China",
                "Japan"
            };
            List<String> Cities = new List<string>()
            {
                "Moskow",
                "Minsk",
                "London",
                "Warszawa",
                "Lublyana",
                "Prague",
                "Bratislava",
                "Belgrad",
                "Beijing",
                "Tokyo"
            };
            List<String> Messages = new List<string>()
            {
                "hello",
                "Hi",
                "Привет",
                "Cześć",
                "How are you?",
                "What do you do?",
                "Jak się masz?",
                "Как поживаешь?",
                "Как дела?",
                "Чем занимаешься?"
            };
            Random rnd = new Random();
            for (int i = 0; i < 30; i++)
            {
                int nameIndex = rnd.Next(10);
                int placeIndex = rnd.Next(10);
                int age = rnd.Next(100);
                int message_index = rnd.Next(10);
                f.Add(new Friend(Names[nameIndex], age, Countries[placeIndex], Cities[placeIndex], Messages[message_index]));
            }
            this.DataContext = f;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AddingWindow AW = new AddingWindow();
            if (AW.ShowDialog() == true)
                f.Add(AW.Friend);
            
        }
    }
}
