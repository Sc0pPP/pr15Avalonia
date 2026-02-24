using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Ram
{
    public int Id { get; set; }

    public int Memorytypeid { get; set; }

    public int Capacity { get; set; }

    public int Count { get; set; }

    public int Ghz { get; set; }

    public string Timings { get; set; } = null!;

    public virtual Basepart IdNavigation { get; set; } = null!;

    public virtual Memorytype Memorytype { get; set; } = null!;
}
