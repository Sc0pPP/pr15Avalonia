using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Cpu
{
    public int Id { get; set; }

    public int Socketid { get; set; }

    public int Numberofcores { get; set; }

    public double Basecorefrequency { get; set; }

    public double Maxcorefrequency { get; set; }

    public int Cachel3 { get; set; }

    public int? Igpuid { get; set; }

    public int Thermalpower { get; set; }

    public bool Hasigpu { get; set; }

    public virtual Basepart IdNavigation { get; set; } = null!;

    public virtual Igpu? Igpu { get; set; }

    public virtual Socket Socket { get; set; } = null!;
}
