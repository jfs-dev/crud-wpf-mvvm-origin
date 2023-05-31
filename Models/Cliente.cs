using System.ComponentModel.DataAnnotations;
using crud_wpf_mvvm_origin.Codes;

namespace crud_wpf_mvvm_origin.Models;

public class Cliente : Notify<Cliente>
{
    private string _nome = string.Empty;
    private string _email = string.Empty;

    [Key]
    [Required()]
    public int Id { get; set; }

    [Required(ErrorMessage = "Favor informar o nome!")]
    [MinLength(3, ErrorMessage = "Favor informar pelo menos {1} caracteres!")]
    public string Nome
    {
        get => _nome;
        
        set
        {
            if (_nome != value)
            {
                OnPropertyChanging(nameof(Nome));
                _nome = value;
                OnPropertyChanged(nameof(Nome));
            }
        }
    }

    [Required(ErrorMessage = "Favor informar o e-mail!")]
    [EmailAddress(ErrorMessage = "Favor informar um e-mail válido!")]
    public string Email
    {
        get => _email;
        
        set
        {
            if (_email != value)
            {
                OnPropertyChanging(nameof(Email));
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }
}