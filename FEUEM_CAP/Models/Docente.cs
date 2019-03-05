using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FEUEM_CAP.Models
{
    public class Docente
    {
        [Key]
        [Required]
        public int DocenteId { get; set; }
        [Required(ErrorMessage = "Por favor informe o nome")]
        [Display(Name = "Nome do docente")]
        [DataType(DataType.Text)]
        public string NomeDocente { get; set; }
        [Required(ErrorMessage = "Por favor informe o apelido")]
        [DataType(DataType.Text)]
        [Display(Name = "Apelido do docente")]
        public string ApelidoDocente { get; set; }
        [Required(ErrorMessage = "Por favor informe o telefone")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefone")]
        public int ContactoTelefone { get; set; }
        [Required(ErrorMessage = "Por favor informe o email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string ContactoEmail { get; set; }
        [Required(ErrorMessage = "Por favor informe o NUIT")]
        [Display(Name = "NUIT")]
        public double Nuit { get; set; }
        [Required(ErrorMessage = "Por favor informe o NIB")]
        [Display(Name = "NIB")]
        public double Nib { get; set; }
        [Required(ErrorMessage = "Informe o numero de conta")]
        [Display(Name = "Número de conta")]
        public double NumeroConta { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual NivelAcademico NivelAcademico { get; set; }
        public int CategoriaId { get; set; }
        public int NivelAcademicoId { get; set; }

    }
}