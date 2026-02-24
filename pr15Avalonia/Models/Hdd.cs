using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Hdd
{
    public int Id { get; set; }

    public int Rotationspeed { get; set; }

    public virtual Storagedevice IdNavigation { get; set; } = null!;
}
