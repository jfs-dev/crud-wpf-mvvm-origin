using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using crud_wpf_mvvm_origin.ViewModels;

namespace crud_wpf_mvvm_origin.Views;

/// <summary>
/// Interaction logic for ClientePesquisaView.xaml
/// </summary>
public partial class ClientePesquisaView : Window
{
    ClienteViewModel _clienteViewModel = new();

    public ClientePesquisaView()
    {
        InitializeComponent();

        DataContext = _clienteViewModel;
    }

    private void ClientePesquisaWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _clienteViewModel.GetAllCommand.Execute(null);
    }

    private void ClientePesquisaWindow_Closing(object sender, CancelEventArgs e)
    {
        _clienteViewModel.Dispose();
    }

    private void NewButton_Click(object sender, RoutedEventArgs e)
    {
        var windowUri = new ClienteView();

        _clienteViewModel.NewCommand.Execute(null);
        _clienteViewModel.Cliente.ValidateAllProperties();
 
        windowUri.DataContext = _clienteViewModel;
        windowUri.Owner = this;
        windowUri.CreateButton.Visibility = Visibility;

        windowUri.ShowDialog();

        _clienteViewModel.GetAllCommand.Execute(null);
    }

    private void ViewButton_Click(object sender, RoutedEventArgs e)
    {
        var windowUri = new ClienteView();

        _clienteViewModel.EditModeOnCommand.Execute(null);

        windowUri.DataContext = _clienteViewModel;
        windowUri.Owner = this;
        windowUri.NomeTextBox.IsEnabled = false;
        windowUri.EmailTextBox.IsEnabled = false;        
        windowUri.ViewButton.Visibility = Visibility;

        windowUri.ShowDialog();

        _clienteViewModel.EditModeOffCommand.Execute(null);
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        var windowUri = new ClienteView();

        _clienteViewModel.EditModeOnCommand.Execute(null);

        windowUri.DataContext = _clienteViewModel;
        windowUri.Owner = this;
        windowUri.EditButton.Visibility = Visibility;

        windowUri.ShowDialog();

        _clienteViewModel.EditModeOffCommand.Execute(null);
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var windowUri = new ClienteView();

        windowUri.DataContext = _clienteViewModel;
        windowUri.Owner = this;
        windowUri.NomeTextBox.IsEnabled = false;
        windowUri.EmailTextBox.IsEnabled = false;        
        windowUri.DeleteButton.Visibility = Visibility;

        windowUri.ShowDialog();

        _clienteViewModel.GetAllCommand.Execute(null);
    }
}