using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using pr15Avalonia.Models;
using WpfLikeAvaloniaNavigation;

namespace pr15Avalonia.Pages;

public partial class ChoiceSSDPage : Page
{
    private readonly PcDbContext _context;
    private ObservableCollection<Ram> _allSSD; // Изменено на ObservableCollection
    private ObservableCollection<Ram> _filteredSSD; // Изменено на ObservableCollection

    public ChoiceSSDPage()
    {
        InitializeComponent();
        _context = new PcDbContext();
        LoadRam();
    }
    private async void LoadRam()
    {
        try
        {
            var RamListBox = this.FindControl<ListBox>("RamListBox");

            var Rams = await _context.Rams
                .Include(m => m.IdNavigation) // Basepart
                .ThenInclude(bp => bp.Manufacturer)
                .Include(p => p.Memorytype)

                .ToListAsync();



            _allRam = new ObservableCollection<Ram>(Rams);
            _filteredRam = new ObservableCollection<Ram>(Rams);

            CoolerListBox.ItemsSource = _filteredRam;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }
    }

    private void Back_Click(object? sender, RoutedEventArgs e)
    {
        NavigationService?.GoBack();
    }
    private void Back_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void SearchBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void SortByPrice_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void SortByName_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void CoolerListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void Select_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}