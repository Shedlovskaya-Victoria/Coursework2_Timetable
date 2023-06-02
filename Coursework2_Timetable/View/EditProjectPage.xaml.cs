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
using System.Drawing.Imaging;

namespace Coursework2_Timetable.View
{
    /// <summary>
    /// Логика взаимодействия для EditProjectPage.xaml
    /// </summary>
    public partial class EditProjectPage : Page, INotifyPropertyChanged
    {
        private DTO.Type selectedType;
        private StagesProject selectedStagesProjects;

        public Project NewProject { get; set; } = new();

        public List<Participant> Participants { get; set; } = new();
        List<Participantproject> Participantprojects { get; set; } = new();
        public Participant SelectedParticipant { get; set; }

        public List<PlanedActivity> PlanedActivities { get; set; } = new();
        public PlanedActivity SelectedPlanedActivitie { get; set; }

        public List<PlanedResult> PlanedResults { get; set; } = new();
        public PlanedResult SelectedPlanedResults { get; set; }

        public List<RisksProject> RisksProjects { get; set; } = new();
        public RisksProject SelectedRisksProject { get; set; }

        public List<ProtectivePlane> ProtectivePlanes { get; set; } = new();
        public ProtectivePlane SelectedProtectivePlane { get; set; }

       

        public ObservableCollection<SupportingMeasure> SupportingMeasures { get; set; }
        public SupportingMeasure SelectedSupportingMeasure { get; set; }

       

        public List<DTO.Type> Types { get; set; } = new();
        public DTO.Type SelectedType
        {
            get => selectedType;
            set
            {
                selectedType = value;
                NewProject.Idtype = value.Id;
                NewProject.IdtypeNavigation = value;
                StrType = value.Type1; Signal(nameof(StrType));
            }
        }

        public List<StagesProject> StagesProjects { get; set; } = new();
        public StagesProject SelectedStagesProjects
        {
            get => selectedStagesProjects;
            set { 
                selectedStagesProjects = value;
                Signal();
            }
        }
        public Double LastBudget { get; set; }
        public string StrType { get; set; }

