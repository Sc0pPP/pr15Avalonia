using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using pr15Avalonia.Models;
using WpfLikeAvaloniaNavigation;

namespace pr15Avalonia.Pages;

public partial class ChoiceProcessor : Page
{
    private readonly PcDbContext _context;
    private ObservableCollection<Cpu> _allCpu; // Изменено на ObservableCollection
    private ObservableCollection<Cpu> _filteredCpu; // Изменено на ObservableCollection
    public ChoiceProcessor()
    {
        InitializeComponent();
        _context = new PcDbContext();
        LoadCpu();
    }
    private async void LoadCpu()
    {
        try
        {
            var CpuListBox = this.FindControl<ListBox>("CpuListBox");

            var Cpus = await _context.Cpus
                .Include(m => m.IdNavigation) // Basepart
                .ThenInclude(bp => bp.Manufacturer)
                .Include(m => m.Socket)
                .ToListAsync();

          
            _allCpu = new ObservableCollection<Cpu>(Cpus);
            _filteredCpu = new ObservableCollection<Cpu>(Cpus);

            ProcessorListBox.ItemsSource = _filteredCpu;
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

    private void SearchBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        try
        {
            var searchBox = sender as TextBox;
            var СpuListBox = this.FindControl<ListBox>("CpuListBox");
                
            var searchText = searchBox?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredCpu = new ObservableCollection<Cpu>(_allCpu);
            }
            else
            {
                var filtered = _allCpu
                    .Where(m => m.IdNavigation != null && 
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredCpu = new ObservableCollection<Cpu>(filtered);
            }

            ProcessorListBox.ItemsSource = _filteredCpu;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка поиска: {ex.Message}");
        }    }

    private void SortByPrice_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var CpuListBox = this.FindControl<ListBox>("CpuListBox");
                
            var sorted = _filteredCpu
                .OrderBy(m => m.IdNavigation?.Price ?? 0)
                .ToList();
                
            _filteredCpu = new ObservableCollection<Cpu>(sorted);
            ProcessorListBox.ItemsSource = _filteredCpu;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сортировки: {ex.Message}");
        }    }

    private void SortByName_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var CpuListBox = this.FindControl<ListBox>("CpuListBox");
                
            var sorted = _filteredCpu
                .OrderBy(m => m.IdNavigation?.Name ?? "")
                .ToList();
                
            _filteredCpu = new ObservableCollection<Cpu>(sorted);
            ProcessorListBox.ItemsSource = _filteredCpu;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сортировки: {ex.Message}");
        }    }

    private void ProcessorListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {

        try
        {
            var selectedInfoText = this.FindControl<TextBlock>("SelectedInfoText");
            var ProcessorListBox = sender as ListBox;
                
            if (ProcessorListBox?.SelectedItem is Cpu selected && 
                selected.IdNavigation != null)
            {
                selectedInfoText.Text = $"Выбрано: {selected.IdNavigation.Name}";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка выбора: {ex.Message}");
        }    }

    private void Select_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var ProcessorListBox = this.FindControl<ListBox>("ProcessorListBox");
                
            if (ProcessorListBox.SelectedItem is Cpu selected && 
                selected.IdNavigation != null)
            {
                CurrentBuild.SelectedCpu = selected;
                CurrentBuild.CpuBasePart = selected.IdNavigation;

                var errors = CompatibilityChecker.ValidateCurrentBuild(_context);
                if (errors.Any())
                {
                    
                    var mainWindow = (Application.Current.ApplicationLifetime
                        as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
                    var dialogg= new Messagebox("Проблемы совместимости: " + string.Join(", ", errors));
                    dialogg.ShowDialog(mainWindow);
                    return;
                    Console.WriteLine("Проблемы совместимости: " + string.Join(", ", errors));
                }
                else
                {
                    Console.WriteLine("Материнская плата добавлена успешно!");
                    NavigationService.Navigate(new ChoiceProcessor());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка выбора: {ex.Message}");
        }
        NavigationService.Navigate(new ChoiceGpuPage() );
    }
}