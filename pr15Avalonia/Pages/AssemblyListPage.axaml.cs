using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using pr15Avalonia.Models;
using WpfLikeAvaloniaNavigation;

namespace pr15Avalonia.Pages;

public partial class AssemblyListPage : Page
{
    private PcDbContext _context = new();
    private Assembly? _selectedAssembly;

    public AssemblyListPage()
    {
        InitializeComponent();
        LoadAssemblies();
    }

    private async void LoadAssemblies()
    {
        var assemblies = await _context.Assemblies
            .Include(a => a.Partassemblies)
            .ToListAsync();

        AssemblyListBox.ItemsSource = assemblies;
    }

    private void AssemblyListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selectedAssembly = AssemblyListBox.SelectedItem as Assembly; 
        if (_selectedAssembly != null)
            SelectedInfoText.Text = _selectedAssembly.Name;
        else
            SelectedInfoText.Text = string.Empty;
    }

    private void Open_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedAssembly != null)
        {
            NavigationService.Navigate(new AssemblyDetailsPage(_selectedAssembly.Id));
        }
    }

    private void Back_Click(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainPage());
    }
}