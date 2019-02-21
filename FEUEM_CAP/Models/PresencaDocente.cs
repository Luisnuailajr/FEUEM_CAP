using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FEUEM_CAP.Models
{
    public class PresencaDocente
    {
        [Key]
        public int IdPresenca { get; set; }
        public Docente Docente { get; set; }
        public int DocenteId { get; set; }
        [Required(ErrorMessage = "Por favor escolha o nome do docente")]
        [Display(Name = "Nome do docente")]
        public virtual IEnumerable<Docente> Docentes { get; set; }
        [Display(Name = "Número de horas")]
        public byte? NumeroHoras { get; set; }
        public DateTime DataPrecensa { get; set; }
        public DateTime DataDaPresenca()
        {
            var data = DateTime.Today;
            return data;
        }
    }
}