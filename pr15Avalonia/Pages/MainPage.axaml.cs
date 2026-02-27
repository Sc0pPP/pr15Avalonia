using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using WpfLikeAvaloniaNavigation;
namespace pr15Avalonia.Pages;

public partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new CollectPage());
    }

    private void Button_OnClick1(object? sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new AssemblyListPage());
    }
}