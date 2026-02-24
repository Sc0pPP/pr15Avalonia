using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Socketprocessorcooler
{
    public int Id { get; set; }

    public int Socketid { get; set; }

    public int Processorcoolerid { get; set; }

    public virtual Processorcooler Processorcooler { get; set; } = null!;

    public virtual Socket Socket { get; set; } = null!;
}
