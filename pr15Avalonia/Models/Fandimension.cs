using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Fandimension
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Powersupply> Powersupplies { get; set; } = new List<Powersupply>();

    public virtual ICollection<Processorcooler> Processorcoolers { get; set; } = new List<Processorcooler>();
}
