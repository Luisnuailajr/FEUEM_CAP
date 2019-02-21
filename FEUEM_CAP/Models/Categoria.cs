using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEUEM_CAP.Models
{
    public class Categoria
    {
        [Key]
        [Required]
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = "Por favor informe a categoria")]
        [DataType(DataType.Text)]
        [Display(Name = "Categoria")]
        public string DescricaoCategoria { get; set; }
        public virtual IEnumerable<Docente> Docentes { get; set; }
    }
}