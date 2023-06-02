using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coursework2_Timetable.DTO;
namespace Coursework2_Timetable
{
    public class DB
    {
        static DbCoursework2TimetableContext context { get; set; }
        public static DbCoursework2TimetableContext GetInstance()
        {
            if (context == null)
                return context = new DbCoursework2TimetableContext();
            return context;
        }
        string str;
        public string Get(string str)
        {
            this.str = str;
            return str;
        }
        public string Set()
        {
            return str;
        }
        //public Project GetNewProgect()
        //{
        //    PlanedActivity planAct = new(); PlanedResult planRes = new();
        //    ProtectivePlane protect = new(); QualityRequrement quality = new();
        //    Statuse statuse = new(); SupportingMeasure supporting = new();
        //    RisksProject risks = new(); Task task = new(); Participant participant = new();
        //    Participantproject participantproject = new(); StagesProject stages = new();
        //    Typeproject typeproject = new(); Type type = new();

        //DbCoursework2TimetableContext dbCoursework2Timetable = new();
        //Project p = dbCoursework2Timetable.Projects.Add(new Project());

        //Project p = new()
        //{
        //    PlanedActivities = new() { planAct },
        //    PlanedResults = new() { planRes }
        //};

    }
}
        //    planAct.IdprojectNavigation = p; planAct.Activities = "actEx";
        //    planRes.IdprojectNavigation = p; planRes.Result = "ResEx";
        //    protect.IdprojectNavigation = p; protect.Protect = "ProtEx";
        //    quality.IdprogectNavigation = p; quality.Quality = "QalEx";
        //    risks.IdprojectNavigation = p; risks.Risk = "RiskEx";
        //    participantproject.IdparticipantNavigation = participant;
        //    participantproject.IdprojectNavigation = p; participant.LastName = "LEx";
        //    task.IdprogectNavigation = p; task.IdstatuseNavigation = statuse; statuse.Statuse1 = "Dx";
        //    task.IdresponsibleParticipantNavigation = participant; task.Task1 = "TasEx";
        //    stages.IdtaskNavigation = task; stages.IdprojectNavigation = p; stages.Content = "StagEx";
        //    supporting.IdprojectNavigation = p; supporting.IdstajesNavigation = stages; supporting.Support = "SupEx";
        //    typeproject.IdprojectNavigation = p; typeproject.IdtypeNavigation = type; type.Type1 = "TypEx";

        //    return p;
        
    

