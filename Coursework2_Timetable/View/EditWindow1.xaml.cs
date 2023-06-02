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
    /// Логика взаимодействия для EditWindow1.xaml
    /// </summary>
    public partial class EditWindow1 : Window, INotifyPropertyChanged
    {
        string TitleStageFirst;
        string TaskFirst;
        public string Task { get; set; }
        public string TitleStage { get; set; }
        public List<StagesProject> Stages { get; set; } = new();
        public StagesProject SelectedStg { get; set; } = new();
        public List<Participant> Participants { get; set; } = new();
        Participant FirstVersPartic;
        public Participant Participant { get; set; }
        public string Text { get; set; }
        string FirstVers;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Visibility VsblPartic { get; set; } = Visibility.Collapsed;
        public  Visibility VsblOne { get; set; } = Visibility.Collapsed;
        public Visibility VsblStage { get; set; } = Visibility.Collapsed;
        public Visibility VsblCmbStages { get; set; } = Visibility.Collapsed;
        public EditWindow1(string edit, Participant participant, string edit2, List<StagesProject> st)
        { 
            InitializeComponent();
            DataContext = this;
            if (!string.IsNullOrEmpty(edit) && participant == null  && string.IsNullOrEmpty(edit2) && st == null)
            {
                FirstVers = edit;
                Text = edit;
                VsblOne = Visibility.Visible;
                Signal(nameof(VsblOne));
            }
           else if(string.IsNullOrEmpty(edit) && participant != null  && string.IsNullOrEmpty(edit2) && st == null)
            {
                VsblPartic = Visibility.Visible;
                FirstVersPartic = participant;
                Participant = participant;
                Participants = DB.GetInstance().Participants.ToList();
                Signal(nameof(VsblPartic));

            }
            else if(!string.IsNullOrEmpty(edit) && participant == null && !string.IsNullOrEmpty(edit) && st == null)
            {

                VsblStage = Visibility.Visible;
                TitleStageFirst = edit;
                TitleStage = edit;
                Task = edit2;
                TaskFirst = edit2;
                Signal(nameof(VsblStage));
            }
            else if(!string.IsNullOrEmpty(edit) && participant == null && string.IsNullOrEmpty(edit2) && st != null)
            {
                VsblCmbStages = Visibility.Visible;
                Text = edit;
                FirstVers = edit;
                Stages = st.ToList();
                Signal(nameof(VsblCmbStages));
            }
            
          
        }
        void Signal([CallerMemberName] string prop = null) =>
              PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        private void ClickCloseWindow(object sender, RoutedEventArgs e)
        {
            Text = FirstVers;
            Participant = FirstVersPartic;
            TitleStage = TitleStageFirst;
            this.Task = TaskFirst;
            SelectedStg = null;
            Close();
        }

        private void ClickSaveWindow(object sender, RoutedEventArgs e)
        {
            if (SelectedStg == null)
            {
                MessageBox.Show("Не была выбрана стадия!"); return;
            }
            Close();
        }
    }
}
