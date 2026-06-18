using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace GerenciadorAlunosV2.Models;

public class UsuarioModel : IdentityUser
{
    public string NomeCompleto { get; set; }
}