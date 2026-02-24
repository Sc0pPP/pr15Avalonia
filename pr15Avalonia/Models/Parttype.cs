using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Parttype
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Basepart> Baseparts { get; set; } = new List<Basepart>();
}
