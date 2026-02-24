using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Igpu
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Cpu> Cpus { get; set; } = new List<Cpu>();
}
