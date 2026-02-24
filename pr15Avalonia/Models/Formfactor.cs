using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Formfactor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Boardformfactorcase> Boardformfactorcases { get; set; } = new List<Boardformfactorcase>();

    public virtual ICollection<Motherboard> Motherboards { get; set; } = new List<Motherboard>();
}
