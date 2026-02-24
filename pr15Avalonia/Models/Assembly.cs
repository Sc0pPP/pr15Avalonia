using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Assembly
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Author { get; set; } = null!;

    public virtual ICollection<Partassembly> Partassemblies { get; set; } = new List<Partassembly>();
}
