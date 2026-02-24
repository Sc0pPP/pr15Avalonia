using System;
using System.Collections.Generic;

namespace pr15Avalonia.Models;

public partial class Boardformfactorcase
{
    public int Id { get; set; }

    public int Caseid { get; set; }

    public int Formfactorid { get; set; }

    public virtual Case Case { get; set; } = null!;

    public virtual Formfactor Formfactor { get; set; } = null!;
}
