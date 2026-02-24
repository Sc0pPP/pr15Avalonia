using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Casesize
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
}
