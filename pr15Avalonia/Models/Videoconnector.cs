using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Videoconnector
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Videoconnectorgpu> Videoconnectorgpus { get; set; } = new List<Videoconnectorgpu>();
}
