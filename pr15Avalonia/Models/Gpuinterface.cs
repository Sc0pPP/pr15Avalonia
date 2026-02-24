using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Gpuinterface
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Gpu> Gpus { get; set; } = new List<Gpu>();
}
