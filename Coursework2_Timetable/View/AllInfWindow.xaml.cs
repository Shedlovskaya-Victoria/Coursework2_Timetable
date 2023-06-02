using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Coursework2_Timetable.DTO;

namespace Coursework2_Timetable.View
{
    /// <summary>
    /// Логика взаимодействия для AllInfWindow.xaml
    /// </summary>
    public partial class AllInfWindow : Window, INotifyPropertyChanged
    {
       public double LastBudget { get; set; } = 0;
        public Project Project { get; set; } = new();
        public AllInfWindow(Project show)
        {
            InitializeComponent();
            Project = show;
            Signal(nameof(Project));
            CountLastBudget();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        double CountLastBudget()
        {
            if (Project.Budget == null)
                return LastBudget = 0;
            LastBudget = (double)Project.Budget;

            foreach (var b in Project.StagesProjects)
            {
                if (b.ExpensesMoney != null)
                {
                    var m = (double)b.ExpensesMoney;
                    LastBudget = LastBudget - m;
                }
            }
            Signal(nameof(LastBudget));
            return LastBudget;
        }
        void Signal([CallerMemberName] string prop = null) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        private void ButtonClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}