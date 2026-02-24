using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Powersupply
{
    public int Id { get; set; }

    public int Power { get; set; }

    public int Fandimensionid { get; set; }

    public int Certificationid { get; set; }

    public virtual Certificate Certification { get; set; } = null!;

    public virtual Fandimension Fandimension { get; set; } = null!;

    public virtual Basepart IdNavigation { get; set; } = null!;
}
