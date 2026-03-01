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

public partial class ChoiceGpuPage : Page
{
    private readonly PcDbContext _context;
    private ObservableCollection<Gpu> _allGpu; 
    private ObservableCollection<Gpu> _filteredGpu; 
    public ChoiceGpuPage()
    {
        InitializeComponent();
        _context = new PcDbContext();
        LoadGpu();
    }
    private async void LoadGpu()
    {
        try
        {
            var GpuListBox = this.FindControl<ListBox>("GpuListBox");

            var Gpus = await _context.Gpus
                .Include(m => m.IdNavigation) // Basepart
                .ThenInclude(bp => bp.Manufacturer)
                .Include(p => p.Gpuinterface)
                .ToListAsync();
            
            _allGpu = new ObservableCollection<Gpu>(Gpus);
            _filteredGpu = new ObservableCollection<Gpu>(Gpus);

            GpuListBox.ItemsSource = _filteredGpu;
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
                _filteredGpu = new ObservableCollection<Gpu>(_allGpu);
            }
            else
            {
                var filtered = _allGpu
                    .Where(m => m.IdNavigation != null && 
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredGpu = new ObservableCollection<Gpu>(filtered);
            }

            GpuListBox.ItemsSource = _filteredGpu;
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
            var GpuListBox = this.FindControl<ListBox>("GpuListBox");
                
            var searchText = searchBox?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredGpu = new ObservableCollection<Gpu>(_allGpu);
            }
            else
            {
                var filtered = _allGpu
                    .Where(m => m.IdNavigation != null && 
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredGpu = new ObservableCollection<Gpu>(filtered);
            }

            GpuListBox.ItemsSource = _filteredGpu;
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
            var GpuListBox = this.FindControl<ListBox>("GpuListBox");
                
            var sorted = _filteredGpu
                .OrderBy(m => m.IdNavigation?.Name ?? "")
                .ToList();
                
            _filteredGpu = new ObservableCollection<Gpu>(sorted);
            GpuListBox.ItemsSource = _filteredGpu;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сортировки: {ex.Message}");
        }   
    }

    private void GpuListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        
        try
        {
            var selectedInfoText = this.FindControl<TextBlock>("SelectedInfoText");
            var GpuListBox = sender as ListBox;
                
            if (GpuListBox?.SelectedItem is Gpu selected && 
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
            var GpuListBox = this.FindControl<ListBox>("GpuListBox");
                
            if (GpuListBox.SelectedItem is Gpu selected && 
                selected.IdNavigation != null)
            {
                CurrentBuild.SelectedGpu = selected;
                CurrentBuild.GpuBasePart = selected.IdNavigation;

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
        NavigationService.Navigate(new ChoiceCoolerPage() );
    }
    }
