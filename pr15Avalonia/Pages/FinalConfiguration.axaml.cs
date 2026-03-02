using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using pr15Avalonia.Models;
using System.Linq;
using System.Threading.Tasks;
using WpfLikeAvaloniaNavigation;

namespace pr15Avalonia.Pages;

public partial class FinalConfiguration : Page
{
    private readonly PcDbContext _context = new();

    public FinalConfiguration()
    {
        InitializeComponent();
        LoadBuild();
    }

    private void LoadBuild()
    {
        CpuText.Text = FormatPart("Процессор", CurrentBuild.CpuBasePart);
        GpuText.Text = FormatPart("Видеокарта", CurrentBuild.GpuBasePart);
        MotherboardText.Text = FormatPart("Материнская плата", CurrentBuild.MotherboardBasePart);
        RamText.Text = FormatPart("Оперативная память", CurrentBuild.RamBasePart);
        CaseText.Text = FormatPart("Корпус", CurrentBuild.CaseBasePart);
        PowerText.Text = FormatPart("Блок питания", CurrentBuild.PowerSupplyBasePart);
        CoolerText.Text = FormatPart("Кулер", CurrentBuild.ProcessorCoolerBasePart);

        if (CurrentBuild.SelectedStorageDevices.Any())
        {
            var storageList = string.Join(", ",
                CurrentBuild.SelectedStorageDevices
                    .Where(s => s.IdNavigation != null)
                    .Select(s => $"{s.IdNavigation.Name} ({s.IdNavigation.Price:N0} ₽)"));
            StorageText.Text = $"Накопители: {storageList}";
        }
        else
        {
            StorageText.Text = "Накопители: не выбраны";
        }

        TotalPriceText.Text = $"Итого: {CurrentBuild.GetTotalPrice():N0} ₽";
    }

    private string FormatPart(string title, Models.Basepart part)
    {
        if (part == null) return $"{title}: не выбран";
        return $"{title}: {part.Name} ({part.Price:N0} ₽)";
    }

    private async void SaveAssembly_Click(object? sender, RoutedEventArgs e)
    {
        var name = AssemblyNameBox.Text?.Trim();
        var author = AuthorNameBox.Text?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            AssemblyNameBox.Watermark = "Введите название!";
            return;
        }
        if (string.IsNullOrEmpty(author))
        {
            AuthorNameBox.Watermark = "Введите автора!";
            return;
        }

        var partIds = new System.Collections.Generic.List<int>();
        if (CurrentBuild.CpuBasePart != null) partIds.Add(CurrentBuild.CpuBasePart.Id);
        if (CurrentBuild.GpuBasePart != null) partIds.Add(CurrentBuild.GpuBasePart.Id);
        if (CurrentBuild.MotherboardBasePart != null) partIds.Add(CurrentBuild.MotherboardBasePart.Id);
        if (CurrentBuild.RamBasePart != null) partIds.Add(CurrentBuild.RamBasePart.Id);
        if (CurrentBuild.CaseBasePart != null) partIds.Add(CurrentBuild.CaseBasePart.Id);
        if (CurrentBuild.PowerSupplyBasePart != null) partIds.Add(CurrentBuild.PowerSupplyBasePart.Id);
        if (CurrentBuild.ProcessorCoolerBasePart != null) partIds.Add(CurrentBuild.ProcessorCoolerBasePart.Id);
        foreach (var s in CurrentBuild.SelectedStorageDevices)
            if (s.IdNavigation != null) partIds.Add(s.IdNavigation.Id);

        var assembly = new Assembly { Name = name, Author = author };
        _context.Assemblies.Add(assembly);
        await _context.SaveChangesAsync();

        foreach (var partId in partIds)
        {
            _context.Partassemblies.Add(new Partassembly
            {
                Assemblyid = assembly.Id,
                Partid = partId
            });
        }
        await _context.SaveChangesAsync();

        NavigationService.Navigate(new AssemblyListPage());
    }
    private void Back_Click(object sender, RoutedEventArgs e)
    {
        CurrentBuild.SelectedCpu = null;
        // CurrentBuild.SelectedGpu = null;
        // CurrentBuild.SelectedProcessorCooler = null;
        CurrentBuild.SelectedMotherboard = null;
        // CurrentBuild.SelectedRam = null;
        // CurrentBuild.SelectedCase = null;
        // CurrentBuild.SelectedPowerSupply = null;
        // CurrentBuild.SelectedStorageDevices = null;
        CurrentBuild.CpuBasePart = null;
        // CurrentBuild.GpuBasePart = null;
        CurrentBuild.MotherboardBasePart = null;
        // CurrentBuild.RamBasePart = null;
        // CurrentBuild.CaseBasePart = null;
        // CurrentBuild.PowerSupplyBasePart = null;
        // CurrentBuild.ProcessorCoolerBasePart = null;
        NavigationService?.Navigate(new MainPage());
            
    }
}