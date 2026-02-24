using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Processorcooler
{
    public int Id { get; set; }

    public int Fandimensionid { get; set; }

    public int Heatpipes { get; set; }

    public int Minspeed { get; set; }

    public int Maxspeed { get; set; }

    public double Noiselevel { get; set; }

    public virtual Fandimension Fandimension { get; set; } = null!;

    public virtual Basepart IdNavigation { get; set; } = null!;

    public virtual ICollection<Socketprocessorcooler> Socketprocessorcoolers { get; set; } = new List<Socketprocessorcooler>();
}
