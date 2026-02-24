using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Socket
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Cpu> Cpus { get; set; } = new List<Cpu>();

    public virtual ICollection<Motherboard> Motherboards { get; set; } = new List<Motherboard>();

    public virtual ICollection<Socketprocessorcooler> Socketprocessorcoolers { get; set; } = new List<Socketprocessorcooler>();
}
