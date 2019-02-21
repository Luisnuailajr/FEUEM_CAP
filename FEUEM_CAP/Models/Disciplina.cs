using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEUEM_CAP.Models
{
    public class Disciplina
    {
        [Key]
        [Required]
        public int DisciplinaId { get; set; }
        [Required(ErrorMessage = "Por favor informe o nome da disciplina")]
        [Display(Name = "Nome da disciplina")]
        [DataType(DataType.Text)]
        public string NomeDisciplina { get; set; }
        [Required(ErrorMessage = "Por favor informe o ano")]
        public byte Ano { get; set; }
        [Required(ErrorMessage = "por favor informe o semestre")]
        public byte Semestre { get; set; }
        public int CursoId { get; set; }
        public virtual Curso Curso { get; set; }
       
    }
}