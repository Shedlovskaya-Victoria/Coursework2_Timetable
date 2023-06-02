using System;
using System.Collections.Generic;

namespace Coursework2_Timetable.DTO;

public partial class PlanedResult
{
    public int Id { get; set; }

    public string Result { get; set; } = null!;

    public int Idproject { get; set; }

    public virtual Project IdprojectNavigation { get; set; } = null!;
}
