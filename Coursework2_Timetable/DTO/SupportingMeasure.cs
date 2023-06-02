using System;
using System.Collections.Generic;

namespace Coursework2_Timetable.DTO;

public partial class SupportingMeasure
{
    public int Id { get; set; }

    public int Idproject { get; set; }

    public string Support { get; set; } = null!;

    public int? Idstajes { get; set; }

    public virtual Project IdprojectNavigation { get; set; } = null!;

    public virtual StagesProject? IdstajesNavigation { get; set; }
}
