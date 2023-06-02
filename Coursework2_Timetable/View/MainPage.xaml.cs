using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        private string searchText = "";
        private string selectedSorting;
        private Statuse selectedFiltration;
        private StagesProject selectedStagesProjects;

        public event PropertyChangedEventHandler? PropertyChanged;
        public List<Project> project { get; set; } = new();
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
            }
        }
        public List<StagesProject> stagesProjects { get; set; } = new();
        public StagesProject SelectedStage { get; set; } = new();
        public Project p { get; set; } = new();
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
        public List<Statuse> Filtration { get; set; }
        public Statuse SelectedFiltration
        {
            get => selectedFiltration;
            set
            {
                selectedFiltration = value;
                Search();
            }
        }
        public string StrType { get; set; }
        public StagesProject SelectedStagesProjects
        {
            get => selectedStagesProjects;
            set
            {
                selectedStagesProjects = value;
                Signal();
            }
        }

        public List<StagesProject> StgPr { get; set; } = new();
        public MainPage()
        {
            InitializeComponent();
            project = DB.GetInstance().Projects.Include(p=>p.IdtypeNavigation).
                Include(p => p.PlanedActivities).
                Include(p => p.PlanedResults).
                Include(p => p.ProtectivePlanes).
                Include(p => p.RisksProjects).
                Include(p => p.StagesProjects).ThenInclude(s => s.IdstatuseNavigation).
                Include(p => p.StagesProjects).ThenInclude(s => s.IdresponsibleParticipantNavigation).
                Include(p => p.SupportingMeasures).ToList();
            p = project.LastOrDefault();
            GetFiltration();
            Search();
            GetStages();
            DataContext = this;

            StrType = p.IdtypeNavigation.Type1;

        }

        private void GetStages()
        {
            stagesProjects = DB.GetInstance().StagesProjects.
                 Include(s => s.IdstatuseNavigation).
                 Include(s=>s.IdresponsibleParticipantNavigation).ToList();
            stagesProjects = stagesProjects.Where(s => s.Idproject == p.Id).ToList();
        }

        private void GetFiltration()
        {
            Filtration = new();
            Filtration = DB.GetInstance().Statuses.ToList();
            Filtration.Add(new Statuse { Statuse1 = "все статусы" });
            SelectedFiltration = Filtration.LastOrDefault();
        }
        void Search()
        {
            var searchProj = DB.GetInstance().StagesProjects.
                Include(t => t.IdresponsibleParticipantNavigation).
                Where(s => s.Title.Contains(SearchText) ||
            s.TaskStage.Contains(SearchText) ||
            s.IdresponsibleParticipantNavigation.LastName.Contains(SearchText) ||
            s.IdresponsibleParticipantNavigation.Name.Contains(SearchText) ||
            s.IdresponsibleParticipantNavigation.MiddleName.Contains(SearchText)
            ).Where(s=>s.Idproject==p.Id);

           if (SelectedSorting == "от старых")
              searchProj = searchProj.OrderBy(s => s.StartDate);
            else if (SelectedSorting == "от новых")
               searchProj = searchProj.OrderByDescending(s => s.StartDate);

            if (SelectedFiltration != null && SelectedFiltration.Id != 0)
                searchProj = searchProj.Where(s => s.Idstatuse == SelectedFiltration.Id);

            stagesProjects = searchProj.ToList();
            Signal(nameof(stagesProjects));
        }
        void Signal([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void ClickAddNewProject(object sender, RoutedEventArgs e)
        {
            // DB dB = new();
            //  p = dB.GetNewProgect();
            Navigation.GetInstance().page = new EditProjectPage(p, false);
        }

        private void ClickEditProject(object sender, RoutedEventArgs e)
        {
            Navigation.GetInstance().page = new EditProjectPage(p, true);
        }

        private void ClickHistoryProject(object sender, RoutedEventArgs e)
        {
            Navigation.GetInstance().page = new HistoryPage();
        }

        private void ClickStatuseInProgress(object sender, RoutedEventArgs e)
        {
            ChangeStatuse(1);
        }

        private void ClickStatuseAwaiting(object sender, RoutedEventArgs e)
        {
            ChangeStatuse(0);
        }

        private void ClickStatuseDone(object sender, RoutedEventArgs e)
        {
            ChangeStatuse(2);
        }
        void ChangeStatuse(int a)
        {
            var orig = DB.GetInstance().StagesProjects.Include(s => s.IdstatuseNavigation).ToList();
            orig.Find(s=>s.IdstatuseNavigation.Id == SelectedStage.Idstatuse).Idstatuse = Filtration[a].Id;
            orig.Find(s => s.IdstatuseNavigation.Id == SelectedStage.Idstatuse).IdstatuseNavigation = Filtration[a];
            DB.GetInstance().SaveChanges();
            Search();
        }

        private void ButtonShovDiagram(object sender, RoutedEventArgs e)
        {
            DiagramDateStageWindow1 diagram = new DiagramDateStageWindow1(stagesProjects);
            diagram.ShowDialog();
            diagram.Close();
        }

        private void ButtonClientData(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"ФИО: {p.ClientName} {p.ClientLastName} {p.ClientMiddleName} " +
             Environment.NewLine + Environment.NewLine + 
             $"Номер карты: {p.ClientNumberCard}" + 
             Environment.NewLine + Environment.NewLine + 
             $"Телефон: {p.ClientPhone}" +
             Environment.NewLine + Environment.NewLine +
             $"Почта: {p.ClientEmail}", "Данные клиента");
        }
    }
}
