using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEUEM_CAP.Models
{
    public class NivelAcademico
    {
        [Key]
        [Required]
        public int NivelAcademicoId { get; set; }
        [Required(ErrorMessage = "Por favor informe o nivel academico")]
        [DataType(DataType.Text)]
        [Display(Name = "Nivel académico")]
        public string DescricaoNivelAcademico { get; set; }
        public virtual IEnumerable<Docente> Docentes { get; set; }
    }
}