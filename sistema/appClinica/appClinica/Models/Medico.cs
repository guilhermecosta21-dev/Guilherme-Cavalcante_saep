using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace appClinica.Models
{
    public class Medico
    {
        [Key]
        public int Codigo { get; set; }
        
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CRMV é obrigatório.")]
        [StringLength(20)]
        [Display(Name = "CRMV")]
        public string Crmv { get; set; } = string.Empty;

        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        [StringLength(50)]
        public string Especialidade { get; set; } = string.Empty;

        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    }
}