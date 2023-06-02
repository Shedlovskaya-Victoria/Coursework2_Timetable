using System;
using System.Collections.Generic;

namespace Coursework2_Timetable.DTO;

public partial class Task
{
    public string Task1 { get; set; } = null!;

    public int Id { get; set; }

    public int Idprogect { get; set; }

    public int Idstatuse { get; set; }

    public int? IdresponsibleParticipant { get; set; }

    public virtual Project IdprogectNavigation { get; set; } = null!;

    public virtual Participant? IdresponsibleParticipantNavigation { get; set; }

    public virtual Statuse IdstatuseNavigation { get; set; } = null!;

}
