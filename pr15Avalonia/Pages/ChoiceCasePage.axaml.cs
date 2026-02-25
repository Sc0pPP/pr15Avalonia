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

public partial class ChoiceCasePage : Page
{
    private readonly PcDbContext _context;
    private ObservableCollection<Case> _allCase; // Изменено на ObservableCollection
    private ObservableCollection<Case> _filteredCase; // Изменено на ObservableCollection

    public ChoiceCasePage()
    {
        InitializeComponent();
        _context = new PcDbContext();
        LoadCase();
    }
    private async void LoadCase()
    {
        try
        {
            var CaseListBox = this.FindControl<ListBox>("CaseListBox");

            var Cases = await _context.Cases
                .Include(m => m.IdNavigation) // Basepart
                .ThenInclude(bp => bp.Manufacturer)
                .Include(p => p.Boardformfactorcases)
                .ThenInclude(p=>p.Formfactor)

                .ToListAsync();



            _allCase = new ObservableCollection<Case>(Cases);
            _filteredCase = new ObservableCollection<Case>(Cases);

            CaseListBox.ItemsSource = _filteredCase;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }
    }


    private void Back_Click(object? sender, RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }

    private void SearchBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        try
        {
            var searchBox = sender as TextBox;
            var CaseListBox = this.FindControl<ListBox>("CaseListBox");

            var searchText = searchBox?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredCase = new ObservableCollection<Case>(_allCase);
            }
            else
            {
                var filtered = _allCase
                    .Where(m => m.IdNavigation != null &&
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredCase = new ObservableCollection<Case>(filtered);
            }

            CaseListBox.ItemsSource = _filteredCase;
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
            var CaseListBox = this.FindControl<ListBox>("CaseListBox");

            var searchText = searchBox?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredCase = new ObservableCollection<Case>(_allCase);
            }
            else
            {
                var filtered = _allCase
                    .Where(m => m.IdNavigation != null &&
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredCase = new ObservableCollection<Case>(filtered);
            }

            CaseListBox.ItemsSource = _filteredCase;
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
            var CaseListBox = this.FindControl<ListBox>("CaseListBox");

            var sorted = _filteredCase
                .OrderBy(m => m.IdNavigation?.Name ?? "")
                .ToList();

            _filteredCase = new ObservableCollection<Case>(sorted);
            CaseListBox.ItemsSource = _filteredCase;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сортировки: {ex.Message}");
        }       }

    private void CoolerListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        try
        {
            var selectedInfoText = this.FindControl<TextBlock>("SelectedInfoText");
            var CaseListBox = sender as ListBox;

            if (CaseListBox?.SelectedItem is Case selected &&
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
            var CaseListBox = this.FindControl<ListBox>("CaseListBox");

            if (CaseListBox.SelectedItem is Case selected &&
                selected.IdNavigation != null)
            {
                CurrentBuild.SelectedCase = selected;
                CurrentBuild.CaseBasePart = selected.IdNavigation;

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
}