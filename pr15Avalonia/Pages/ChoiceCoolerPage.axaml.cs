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
using pr15Avalonia.Pages;
using WpfLikeAvaloniaNavigation;

namespace pr15Avalonia;

public partial class ChoiceCoolerPage : Page
{
    private readonly PcDbContext _context;
    private ObservableCollection<Processorcooler> _allCooler; // Изменено на ObservableCollection
    private ObservableCollection<Processorcooler> _filteredCooler; // Изменено на ObservableCollection

    public ChoiceCoolerPage()
    {
        InitializeComponent();
        _context = new PcDbContext();
        LoadCooler();
    }

    private async void LoadCooler()
    {
        try
        {
            var CoolerListBox = this.FindControl<ListBox>("CoolerListBox");

            var Coolers = await _context.Processorcoolers
                .Include(m => m.IdNavigation) // Basepart
                .ThenInclude(bp => bp.Manufacturer)
                .Include(p => p.Fandimension)
                .ToListAsync();



            _allCooler = new ObservableCollection<Processorcooler>(Coolers);
            _filteredCooler = new ObservableCollection<Processorcooler>(Coolers);

            CoolerListBox.ItemsSource = _filteredCooler;
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
            var CoolerListBox = this.FindControl<ListBox>("CoolerListBox");

            var searchText = searchBox?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredCooler = new ObservableCollection<Processorcooler>(_allCooler);
            }
            else
            {
                var filtered = _allCooler
                    .Where(m => m.IdNavigation != null &&
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredCooler = new ObservableCollection<Processorcooler>(filtered);
            }

            CoolerListBox.ItemsSource = _filteredCooler;
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
            var CoolerListBox = this.FindControl<ListBox>("CoolerListBox");

            var searchText = searchBox?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredCooler = new ObservableCollection<Processorcooler>(_allCooler);
            }
            else
            {
                var filtered = _allCooler
                    .Where(m => m.IdNavigation != null &&
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredCooler = new ObservableCollection<Processorcooler>(filtered);
            }

            CoolerListBox.ItemsSource = _filteredCooler;
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
            var CoolerListBox = this.FindControl<ListBox>("CoolerListBox");

            var sorted = _filteredCooler
                .OrderBy(m => m.IdNavigation?.Name ?? "")
                .ToList();

            _filteredCooler = new ObservableCollection<Processorcooler>(sorted);
            CoolerListBox.ItemsSource = _filteredCooler;
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
            var CoolerListBox = sender as ListBox;

            if (CoolerListBox?.SelectedItem is Processorcooler selected &&
                selected.IdNavigation != null)
            {
                selectedInfoText.Text = $"Выбрано: {selected.IdNavigation.Name}";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка выбора: {ex.Message}");
        }
    }

    private void Select_Click(object? sender, RoutedEventArgs e)
    {

        try
        {
            var CoolerListBox = this.FindControl<ListBox>("CoolerListBox");

            if (CoolerListBox.SelectedItem is Processorcooler selected &&
                selected.IdNavigation != null)
            {
                CurrentBuild.SelectedProcessorCooler = selected;
                CurrentBuild.ProcessorCoolerBasePart = selected.IdNavigation;

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
                    NavigationService.Navigate(new ChoiceProcessor());
                }

            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка выбора: {ex.Message}");
        }

        NavigationService.Navigate(new ChoiceRamPage());
    }
}