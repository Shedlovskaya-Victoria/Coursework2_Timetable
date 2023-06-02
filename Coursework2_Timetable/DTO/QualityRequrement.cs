using System;
using System.Collections.Generic;

namespace Coursework2_Timetable;

public partial class QualityRequrement
{
    public int Id { get; set; }

    public int Idprogect { get; set; }

    public string Quality { get; set; } = null!;

    public virtual DTO.Project IdprogectNavigation { get; set; } = null!;
}
