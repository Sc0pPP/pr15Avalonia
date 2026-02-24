using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Ssd
{
    public int Id { get; set; }

    public int Tbw { get; set; }

    public virtual Storagedevice IdNavigation { get; set; } = null!;
}
