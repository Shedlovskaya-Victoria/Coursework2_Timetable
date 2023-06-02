using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Coursework2_Timetable
{
    public class Navigation : INotifyPropertyChanged
    {
         Page currentpage1;

        public event PropertyChangedEventHandler? PropertyChanged;

        static Navigation navigation { get; set; }
        public Page page { 
            get => currentpage1;
            set
            {
                currentpage1 = value;
                Signal();
            }
        }

        private void Signal([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public static Navigation GetInstance()
        {
            if (navigation == null)
                return navigation = new Navigation();
            return navigation;
        }
    }
}
