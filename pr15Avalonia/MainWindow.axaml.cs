using Avalonia.Controls;
using pr15Avalonia.Pages;

namespace pr15Avalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainContent.Navigate(new MainPage());
    }
}