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

public partial class ChoiceRamPage : Page
{
    private readonly PcDbContext _context;
    private ObservableCollection<Ram> _allRam; // Изменено на ObservableCollection
    private ObservableCollection<Ram> _filteredRam; // Изменено на ObservableCollection

    public ChoiceRamPage()
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

    private void Select_Click(object? sender, RoutedEventArgs e)
    {
      
        try
        {
            var RamListBox = this.FindControl<ListBox>("RamListBox");

            if (RamListBox.SelectedItem is Ram selected &&
                selected.IdNavigation != null)
            {
                CurrentBuild.SelectedRam = selected;
                CurrentBuild.RamBasePart = selected.IdNavigation;

                var errors = CompatibilityChecker.ValidateCurrentBuild(_context);
                if (errors.Any())
                {

                    var mainWindow = (Application.Current.ApplicationLifetime
                        as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
                    var dialogg = new Messagebox("Проблемы совместимости: " + string.Join(", ", errors));
                    dialogg.ShowDialog(mainWindow);
                    return;
                    Console.WriteLine("Проблемы совместимости: " + string.Join(", ", errors));
                }
                else
                {
                    Console.WriteLine("Материнская плата добавлена успешно!");
                    NavigationService.Navigate(new ChoiceBPPage());
                }

            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка выбора: {ex.Message}");
        }

        NavigationService.Navigate(new ChoiceBPPage());
    }  
    

    private void SearchBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        try
        {
            var searchBox = sender as TextBox;
            var RamListBox = this.FindControl<ListBox>("RamListBox");

            var searchText = searchBox?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredRam = new ObservableCollection<Ram>(_allRam);
            }
            else
            {
                var filtered = _allRam
                    .Where(m => m.IdNavigation != null &&
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredRam = new ObservableCollection<Ram>(filtered);
            }

            CoolerListBox.ItemsSource = _filteredRam;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка поиска: {ex.Message}");
        }
        
    }

    private void SortByPrice_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var searchBox = sender as TextBox;
            var RamListBox = this.FindControl<ListBox>("RamListBox");

            var searchText = searchBox?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredRam = new ObservableCollection<Ram>(_allRam);
            }
            else
            {
                var filtered = _allRam
                    .Where(m => m.IdNavigation != null &&
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredRam = new ObservableCollection<Ram>(filtered);
            }

            CoolerListBox.ItemsSource = _filteredRam;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка поиска: {ex.Message}");
        }
        
    }

    private void SortByName_Click(object? sender, RoutedEventArgs e)
    {

        try
        {
            var RamListBox = this.FindControl<ListBox>("RamListBox");

            var sorted = _filteredRam
                .OrderBy(m => m.IdNavigation?.Name ?? "")
                .ToList();

            _filteredRam = new ObservableCollection<Ram>(sorted);
            CoolerListBox.ItemsSource = _filteredRam;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сортировки: {ex.Message}");
        }    }

    private void CoolerListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {

        try
        {
            var selectedInfoText = this.FindControl<TextBlock>("SelectedInfoText");
            var RamListBox = sender as ListBox;

            if (CoolerListBox?.SelectedItem is Ram selected &&
                selected.IdNavigation != null)
            {
                selectedInfoText.Text = $"Выбрано: {selected.IdNavigation.Name}";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка выбора: {ex.Message}");
        }    }
}