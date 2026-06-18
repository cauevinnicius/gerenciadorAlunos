using System.ComponentModel.DataAnnotations;

namespace GerenciadorAlunosV2.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirme sua senha")]
    // uso o atributo da senha com a data annotation de comparar
    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string ConfirmarSenha { get; set; }

    [Display(Name = "Lembrar-me?")]
    public bool LembrarMe { get; set; }
}