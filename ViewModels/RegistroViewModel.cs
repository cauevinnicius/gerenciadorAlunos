using System.ComponentModel.DataAnnotations;

namespace GerenciadorAlunosV2.ViewModels
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório!")]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "O email é obrigatório!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória!")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "A senha deve ter, no mínimo, {2} caracteres e, no máximo, {1} caracteres" )]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string Senha { get; set; }
        
        [Required(ErrorMessage = "Confirmar a senha é obrigatório!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova senha")]
        public string ConfirmarSenha { get; set; }       
    }
    
}