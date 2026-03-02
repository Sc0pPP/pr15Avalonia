using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using pr15Avalonia.Models;
using System.Linq;
using System.Threading.Tasks;
using WpfLikeAvaloniaNavigation;

namespace pr15Avalonia.Pages;

public partial class AssemblyDetailsPage : Page
{
    private PcDbContext _context = new();
    private int _assemblyId;

    public AssemblyDetailsPage(int assemblyId)
    {
        InitializeComponent();
        _assemblyId = assemblyId;
        _ = LoadAssemblyAsync();
    }

    private async Task LoadAssemblyAsync()
    {
        var assembly = await _context.Assemblies
            .FirstOrDefaultAsync(a => a.Id == _assemblyId);

        if (assembly == null) return;

        TitleText.Text = $"Сборка: {assembly.Name}";

        var partIds = await _context.Partassemblies
            .Where(pa => pa.Assemblyid == _assemblyId)
            .Select(pa => pa.Partid)
            .ToListAsync();

        var parts = await _context.Baseparts
            .Where(bp => partIds.Contains(bp.Id))
            .Include(bp => bp.Manufacturer)
            .ToListAsync();

        PartsListBox.ItemsSource = parts;

        decimal total = parts.Sum(p => p.Price);
        TotalText.Text = $"Итого: {total:N0} ₽";
    }

    private void Back_Click(object? sender, RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }
}