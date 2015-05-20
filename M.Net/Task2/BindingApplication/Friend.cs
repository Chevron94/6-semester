using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Threading;
using System.ComponentModel;
namespace BindingApplication
{
    public class Friend : INotifyPropertyChanged
    {
        string nickName;
        int age;
        string country;
        string city;
        string lastMessage;

        public Friend(string _NickName, int _Age, string _Country, string _City, string _LastMessage)
        {
            NickName = _NickName;
            Age = _Age;
            Country = _Country;
            City = _City;
            LastMessage = _LastMessage;
        }

        public Friend()
        {
        }

        public string NickName
        {
            get
            {
                return nickName;
            }
            set
            {
                if (nickName != value)
                {
                    nickName = value.Trim();
                    OnPropertyChanged("NickName");
                }
            }
        }

        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                if (age != value)
                {
                    age = value;
                    OnPropertyChanged("Age");
                }
            }
        }

        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                if (country != value)
                {
                    country = value.Trim();
                    OnPropertyChanged("Country");
                }
            }
        }

        public string City
        {
            get
            {
                return city;
            }
            set
            {
                if (city != value)
                {
                    city = value.Trim();
                    OnPropertyChanged("City");
                }
            }
        }

        public string LastMessage
        {
            get
            {
                return lastMessage;
            }
            set
            {
                if (lastMessage != value)
                {
                    lastMessage = value.Trim();
                    OnPropertyChanged("LastMessage");
                }
            }
        }
        public override string ToString()
        {
            return "["+NickName+"] ["+Age.ToString()+"] ["+Country+"] ["+City+"] ["+LastMessage+"]";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
