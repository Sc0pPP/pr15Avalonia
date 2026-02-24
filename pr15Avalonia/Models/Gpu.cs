using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Gpu
{
    public int Id { get; set; }

    public int Gpuinterfaceid { get; set; }

    public int Chipfrequency { get; set; }

    public int Videomemory { get; set; }

    public int Memorybus { get; set; }

    public int? Recommendpower { get; set; }

    public virtual Gpuinterface Gpuinterface { get; set; } = null!;

    public virtual Basepart IdNavigation { get; set; } = null!;

    public virtual ICollection<Videoconnectorgpu> Videoconnectorgpus { get; set; } = new List<Videoconnectorgpu>();
}
