using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using crud_wpf_mvvm_origin.Codes;
using crud_wpf_mvvm_origin.Data;
using crud_wpf_mvvm_origin.Models;

namespace crud_wpf_mvvm_origin.ViewModels;

public partial class ClienteViewModel : Notify<ClienteViewModel>, IDisposable
{
    protected readonly AppDbContext _context;

    public ICommand NewCommand { get; set; }
    public ICommand EditModeOnCommand { get; set; }
    public ICommand EditModeOffCommand { get; set; }
    public ICommand CreateCommand { get; set; }
    public ICommand EditCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public ICommand GetAllCommand { get; set; }

    public ObservableCollection<Cliente> Clientes { get ; set ; } = new();

    private Cliente _cliente = new();
    public Cliente Cliente
    {
        get => _cliente;
        
        set
        {
            if (_cliente != value)
            {
                OnPropertyChanging(nameof(Cliente));
                _cliente = value;
                OnPropertyChanged(nameof(Cliente));
            }
        }
    }

    public Cliente ClienteEditMode { get ; set ; } = new();

    private string _hasErrorsCodeBehind = string.Empty;
    public string HasErrorsCodeBehind
    {
        get => _hasErrorsCodeBehind;
        
        set
        {
            if (_hasErrorsCodeBehind != value)
            {
                OnPropertyChanging(nameof(HasErrorsCodeBehind));
                _hasErrorsCodeBehind = value;
                OnPropertyChanged(nameof(HasErrorsCodeBehind));
            }
        }
    }
    
    public ClienteViewModel()
    {
        NewCommand = new Command<object>(NewExecute, parameter => !Cliente.HasErrors);
        EditModeOnCommand = new Command<object>(EditModeOnExecute, parameter => !Cliente.HasErrors);
        EditModeOffCommand = new Command<object>(EditModeOffExecute, parameter => !Cliente.HasErrors);
        CreateCommand = new Command<object>(CreateExecute, parameter => !Cliente.HasErrors);
        EditCommand = new Command<object>(EditExecute, parameter => !Cliente.HasErrors);
        DeleteCommand = new Command<object>(DeleteExecute, parameter => !Cliente.HasErrors);
        GetAllCommand = new Command<object>(GetAllExecute, parameter => !Cliente.HasErrors);

        _context = new AppDbContext();
    }

    private void NewExecute(object? parameter)
    {
        Cliente = new();
    }

    private void EditModeOnExecute(object? parameter)
    {
        ClienteEditMode.Nome = Cliente.Nome;
        ClienteEditMode.Email = Cliente.Email;
    }


    private void EditModeOffExecute(object? parameter)
    {
        Cliente.Nome = ClienteEditMode.Nome;
        Cliente.Email = ClienteEditMode.Email;
    }

    private void CreateExecute(object? parameter)
    {
        try
        {
            HasErrorsCodeBehind = string.Empty;

            _context.Entry(Cliente).State = EntityState.Added;
            _context.SaveChanges();
        }
        catch (System.Exception ex)
        {
            HasErrorsCodeBehind = ex.Message;
        }
    }

    private void EditExecute(object? parameter)
    {
        try
        {
            HasErrorsCodeBehind = string.Empty;

            _context.Entry(Cliente).State = EntityState.Modified;
            _context.SaveChanges();
            
            EditModeOnExecute(parameter);
        }
        catch (System.Exception ex)
        {
            HasErrorsCodeBehind = ex.Message;
        }
    }

    private void DeleteExecute(object? parameter)
    {
        try
        {
            HasErrorsCodeBehind = string.Empty;

            _context.Entry(Cliente).State = EntityState.Deleted;
            _context.SaveChanges();
        }
        catch (System.Exception ex)
        {
            HasErrorsCodeBehind = ex.Message;
        }
    }

    private void GetAllExecute(object? parameter)
    {
        Clientes.Clear();

        foreach (var item in _context.Clientes.OrderBy(x => x.Id).ToList())
        {
            Clientes.Add(item);
        }
    }    

    public void Dispose()
    {
        _context.Dispose();
    }
}
