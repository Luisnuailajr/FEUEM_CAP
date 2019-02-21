using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FEUEM_CAP.Models
{
    public class Curso
    {
        [Key]
        public int CursoId { get; set; }
        [Required(ErrorMessage = "Por favor informe o nome do curso")]
        [Display(Name = "Nome do curso")]
        [DataType(DataType.Text)]
        public string NomeCurso { get; set; }
        [Required(ErrorMessage = "Por favor informe a duração do curso")]
        [Display(Name = "Duração do curso")]
        public byte Duracao { get; set; }
        [Required(ErrorMessage = "por favor informe o ano de criação do curso")]
        [Display(Name = "Ano de criação")]
        public int AnoCriacao { get; set; }
        [Display(Name = "Nome do departamento")]
        public Departamento Departamento { get; set; }
        public int DepartamentoId { get; set; }
        public virtual IEnumerable<Disciplina> Disciplinas { get; set; }
        
    }

   
}