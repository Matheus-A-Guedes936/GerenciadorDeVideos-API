using System.ComponentModel.DataAnnotations;

namespace GerenciadorDeVideos_API.DTOs.Usuarios
{
    public class UsuarioLoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email invalido")]
        string Email { get; set; }

        [Required(ErrorMessage = "Porfavor Digite Sua senha")]
        string Senha { get; set; }
    }
}
