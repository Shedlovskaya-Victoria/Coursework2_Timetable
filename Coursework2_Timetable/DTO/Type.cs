using System;
using System.Collections.Generic;

namespace Coursework2_Timetable.DTO;

public partial class Type
{
    public int Id { get; set; }

    public string Type1 { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; } = new List<Project>();
}
