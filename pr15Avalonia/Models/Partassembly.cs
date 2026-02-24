using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Partassembly
{
    public int Id { get; set; }

    public int Partid { get; set; }

    public int Assemblyid { get; set; }

    public virtual Assembly Assembly { get; set; } = null!;

    public virtual Basepart Part { get; set; } = null!;
}
