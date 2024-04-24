using System.ComponentModel.DataAnnotations;

namespace UsuariosApi.Data.Dtos.UsuarioDto
{
    public class LoginUsuarioDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
