using System;
using System.Collections.Generic;

namespace Coursework2_Timetable.DTO;

public partial class Participantproject
{
    public int Idparticipant { get; set; }

    public int Idproject { get; set; }

    public int Id { get; set; }

    public virtual Participant IdparticipantNavigation { get; set; } = null!;

    public virtual Project IdprojectNavigation { get; set; } = null!;
}
