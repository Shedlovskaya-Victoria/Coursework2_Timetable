using System;
using System.Collections.Generic;

namespace Coursework2_Timetable.DTO;

public partial class ProtectivePlane
{
    public int Id { get; set; }

    public int Idproject { get; set; }

    public string Protect { get; set; } = null!;

    public virtual Project IdprojectNavigation { get; set; } = null!;
}
