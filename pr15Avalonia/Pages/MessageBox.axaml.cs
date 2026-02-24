using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace pr15Avalonia.Pages;

public partial class Messagebox : Window
{
    public Messagebox(string message)
    {
        InitializeComponent();
        MessageText.Text = message;
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}