using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Basepart
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Manufacturerid { get; set; }

    public int Parttypeid { get; set; }

    public string Image { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Case? Case { get; set; }

    public virtual Cpu? Cpu { get; set; }

    public virtual Gpu? Gpu { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual Motherboard? Motherboard { get; set; }

    public virtual ICollection<Partassembly> Partassemblies { get; set; } = new List<Partassembly>();

    public virtual Parttype Parttype { get; set; } = null!;

    public virtual Powersupply? Powersupply { get; set; }

    public virtual Processorcooler? Processorcooler { get; set; }

    public virtual Ram? Ram { get; set; }

    public virtual Storagedevice? Storagedevice { get; set; }
}
