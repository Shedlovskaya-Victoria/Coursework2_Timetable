using System;
using System.Collections.Generic;

namespace Coursework2_Timetable;

public partial class Typeproject
{
    public int Idtype { get; set; }

    public int Idproject { get; set; }

    public int Id { get; set; }

    public virtual DTO.Project IdprojectNavigation { get; set; } = null!;

    public virtual Type IdtypeNavigation { get; set; } = null!;
}
