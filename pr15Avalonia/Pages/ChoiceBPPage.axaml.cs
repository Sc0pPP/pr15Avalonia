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

public partial class ChoiceBPPage : Page
{
    private readonly PcDbContext _context;
    private ObservableCollection<Powersupply> _allBP; // Изменено на ObservableCollection
    private ObservableCollection<Powersupply> _filteredBP; // Изменено на ObservableCollection

    public ChoiceBPPage()
    {
        InitializeComponent();
        _context = new PcDbContext();
        LoadBP();
    }

    private async void LoadBP()
    {
        try
        {
            var BPListBox = this.FindControl<ListBox>("BPListBox");

            var BPs = await _context.Powersupplies
                .Include(m => m.IdNavigation) // Basepart
                .ThenInclude(bp => bp.Manufacturer)
                .Include(p => p.Fandimension)
                .Include(p => p.Certification)

                .ToListAsync();



            _allBP = new ObservableCollection<Powersupply>(BPs);
            _filteredBP = new ObservableCollection<Powersupply>(BPs);
        
        BPListBox.ItemsSource = _filteredBP;
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
            var BPListBox = this.FindControl<ListBox>("BPListBox");

            var searchText = searchBox?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredBP = new ObservableCollection<Powersupply>(_allBP);
            }
            else
            {
                var filtered = _allBP
                    .Where(m => m.IdNavigation != null &&
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredBP = new ObservableCollection<Powersupply>(filtered);
            }

            BPListBox.ItemsSource = _filteredBP;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка поиска: {ex.Message}");
        }    }

    private void SortByPrice_Click(object? sender, RoutedEventArgs e)
    {

        try
        {
            var searchBox = sender as TextBox;
            var BPListBox = this.FindControl<ListBox>("BPListBox");

            var searchText = searchBox?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredBP = new ObservableCollection<Powersupply>(_allBP);
            }
            else
            {
                var filtered = _allBP
                    .Where(m => m.IdNavigation != null &&
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredBP = new ObservableCollection<Powersupply>(filtered);
            }

            BPListBox.ItemsSource = _filteredBP;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка поиска: {ex.Message}");
        }    }

    private void SortByName_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var BPListBox = this.FindControl<ListBox>("BPListBox");

            var sorted = _filteredBP
                .OrderBy(m => m.IdNavigation?.Name ?? "")
                .ToList();

            _filteredBP = new ObservableCollection<Powersupply>(sorted);
            BPListBox.ItemsSource = _filteredBP;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сортировки: {ex.Message}");
        }   
    }

    private void CoolerListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        try
        {
            var selectedInfoText = this.FindControl<TextBlock>("SelectedInfoText");
            var BPListBox = sender as ListBox;

            if (BPListBox?.SelectedItem is Powersupply selected &&
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
            var BPListBox = this.FindControl<ListBox>("BPListBox");

            if (BPListBox.SelectedItem is Powersupply selected &&
                selected.IdNavigation != null)
            {
                CurrentBuild.SelectedPowerSupply = selected;
                CurrentBuild.PowerSupplyBasePart = selected.IdNavigation;

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
                    NavigationService.Navigate(new ChoiceCasePage());
                }

            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка выбора: {ex.Message}");
        }
        
    }
}   