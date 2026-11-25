using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace appClinica.Models
{
    public class Pet
    {
        [Key]
        public int Codigo { get; set; }
        
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A espécie é obrigatória.")]
        [StringLength(50)]
        public string Especie { get; set; } = string.Empty;

        [Required(ErrorMessage = "A raça é obrigatória.")]
        [StringLength(50)]
        public string Raca { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Selecione o tutor.")]
        [Display(Name = "Tutor")]
        public int TutorId { get; set; }

        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        public virtual Tutor? Tutor { get; set; }
    }
}