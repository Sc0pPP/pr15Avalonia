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

public partial class ChoiceSSDPage : Page
{

    private readonly PcDbContext _context;
    private ObservableCollection<Storagedevice> _allStorageDevice; // Изменено на ObservableCollection
    private ObservableCollection<Storagedevice> _filteredStorageDevice; // Изменено на ObservableCollection

    public ChoiceSSDPage()
    {
        InitializeComponent();
        _context = new PcDbContext();
        LoadStorageDevices();
    }

    private async void LoadStorageDevices()
    {
        try
        {
            var SSDListBox = this.FindControl<ListBox>("SSDListBox");

            // отдельный список для ssd
            // отдельный список для hdd
            var Storagedevices = await _context.Storagedevices
                .Include(m => m.IdNavigation) // Basepart
                .ThenInclude(bp => bp.Manufacturer)
                .Include(s=>s.Ssd)
                .ThenInclude(s=>s.Tbw)
                .Include(h=>h.Hdd)
                .ThenInclude(h=>h.Rotationspeed)
                .ToListAsync();



            _allStorageDevice = new ObservableCollection<Storagedevice>(Storagedevices);
            _filteredStorageDevice = new ObservableCollection<Storagedevice>(Storagedevices);

            SSDListBox.ItemsSource = _allStorageDevice;
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
            var SSDListBox = this.FindControl<ListBox>("SSDListBox");

            var searchText = searchBox?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredStorageDevice = new ObservableCollection<Storagedevice>(_allStorageDevice);
            }
            else
            {
                var filtered = _allStorageDevice
                    .Where(m => m.IdNavigation != null &&
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredStorageDevice = new ObservableCollection<Storagedevice>(filtered);
            }

            SSDListBox.ItemsSource = _filteredStorageDevice;
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
            var SSDListBox = this.FindControl<ListBox>("SSDListBox");

            var searchText = searchBox?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredStorageDevice = new ObservableCollection<Storagedevice>(_allStorageDevice);
            }
            else
            {
                var filtered = _allStorageDevice
                    .Where(m => m.IdNavigation != null &&
                                m.IdNavigation.Name.ToLower().Contains(searchText))
                    .ToList();
                _filteredStorageDevice = new ObservableCollection<Storagedevice>(filtered);
            }

            SSDListBox.ItemsSource = _filteredStorageDevice;
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
            var SSDListBox = this.FindControl<ListBox>("SSDListBox");

            var sorted = _filteredStorageDevice
                .OrderBy(m => m.IdNavigation?.Name ?? "")
                .ToList();

            _filteredStorageDevice = new ObservableCollection<Storagedevice>(sorted);
            SSDListBox.ItemsSource = _filteredStorageDevice;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сортировки: {ex.Message}");
        }    }
    

    private void MotherboardListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {

        try
        {
            var selectedInfoText = this.FindControl<TextBlock>("SelectedInfoText");
            var SSDListBox = sender as ListBox;

            if (SSDListBox?.SelectedItem is Ram selected &&
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
            var SSDListBox = this.FindControl<ListBox>("SSDListBox");

            if (SSDListBox.SelectedItem is Storagedevice selected &&
                selected.IdNavigation != null)
            {
                CurrentBuild.SelectedStorageDevices.Add(selected);

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
