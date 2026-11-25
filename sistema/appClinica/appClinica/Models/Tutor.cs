using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace appClinica.Models
{
    public class Tutor
    {
        [Key]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "CPF")]
        [StringLength(64)]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(20)]
        public string Telefone { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
    }
}