using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Memorytype
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Motherboard> Motherboards { get; set; } = new List<Motherboard>();

    public virtual ICollection<Ram> Rams { get; set; } = new List<Ram>();
}
