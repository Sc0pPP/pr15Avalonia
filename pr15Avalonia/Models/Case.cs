using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Case
{
    public int Id { get; set; }

    public int Sizeid { get; set; }

    public int Expansionslots { get; set; }

    public int Fans { get; set; }

    public virtual ICollection<Boardformfactorcase> Boardformfactorcases { get; set; } = new List<Boardformfactorcase>();

    public virtual Basepart IdNavigation { get; set; } = null!;

    public virtual Casesize Size { get; set; } = null!;
}
