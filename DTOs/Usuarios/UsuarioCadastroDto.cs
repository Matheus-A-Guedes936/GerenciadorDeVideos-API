using System.ComponentModel.DataAnnotations;

namespace GerenciadorDeVideos_API.DTOs.Usuarios
{
    public class UsuarioCadastroDto
    {
        [Required(ErrorMessage ="Nome obrigatorio")]
        [MinLength(3, ErrorMessage ="O Nome De Usuario deve possuir ao menos 3 caracteres")]
        string Nome {  get; set; }

        [Required]
        [EmailAddress(ErrorMessage ="Email invalido")]
        string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage ="A senha deve possuir ao menos 6 caracteres")]
        string Senha { get; set; }

    }
}
