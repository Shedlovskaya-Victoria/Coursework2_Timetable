using System;
using System.Collections.Generic;

namespace Coursework2_Timetable.DTO;

public partial class StagesProject
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Content { get; set; } = null!;

    public decimal? ExpensesMoney { get; set; }

    public string? ResoursesExpendable { get; set; }

    public string? ResoursesRenewable { get; set; }

    public string? ResoursesFinance { get; set; }

    public int Idproject { get; set; }

    public string? Comment { get; set; }

    public int? IdresponsibleParticipant { get; set; }

    public int? Idstatuse { get; set; }

    public string? TaskStage { get; set; }

    public virtual Project IdprojectNavigation { get; set; } = null!;

    public virtual Participant? IdresponsibleParticipantNavigation { get; set; }

    public virtual Statuse? IdstatuseNavigation { get; set; }

    public virtual ICollection<SupportingMeasure> SupportingMeasures { get; } = new List<SupportingMeasure>();
}
