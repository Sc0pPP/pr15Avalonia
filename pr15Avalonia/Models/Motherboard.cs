using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Motherboard
{
    public int Id { get; set; }

    public int Socketid { get; set; }

    public int Formfactorid { get; set; }

    public int Memoryslots { get; set; }

    public int Memorytypeid { get; set; }

    public int Pcislots { get; set; }

    public int Sataports { get; set; }

    public int Usbports { get; set; }

    public virtual Formfactor Formfactor { get; set; } = null!;

    public virtual Basepart IdNavigation { get; set; } = null!;

    public virtual Memorytype Memorytype { get; set; } = null!;

    public virtual Socket Socket { get; set; } = null!;
}
