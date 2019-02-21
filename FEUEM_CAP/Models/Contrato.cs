using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEUEM_CAP.Models
{
    public class Contrato
    {
        [Key]
        public int ContratoId { get; set; }
        public Docente DocenteId { get; set; }
        [Required(ErrorMessage = "Por favor informe o Nº de disciplinas")]
        [Display(Name = "Nº de disciplinas")]
        public byte NumeDisciplinas { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DataContato { get; set; }
        public virtual Docente Docente { get; set; }
    }
}