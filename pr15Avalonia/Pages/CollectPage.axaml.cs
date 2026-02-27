using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using pr15Avalonia.Models;
using WpfLikeAvaloniaNavigation;

namespace pr15Avalonia.Pages
{
    public partial class CollectPage : Page
    {
        private readonly PcDbContext _context;
        private ObservableCollection<Motherboard> _allMotherboards; // Изменено на ObservableCollection
        private ObservableCollection<Motherboard> _filteredMotherboards; // Изменено на ObservableCollection

        public CollectPage()
        {
            InitializeComponent();
            _context = new PcDbContext();
            LoadMotherboards();
            
        }
        private async void LoadMotherboards()
        {
            try
            {
                var motherboardListBox = this.FindControl<ListBox>("MotherboardListBox");

                var motherboards = await _context.Motherboards
                    .Include(m => m.IdNavigation) // Basepart
                    .ThenInclude(bp => bp.Manufacturer)
                    .Include(m => m.Socket)
                    .Include(m => m.Formfactor)
                    .Include(m => m.Memorytype)
                    .ToListAsync();

                // Фильтрация по совместимости
                if (CurrentBuild.SelectedCpu != null)
                {
                    motherboards = motherboards
                        .Where(m => m.Socketid == CurrentBuild.SelectedCpu.Socketid)
                        .ToList();
                }

                _allMotherboards = new ObservableCollection<Motherboard>(motherboards);
                _filteredMotherboards = new ObservableCollection<Motherboard>(motherboards);

                motherboardListBox.ItemsSource = _filteredMotherboards;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        
        

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var searchBox = sender as TextBox;
                var motherboardListBox = this.FindControl<ListBox>("MotherboardListBox");
                
                var searchText = searchBox?.Text?.ToLower() ?? "";

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    _filteredMotherboards = new ObservableCollection<Motherboard>(_allMotherboards);
                }
                else
                {
                    var filtered = _allMotherboards
                        .Where(m => m.IdNavigation != null && 
                                    m.IdNavigation.Name.ToLower().Contains(searchText))
                        .ToList();
                    _filteredMotherboards = new ObservableCollection<Motherboard>(filtered);
                }

                motherboardListBox.ItemsSource = _filteredMotherboards;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка поиска: {ex.Message}");
            }
        }

        private void SortByPrice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var motherboardListBox = this.FindControl<ListBox>("MotherboardListBox");
                
                var sorted = _filteredMotherboards
                    .OrderBy(m => m.IdNavigation?.Price ?? 0)
                    .ToList();
                
                _filteredMotherboards = new ObservableCollection<Motherboard>(sorted);
                motherboardListBox.ItemsSource = _filteredMotherboards;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сортировки: {ex.Message}");
            }
        }

        private void SortByName_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var motherboardListBox = this.FindControl<ListBox>("MotherboardListBox");
                
                var sorted = _filteredMotherboards
                    .OrderBy(m => m.IdNavigation?.Name ?? "")
                    .ToList();
                
                _filteredMotherboards = new ObservableCollection<Motherboard>(sorted);
                motherboardListBox.ItemsSource = _filteredMotherboards;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сортировки: {ex.Message}");
            }
        }

        private void MotherboardListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedInfoText = this.FindControl<TextBlock>("SelectedInfoText");
                var motherboardListBox = sender as ListBox;
                
                if (motherboardListBox?.SelectedItem is Motherboard selected && 
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

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var motherboardListBox = this.FindControl<ListBox>("MotherboardListBox");
                
                if (motherboardListBox.SelectedItem is Motherboard selected && 
                    selected.IdNavigation != null)
                {
                    CurrentBuild.SelectedMotherboard = selected;
                    CurrentBuild.MotherboardBasePart = selected.IdNavigation;

                    var errors = CompatibilityChecker.ValidateCurrentBuild(_context);
                    if (errors.Any())
                    {
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
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            
                NavigationService?.GoBack();
            
        }
    }
}