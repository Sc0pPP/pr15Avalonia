using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Videoconnectorgpu
{
    public int Id { get; set; }

    public int Gpuid { get; set; }

    public int Videoconnectorid { get; set; }

    public virtual Gpu Gpu { get; set; } = null!;

    public virtual Videoconnector Videoconnector { get; set; } = null!;
}