        public PlanedActivity SelectedCmbBoxAct { get; set; }
        public PlanedResult SelectedCmbBoxRes { get; set; }
        bool old;
        public EditProjectPage(Project edit, bool old)
        {
            InitializeComponent();
            if (old)
            {
                this.old = old;
                NewProject = edit;
                PlanedActivities = DB.GetInstance().PlanedActivities.
                    Where(p => p.Idproject == NewProject.Id).ToList();
                PlanedResults = DB.GetInstance().PlanedResults.
                    Where(p => p.Idproject == NewProject.Id).ToList();
                StagesProjects = DB.GetInstance().StagesProjects.
                    Where(p => p.Idproject == NewProject.Id).ToList();
                RisksProjects = DB.GetInstance().RisksProjects.
                    Where(p => p.Idproject == NewProject.Id).ToList();
                ProtectivePlanes = DB.GetInstance().ProtectivePlanes.
                    Where(p => p.Idproject == NewProject.Id).ToList();
                Participants = DB.GetInstance().Participants.Include(s => s.Participantprojects).ToList();
                Participants = Participants.Where(s => s.Participantprojects.Where(s => s.Idproject == NewProject.Id) != null).ToList();
                var s = DB.GetInstance().SupportingMeasures.
                     Where(p => p.Idproject == NewProject.Id);
                SupportingMeasures = new ObservableCollection<SupportingMeasure>(s) ;
                CountLastBudget();
                SelectedType = NewProject.IdtypeNavigation;
                StrType = NewProject.IdtypeNavigation.Type1; Signal(nameof(StrType));
            }
            else
            {
                NewProject.StartDate = DateTime.Now;
                if (edit == null)
                    return;
                NewProject.UserName = edit.UserName;
                NewProject.UserLastName = edit.UserLastName;
                NewProject.UserMiddleName = edit.UserMiddleName;
            }
            Types = DB.GetInstance().Types.ToList();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void Signal([CallerMemberName] string prop = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        double CountLastBudget()
        {
            if (NewProject.Budget == null)
                return LastBudget = 0;
            LastBudget = (double)NewProject.Budget;

            foreach (var b in StagesProjects)
            {
                if (b.ExpensesMoney != null)
                {
                    var m = (double)b.ExpensesMoney;
                    LastBudget = LastBudget - m;
                }
            }
            Signal();
            Signal(nameof(LastBudget));
            return LastBudget;
        }
        private void ClickGoMainPage(object sender, RoutedEventArgs e)
        {
            Navigation.GetInstance().page = new MainPage();
        }

        private void ClickHistoryProject(object sender, RoutedEventArgs e)
        {
            Navigation.GetInstance().page = new HistoryPage();
        }

        private void ClickAddPLanAct(object sender, RoutedEventArgs e)
        {
            EditWindow1 editWindow1 = new("new", null, null, null);
            editWindow1.ShowDialog();
            if (editWindow1.Text == "new")
                return;
            PlanedActivities.Add(new PlanedActivity
            {
                Activities = editWindow1.Text,
                Idproject = NewProject.Id,
                IdprojectNavigation = NewProject
            });
            PlanedActivities = new List<PlanedActivity>(PlanedActivities);
            Signal(nameof(PlanedActivities));


        }

        private void ClickDeletePLanAct(object sender, RoutedEventArgs e)
        {
            if (SelectedPlanedActivitie == null)
                return;
            PlanedActivities.Remove(SelectedPlanedActivitie);
            PlanedActivities = new List<PlanedActivity>(PlanedActivities);
            Signal(nameof(PlanedActivities));
        }

        private void ClickRemovePartic(object sender, RoutedEventArgs e)
        {
            if (SelectedParticipant == null)
                return;
            Participants.Remove(SelectedParticipant);
            Participants = new List<Participant>(Participants);
            Signal(nameof(Participants));
            if (SelectedParticipant == null)
                return;
            var a = (Participantproject)Participantprojects.LastOrDefault(a => a.Idparticipant == SelectedParticipant.Id);
            Participantprojects.Remove(a);
            Participantprojects = new List<Participantproject>(Participantprojects);
            Signal(nameof(Participantprojects));
        }

        private void ClickAddPartic(object sender, RoutedEventArgs e)
        {
            EditWindow1 window1 = new(null, new Participant(),  null, null);
            window1.ShowDialog();
            if (window1.Participant != new Participant())
            {
                Participants.Add(new Participant
                {
                    Name = window1.Participant.Name,
                    LastName = window1.Participant.LastName,
                    MiddleName = window1.Participant.MiddleName,

                });
                Participantprojects.Add(new Participantproject
                {
                    Idparticipant = Participants.FirstOrDefault(s => s.LastName == window1.Participant.LastName).Id,
                    Idproject = NewProject.Id,
                    IdprojectNavigation = NewProject,
                    IdparticipantNavigation = Participants.FirstOrDefault(s => s.LastName == window1.Participant.LastName)
                });
                Participants = new List<Participant>(Participants);
                Signal(nameof(Participants));
            }

        }

        private void ClickAddStages(object sender, RoutedEventArgs e)
        {
            EditWindow1 window1 = new("new", null, "new", null);
            window1.ShowDialog();
            if (window1.Task == "new" && window1.TitleStage == "new")
                return;

            StagesProjects.Add(new StagesProject
            {
                Title = window1.TitleStage,
                Idproject = NewProject.Id,
                IdprojectNavigation = NewProject,
                TaskStage = window1.Task,
                Idstatuse = 1,
                IdstatuseNavigation = DB.GetInstance().Statuses.FirstOrDefault(s => s.Id == 1)
            });
            
            StagesProjects = new List<StagesProject>(StagesProjects);
            Signal(nameof(StagesProjects));
        }

        private void ClickDeleteStages(object sender, RoutedEventArgs e)
        {
            if (SelectedStagesProjects == null)
                return;
            StagesProjects.Remove(SelectedStagesProjects);
            StagesProjects = new List<StagesProject>(StagesProjects);
            Signal(nameof(StagesProjects));
        }

        private void ClickAddPLanRes(object sender, RoutedEventArgs e)
        {
            EditWindow1 window1 = new("new", null, null, null);
            window1.ShowDialog();
            if (window1.Text != "new")
            {
                PlanedResults.Add(new PlanedResult
                {
                    Result = window1.Text,
                    Idproject = NewProject.Id,
                    IdprojectNavigation = NewProject
                });
                PlanedResults = new List<PlanedResult>(PlanedResults);
                Signal(nameof(PlanedResults));
            }

        }

        private void ClickDeletePlanRes(object sender, RoutedEventArgs e)
        {
            if (SelectedPlanedResults == null)
                return;
            PlanedResults.Remove(SelectedPlanedResults);
            PlanedResults = new List<PlanedResult>(PlanedResults);
            Signal(nameof(PlanedResults));
        }

        private void ClickAddRisk(object sender, RoutedEventArgs e)
        {
            EditWindow1 window1 = new EditWindow1("new", null, null, null);
            window1.ShowDialog();
            if (window1.Text != "new")
            {
                RisksProjects.Add(new RisksProject
                {
                    Idproject = NewProject.Id,
                    IdprojectNavigation = NewProject,
                    Risk = window1.Text
                });
                RisksProjects = new List<RisksProject>(RisksProjects);
                Signal(nameof(RisksProjects));
            }
          
        }

        private void ClickDeleteRisk(object sender, RoutedEventArgs e)
        {
            if (SelectedRisksProject == null)
                return;
            RisksProjects.Remove(SelectedRisksProject);
            RisksProjects = new List<RisksProject>(RisksProjects);
            Signal(nameof(RisksProjects));
        }

        private void ClickEditRisk(object sender, RoutedEventArgs e)
        {
            if (SelectedRisksProject == null)
                return;
            EditWindow1 window1 = new EditWindow1(SelectedRisksProject.Risk, null, null, null);
            window1.ShowDialog();
            if (window1.Text == SelectedRisksProject.Risk)
                return;
            SelectedRisksProject.Risk = window1.Text;
            RisksProjects = new List<RisksProject>(RisksProjects);
            Signal(nameof(RisksProjects));
        }

        private void ClickEditProtect(object sender, RoutedEventArgs e)
        {
            if (SelectedProtectivePlane == null)
                return;
            EditWindow1 window1 = new EditWindow1(SelectedProtectivePlane.Protect, null, null, null);
            window1.ShowDialog();
            if (window1.Text == SelectedProtectivePlane.Protect)
                return;
            SelectedProtectivePlane.Protect = window1.Text;
            ProtectivePlanes = new List<ProtectivePlane>(ProtectivePlanes);
            Signal(nameof(ProtectivePlanes));
        }

        private void ClickDeleteProtect(object sender, RoutedEventArgs e)
        {
            if (SelectedProtectivePlane == null)
                return;
            ProtectivePlanes.Remove(SelectedProtectivePlane);
            ProtectivePlanes = new List<ProtectivePlane>(ProtectivePlanes);
            Signal(nameof(ProtectivePlanes));
        }

        private void ClickAddProtect(object sender, RoutedEventArgs e)
        {
            EditWindow1 window1 = new EditWindow1("new", null, null, null);
            window1.ShowDialog();
            if (window1.Text == "new")
                return;
            ProtectivePlanes.Add(new ProtectivePlane
            {
                Idproject = NewProject.Id,
                IdprojectNavigation = NewProject,
                Protect = window1.Text
            });
            ProtectivePlanes = new List<ProtectivePlane>(ProtectivePlanes);
            Signal(nameof(ProtectivePlanes));
        }

        private void ClickDeleteTaskPartic(object sender, RoutedEventArgs e)
        {
            if (selectedStagesProjects == null)
                return;
            selectedStagesProjects.IdresponsibleParticipant = null;
            selectedStagesProjects.IdresponsibleParticipantNavigation = null;
            
            Participants.Remove(selectedStagesProjects.IdresponsibleParticipantNavigation);
            
            var p = (Participantproject)Participantprojects.FirstOrDefault(s => s.Idparticipant == selectedStagesProjects.IdresponsibleParticipantNavigation.Id);
            Participantprojects.Remove(p);
            
            StagesProjects = new List<StagesProject>(StagesProjects);
            Signal(nameof(StagesProjects));
        }

        private void ClickEditTaskPartic(object sender, RoutedEventArgs e)
        {
            if(SelectedStagesProjects==null)
            {
                MessageBox.Show("Выберите этап!"); return;
            }
            Participant a = (Participant)SelectedStagesProjects.IdresponsibleParticipantNavigation;
            if (a == null)
                a = new Participant();
           EditWindow1 window1 = new EditWindow1(null, a, null, null);
            window1.ShowDialog();
            if (window1.Participant == a)
                return;
            SelectedStagesProjects.IdresponsibleParticipant = window1.Participant.Id;
            SelectedStagesProjects.IdresponsibleParticipantNavigation = window1.Participant;
            Participants.Add(new Participant
            {
                Name = window1.Participant.Name,
                LastName = window1.Participant.LastName,
                MiddleName = window1.Participant.MiddleName
            });
            Participantprojects.Add(new Participantproject
            {
                Idparticipant = Participants.FirstOrDefault(s => s.LastName == window1.Participant.LastName).Id,
                Idproject = NewProject.Id,
                IdprojectNavigation = NewProject,
                IdparticipantNavigation = Participants.FirstOrDefault(s => s.LastName == window1.Participant.LastName)
            });
            StagesProjects = new List<StagesProject>(StagesProjects);
            Signal(nameof(StagesProjects));
        }

        private void ButtonAddNewPlan(object sender, RoutedEventArgs e)
        {
            if(old) //перезапись изменившихся строк
            {
                var original = DB.GetInstance().Projects.Find(NewProject.Id);
                DB.GetInstance().Projects.Entry(original).CurrentValues.SetValues(NewProject);

                foreach (var pl in PlanedActivities)
                {
                    if (!DB.GetInstance().PlanedActivities.Contains(pl))
                    {
                        var originalA = DB.GetInstance().PlanedActivities.Find(pl.Id);
                        DB.GetInstance().PlanedActivities.Entry(originalA).CurrentValues.SetValues(pl);
                    }
                }
                foreach (var pl in PlanedResults)
                {
                    if (!DB.GetInstance().PlanedResults.Contains(pl))
                    {
                        var originalR = DB.GetInstance().PlanedResults.Find(pl.Id);
                        DB.GetInstance().PlanedResults.Entry(originalR).CurrentValues.SetValues(pl);
                    }
                }
                foreach (var pl in RisksProjects)
                {
                    if (!DB.GetInstance().RisksProjects.Contains(pl))
                    {
                        var originalR = DB.GetInstance().RisksProjects.Find(pl.Id);
                        DB.GetInstance().RisksProjects.Entry(originalR).CurrentValues.SetValues(pl);
                    }
                }

                foreach (var pp in ProtectivePlanes)
                {
                    if (!DB.GetInstance().ProtectivePlanes.Contains(pp))
                    { 
                        var originalR = DB.GetInstance().ProtectivePlanes.Find(pp.Id);
                        DB.GetInstance().ProtectivePlanes.Entry(originalR).CurrentValues.SetValues(pp);
                    }
                }
                foreach (var st in StagesProjects)
                {
                    if (!DB.GetInstance().StagesProjects.Contains(st))
                    {
                        if (st.StartDate == null)
                        {
                            MessageBox.Show($"Не заполнено содержание этапа {st.Title}!"); return;
                        }
                        if (st.Content == null)
                        {
                            MessageBox.Show($"Не заполнено содержание этапа {st.Title}!") ; return;
                        }
                        var originalR = DB.GetInstance().StagesProjects.Find(st.Id);
                        DB.GetInstance().StagesProjects.Entry(originalR).CurrentValues.SetValues(st);
                    }
                }

                foreach (var sm in SupportingMeasures)
                {
                    if (!DB.GetInstance().SupportingMeasures.Contains(sm))
                    {
                        var originalR = DB.GetInstance().SupportingMeasures.Find(sm.Id);
                        DB.GetInstance().SupportingMeasures.Entry(originalR).CurrentValues.SetValues(sm);
                    }
                }
                foreach (var p in Participants)
                {
                    if (!DB.GetInstance().Participants.Contains(p))
                    {
                        var originalR = DB.GetInstance().Participants.Find(p.Id);
                        DB.GetInstance().Participants.Entry(originalR).CurrentValues.SetValues(p);
                    }
                }
                foreach (var pp in Participantprojects)
                {
                    if (!DB.GetInstance().Participantprojects.Contains(pp))
                    { 
                        var originalR = DB.GetInstance().Participantprojects.Find(pp.Id);
                        DB.GetInstance().Participantprojects.Entry(originalR).CurrentValues.SetValues(pp);
                    }
                }
                DB.GetInstance().SaveChanges();
            }
            else //cоздание нового проекта
            {
                if (Types != null)
                {
                    foreach (var t in Types)
                    {
                        if (!DB.GetInstance().Types.Contains(t))
                        {
                            DB.GetInstance().Types.Add(t);
                            DB.GetInstance().SaveChanges();
                        }
                    }
                }
                if (NewProject.Idtype == null)
                {
                    MessageBox.Show("Выберите тип проекта!"); return;
                }
                if(NewProject.IdtypeNavigation == null)
                {
                    MessageBox.Show("Выберите тип проекта!"); return;
                }
                if (NewProject.ClientNumberCard == null)
                {
                    MessageBox.Show("Введите номер карты!"); return;
                }
                if (NewProject.ClientName == null || NewProject.ClientLastName == null)
                {
                    MessageBox.Show("Заполните имя или фамилию клиента!"); return;
                }
                if (NewProject.Target == null || NewProject.Theme == null)
                {
                    MessageBox.Show("Заполните цель или тему проекта!"); return;
                }
                if (NewProject.Title == null)
                {
                    MessageBox.Show("Заполните название проекта!"); return;
                }
                
                DB.GetInstance().Projects.Add(NewProject);
                DB.GetInstance().SaveChanges();
                if (PlanedActivities != null)
                {
                    foreach (var pl in PlanedActivities)
                    {
                        DB.GetInstance().PlanedActivities.Add(pl);
                        DB.GetInstance().SaveChanges();
                    }
                }
                if (PlanedResults != null)
                {

                    foreach (var pl in PlanedResults)
                    {
                        DB.GetInstance().PlanedResults.Add(pl);
                        DB.GetInstance().SaveChanges();
                    }
                }
                if (RisksProjects != null)
                {
                    foreach (var pl in RisksProjects)
                    {
                        DB.GetInstance().RisksProjects.Add(pl);
                        DB.GetInstance().SaveChanges();
                    }
                }
                if (ProtectivePlanes != null)
                {
                    foreach (var pp in ProtectivePlanes)
                    {
                        DB.GetInstance().ProtectivePlanes.Add(pp);
                        DB.GetInstance().SaveChanges();
                    }
                }
                foreach (var p in Participants)
                {
                    if (!DB.GetInstance().Participants.Contains(p))
                    {
                        DB.GetInstance().Participants.Add(p);
                        DB.GetInstance().SaveChanges();
                    }
                }
                if (Participantprojects != null)
                {
                    foreach (var pp in Participantprojects)
                    {
                        DB.GetInstance().Participantprojects.Add(pp);
                        DB.GetInstance().SaveChanges();
                    }
                }
                if (StagesProjects != null)
                {
                    foreach (var st in StagesProjects)
                    {
                        if (st.StartDate == null)
                        {
                            MessageBox.Show($"Не заполнено содержание этапа {st.Title}!"); return;
                        }
                        if (st.Content == null)
                        {
                            MessageBox.Show($"Не заполнено содержание этапа {st.Title}!"); return;
                        }
                        DB.GetInstance().StagesProjects.Add(st);
                        DB.GetInstance().SaveChanges();
                    }
                }
                if (SupportingMeasures != null)
                {
                    foreach (var sm in SupportingMeasures)
                    {
                        DB.GetInstance().SupportingMeasures.Add(sm);
                        DB.GetInstance().SaveChanges();
                    }
                }
            }
           
            Navigation.GetInstance().page = new MainPage();
        }

        private void ClickEditStages(object sender, RoutedEventArgs e)
        {
            if (SelectedStagesProjects == null)
                return;
            EditWindow1 window1 = new(SelectedStagesProjects.Title, null, SelectedStagesProjects.TaskStage, null);
            window1.ShowDialog();
            if (window1.TitleStage == SelectedStagesProjects.Title && window1.Task == SelectedStagesProjects.TaskStage)
                return;

           SelectedStagesProjects.Title = window1.TitleStage;
           SelectedStagesProjects.TaskStage = window1.Task;
            StagesProjects = new List<StagesProject>(StagesProjects);
            Signal(nameof(StagesProjects));
        }

        private void ClickEditPartic(object sender, RoutedEventArgs e)
        {
            if (SelectedParticipant == null)
                return;
            EditWindow1 window1 = new(null, SelectedParticipant, null, null);
            window1.ShowDialog();
            if (window1.Participant == SelectedParticipant)
                return;
            SelectedParticipant.Name = window1.Participant.Name;
            SelectedParticipant.LastName = window1.Participant.LastName;
            SelectedParticipant.MiddleName = window1.Participant.MiddleName;
            Participants = new List<Participant>(Participants);
            Signal(nameof(Participants));
        }

        private void ClickEditPlanRes(object sender, RoutedEventArgs e)
        {
            if (SelectedPlanedResults == null)
                return;
            EditWindow1 window1 = new(SelectedPlanedResults.Result, null, null, null);
            window1.ShowDialog();
            if (window1.Text == SelectedPlanedResults.Result)
                return;
            SelectedPlanedResults.Result = window1.Text;
            PlanedResults = new List<PlanedResult>(PlanedResults);
            Signal(nameof(PlanedResults));
        }

        private void ClickEditPlanAct(object sender, RoutedEventArgs e)
        {
            if (SelectedPlanedActivitie == null)
                return;
            EditWindow1 editWindow1 = new(SelectedPlanedActivitie.Activities, null, null, null);
            editWindow1.ShowDialog();
            if (editWindow1.Text == SelectedPlanedActivitie.Activities)
                return;
            SelectedPlanedActivitie.Activities = editWindow1.Text;
            PlanedActivities = new List<PlanedActivity>(PlanedActivities);
            Signal(nameof(PlanedActivities));
        }

        private void ClickSaveAllStage(object sender, RoutedEventArgs e)
        {
            if(SelectedStagesProjects == null)
            {
                MessageBox.Show("Выберите этап, перед его сохранением.", "И заполните, если что, сначала."); return;
            }

            CountLastBudget();

            if (SelectedStagesProjects.StartDate == null)
            { MessageBox.Show("напишите начало этапа"); return; }

            if (SelectedStagesProjects.Content == null)
            { MessageBox.Show("напишите содержимое этапа"); return; }

            Signal(nameof(StagesProjects));
            MessageBox.Show("Это успех!");
        }

        private void ClickAddSupp(object sender, RoutedEventArgs e)
        {
            if (StagesProjects == null)
            {
                MessageBox.Show("Напишите стадии!"); return;
            }
                
            EditWindow1 window1 = new EditWindow1("new", null, null, StagesProjects);
            window1.ShowDialog();
            if (window1.Text == "new")
                return;
            if (window1.SelectedStg == null)
                return;
                SupportingMeasures.Add(new SupportingMeasure
                {
                    Idproject = NewProject.Id,
                    IdprojectNavigation = NewProject,
                    Idstajes = window1.SelectedStg.Id,
                    IdstajesNavigation = window1.SelectedStg,
                    Support = window1.Text
                });
            SupportingMeasures = new ObservableCollection<SupportingMeasure>(SupportingMeasures);
            Signal(nameof(SupportingMeasures));
        }

        private void ClickDeleteSupp(object sender, RoutedEventArgs e)
        {
            if (SelectedSupportingMeasure == null)
                return;
            SupportingMeasures.Remove(SelectedSupportingMeasure);
        }

        private void ClickEditSupp(object sender, RoutedEventArgs e)
        {
            if (SelectedSupportingMeasure == null)
            {
                MessageBox.Show("Выберите запись!"); return;
            }
            if (StagesProjects == null)
            {
                MessageBox.Show("Напишите стадии!"); return;
            }
            EditWindow1 window1 = new EditWindow1(SelectedSupportingMeasure.Support, null, null, StagesProjects);
            window1.ShowDialog();
            if (window1.Text == SelectedSupportingMeasure.Support)
                return;
            SelectedSupportingMeasure.Support = window1.Text;
            SelectedSupportingMeasure.Idstajes = window1.SelectedStg.Id;
            SelectedSupportingMeasure.IdstajesNavigation = window1.SelectedStg;
            SupportingMeasures = new ObservableCollection<SupportingMeasure>(SupportingMeasures);
            Signal(nameof(SupportingMeasures));
        }

        private void ClickAddNewTypes(object sender, RoutedEventArgs e)       
        {
            EditWindow1 window1 = new EditWindow1("new", null, null, null);
            window1.ShowDialog();
            if (window1.Text == "new")
                return;
            Types.Add(new DTO.Type
            {
                Type1 = window1.Text
            });
            NewProject.Idtype = Types.FirstOrDefault(s=>s.Type1==window1.Text).Id;
            NewProject.IdtypeNavigation.Type1 = window1.Text;
            StrType = window1.Text; Signal(nameof(StrType));
           
            Types = new List<DTO.Type>(Types);
            Signal(nameof(Types));
        }

    }
}
