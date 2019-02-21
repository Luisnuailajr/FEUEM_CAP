using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEUEM_CAP.Models
{
    public class Departamento
    {
        [Key]
        [Required]
        public int DepartamentoId { get; set; }
        [Required(ErrorMessage = "Por favor informe o nome do departamento")]
        [DataType(DataType.Text)]
        [Display(Name = "Nome do departamento")]
        public string NomeDepartamento { get; set; }
        [Required(ErrorMessage = "por favor informe a sigla")]
        [DataType(DataType.Text)]
        [Display(Name = "Sigla")]
        public string Sigla { get; set; }
        public virtual IEnumerable<Curso> Cursos { get; set; }
    }
}