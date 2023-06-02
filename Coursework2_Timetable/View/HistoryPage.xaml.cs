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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Coursework2_Timetable.DTO;
namespace Coursework2_Timetable.View
{
    /// <summary>
    /// Логика взаимодействия для HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string searchText = "";

        private string selectedSorting;
        private DTO.Type selectedType;
        public Project SelectedProject { get; set; }
        public Project p { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
            }
        }
        public List<string> Sorting { get; set; } = new() { "без сортировки", "от старых", "от новых" };
        public string SelectedSorting
        {
            get => selectedSorting;
            set
            {
                selectedSorting = value;
                Search();
            }
        }
      
        public List<DTO.Type> Types { get; set; } = new();
        public DTO.Type SelectedType { 
            get => selectedType;
            set
            {
                selectedType = value;
                Search();
            }
        }
        public HistoryPage()
        {
            InitializeComponent();
            Projects = GetProgects();
            Types = DB.GetInstance().Types.ToList();
            p = Projects.LastOrDefault();
            Types.Add(new DTO. Type { Type1 = "все типы" });
            Search();
            DataContext = this;
        }

        void Search()
        {
            var searchProj = DB.GetInstance().Projects.
                Where(p=>p.Title.Contains(SearchText) ||
                p.Theme.Contains(SearchText) ||
                p.Target.Contains(SearchText) ||
                p.ClientName.Contains(SearchText) ||
                p.ClientMiddleName.Contains(SearchText));
           

            if (SelectedType != null)
            {
                if(SelectedType.Type1 != "все типы")
                {
                    searchProj = searchProj.Where(p => p.IdtypeNavigation.Type1.Contains(SelectedType.Type1));
                }
            }

            if (SelectedSorting == "от старых")
                searchProj = searchProj.OrderBy(s => s.StartDate);
            else if (SelectedSorting == "от новых")
                searchProj = searchProj.OrderByDescending(s => s.StartDate);

           
            Projects = searchProj.ToList();
            Signal(nameof(Projects));
        }
        List<Project> GetProgects()
        {
            List<Project> project = DB.GetInstance().Projects.Include(p => p.IdtypeNavigation).
                Include(p => p.PlanedActivities).
                Include(p => p.PlanedResults).
                Include(p => p.ProtectivePlanes).
                Include(p => p.RisksProjects).
               Include(p => p.StagesProjects).ThenInclude(s => s.IdstatuseNavigation).
                Include(p => p.StagesProjects).ThenInclude(s => s.IdresponsibleParticipantNavigation).
                Include(p => p.SupportingMeasures).ToList();
            return project;
        }
        void Signal([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        private void ClickGoMainPage(object sender, RoutedEventArgs e)
        {
            Navigation.GetInstance().page = new MainPage();
        }

        private void ClickAddNewProject(object sender, RoutedEventArgs e)
        {
            Navigation.GetInstance().page = new EditProjectPage(p, false);
        }

        private void ClickAllInf(object sender, RoutedEventArgs e)
        {
            if (SelectedProject == null)
            { MessageBox.Show("выберите проект!"); return; }

            AllInfWindow all = new AllInfWindow(SelectedProject);
            all.ShowDialog();
            all.Close();
        }

        private void ClikDeleteProject(object sender, RoutedEventArgs e)
        {
            if (SelectedProject == null)
            { MessageBox.Show("выберите проект!"); return; }

            if (MessageBox.Show("Удалить проект?", "Проверка",
                       MessageBoxButton.YesNo)==MessageBoxResult.Yes)
            { 
                if(SelectedProject.PlanedResults!=null)
                {
                    var a = SelectedProject.PlanedResults.ToList();
                    foreach (var pl in a)
                    {
                        DB.GetInstance().PlanedResults.Remove(pl);
                        DB.GetInstance().SaveChanges();
                    }
                }
                if(SelectedProject.RisksProjects!=null)
                {
                    var b = SelectedProject.RisksProjects.ToList();
                    foreach (var pl in b)
                    {
                        DB.GetInstance().RisksProjects.Remove(pl);
                        DB.GetInstance().SaveChanges();
                    }
                }
               if(SelectedProject.PlanedActivities!=null)
                {
                    var c = SelectedProject.PlanedActivities.ToList();
                    foreach (var pl in c)
                    {
                        DB.GetInstance().PlanedActivities.Remove(pl);
                        DB.GetInstance().SaveChanges();
                    }
                }
                if (SelectedProject.ProtectivePlanes!=null)
                {
                    var d = SelectedProject.ProtectivePlanes.ToList();
                    foreach (var pp in d)
                    {
                        DB.GetInstance().ProtectivePlanes.Remove(pp);
                        DB.GetInstance().SaveChanges();
                    }
                }
                if(SelectedProject.Participantprojects!=null)
                {
                    var q = SelectedProject.Participantprojects.ToList();
                    foreach (var pp in q)
                    {
                        DB.GetInstance().Participantprojects.Remove(pp);
                        DB.GetInstance().SaveChanges();
                    }
                }
                if(SelectedProject.StagesProjects!=null)
                {
                    var j = SelectedProject.StagesProjects.ToList();
                    foreach (var st in j)
                    {
                        DB.GetInstance().StagesProjects.Remove(st);
                        DB.GetInstance().SaveChanges();
                    }
                }
                if(SelectedProject.SupportingMeasures!=null)
                {
                    var h = SelectedProject.SupportingMeasures.ToList();
                    foreach (var sm in h)
                    {
                        DB.GetInstance().SupportingMeasures.Remove(sm);
                        DB.GetInstance().SaveChanges();
                    }
                }
                
                DB.GetInstance().Projects.Remove(SelectedProject);
                DB.GetInstance().SaveChanges();

                Projects = GetProgects();
                Signal(nameof(Projects));
            }
        }
    }
}
