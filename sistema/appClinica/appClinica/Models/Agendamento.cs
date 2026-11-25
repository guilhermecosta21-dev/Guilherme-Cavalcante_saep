using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace appClinica.Models
{
    public class Agendamento
    {
        [Key]
        public int Codigo { get; set; }
        
        [Required(ErrorMessage = "A data do agendamento é obrigatória.")]
        [Display(Name = "Data do Agendamento")]
        public DateTime DataAgendamento { get; set; }

        [Required(ErrorMessage = "Selecione o tutor.")]
        [Display(Name = "Tutor")]
        public int TutorId { get; set; }

        [Required(ErrorMessage = "Selecione o pet.")]
        [Display(Name = "Pet")]
        public int PetId { get; set; }
        
        [Required(ErrorMessage = "Selecione o médico.")]
        [Display(Name = "Médico Veterinário")]
        public int MedicoId { get; set; }

        public virtual Medico? Medico { get; set; }

        public virtual Pet? Pet { get; set; }

        public virtual Tutor? Tutor { get; set; }
    }
}