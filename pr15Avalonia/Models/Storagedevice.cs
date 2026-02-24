using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Storagedevice
{
    public int Id { get; set; }

    public int Capacity { get; set; }

    public int Storagedeviceinterfaceid { get; set; }

    public int Storagedevicetypeid { get; set; }

    public virtual Hdd? Hdd { get; set; }

    public virtual Basepart IdNavigation { get; set; } = null!;

    public virtual Ssd? Ssd { get; set; }

    public virtual Storagedeviceinterface Storagedeviceinterface { get; set; } = null!;

    public virtual Storagedevicetype Storagedevicetype { get; set; } = null!;
}
