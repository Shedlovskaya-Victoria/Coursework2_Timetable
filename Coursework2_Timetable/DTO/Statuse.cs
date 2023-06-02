using System;
using System.Collections.Generic;

namespace Coursework2_Timetable.DTO;

public partial class Statuse
{
    public int Id { get; set; }

    public string Statuse1 { get; set; } = null!;

    public virtual ICollection<StagesProject> StagesProjects { get; } = new List<StagesProject>();
}
