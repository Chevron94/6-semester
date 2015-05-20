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
using System.ComponentModel;

namespace BindingApplication
{
    /// <summary>
    /// Interaction logic for AddingWindow.xaml
    /// </summary>
    public partial class AddingWindow : Window, INotifyPropertyChanged
    {
        public Friend friend = new Friend("",0,"","","");
        bool empty = true;
        public AddingWindow()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        public Friend Friend
        {
            get
            {
                return friend;
            }
            set
            {
                friend = value;
            }
        }

        public bool Empty
        {
            get
            {
                return empty;
            }
            set
            {
                if (empty != value)
                {
                    empty = value;
                    OnPropertyChanged("Empty");
                }

            }
        }

        private void textBox5_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = (!Char.IsDigit(e.Text, 0));
        }

        private void textBox5_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = (e.Key == Key.Space);
            textBox1_PreviewTextInput(null, null);

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            //f = new Friend(textBox1.Text.Trim(), Int32.Parse(textBox2.Text.Trim()), textBox3.Text.Trim(), textBox4.Text.Trim(), textBox5.Text.Trim());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Empty = textBox1.Text.Trim().Length == 0 ||
                                textBox2.Text.Trim().Length == 0 ||
                                textBox3.Text.Trim().Length == 0 ||
                                textBox4.Text.Trim().Length == 0 ||
                                textBox5.Text.Trim().Length == 0;
        }
    }
}
