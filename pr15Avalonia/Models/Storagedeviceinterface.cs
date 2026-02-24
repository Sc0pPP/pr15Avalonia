using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Storagedeviceinterface
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Storagedevice> Storagedevices { get; set; } = new List<Storagedevice>();
}
