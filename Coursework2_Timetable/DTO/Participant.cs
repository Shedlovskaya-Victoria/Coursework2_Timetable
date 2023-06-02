using System;
using System.Collections.Generic;

namespace Coursework2_Timetable.DTO;

public partial class Participant
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public virtual ICollection<Participantproject> Participantprojects { get; } = new List<Participantproject>();

    public virtual ICollection<StagesProject> StagesProjects { get; } = new List<StagesProject>();
}
