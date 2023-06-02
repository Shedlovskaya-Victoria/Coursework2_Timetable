using System;
using System.Collections.Generic;

namespace Coursework2_Timetable.DTO;

public partial class Project
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Target { get; set; } = null!;

    public string Theme { get; set; } = null!;

    public string? StrategyProtect { get; set; }

    public string ClientName { get; set; } = null!;

    public string ClientLastName { get; set; } = null!;

    public string? ClientMiddleName { get; set; }

    public string ClientNumberCard { get; set; } = null!;

    public string? Tools { get; set; }

    public string? Comment { get; set; }

    public string? UserName { get; set; }

    public string? UserLastName { get; set; }

    public string? UserMiddleName { get; set; }

    public decimal? Budget { get; set; }

    public string? QualityRequments { get; set; }

    public int Idtype { get; set; }

    public string? ClientPhone { get; set; }

    public string? ClientEmail { get; set; }

    public virtual Type IdtypeNavigation { get; set; } = null!;

    public virtual ICollection<Participantproject> Participantprojects { get; } = new List<Participantproject>();

    public virtual ICollection<PlanedActivity> PlanedActivities { get; } = new List<PlanedActivity>();

    public virtual ICollection<PlanedResult> PlanedResults { get; } = new List<PlanedResult>();

    public virtual ICollection<ProtectivePlane> ProtectivePlanes { get; } = new List<ProtectivePlane>();

    public virtual ICollection<RisksProject> RisksProjects { get; } = new List<RisksProject>();

    public virtual ICollection<StagesProject> StagesProjects { get; } = new List<StagesProject>();

    public virtual ICollection<SupportingMeasure> SupportingMeasures { get; } = new List<SupportingMeasure>();
}
