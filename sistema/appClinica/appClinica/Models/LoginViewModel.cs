using System.ComponentModel.DataAnnotations;

namespace appClinica.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [Display(Name = "CPF do Tutor")]
        public string Cpf { get; set; } = string.Empty;
    }
}