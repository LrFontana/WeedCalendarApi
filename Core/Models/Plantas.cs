using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Plantas
    {
        //Propiedades.
        
        [Key]       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]  
        public int Id { get; set; } 

        [Required(ErrorMessage = "El Nombre de la Plata es Requrido.")]
        [MaxLength(30, ErrorMessage = "El Nombre de la Plata no puede ser mayaor a 30 caracteres.")]        
        public string NombrePlanta { get; set; }

        [Required(ErrorMessage = "La Variedad de la Plata es Requerida.")]
        [MaxLength(15, ErrorMessage = "La Variedad de la Plata no puede ser mayaor a 15 caracteres.")] 
        public string VariedadPlanta { get; set; }
        public string EstapaCrecimento { get; set; }        
        public string Notas { get; set; }

        [Required(ErrorMessage = "La Fecha de Siembra es Requerida.")]        
        public DateTime FechaSiembre { get; set; }

        [Required(ErrorMessage = "La Fecha Aproxima es Requerida.")]
        public DateTime FechaCosecha { get; set; }

        [Required(ErrorMessage = "El Id Usuario  es Requrido.")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")] 
        public Usuario Usuario { get; set; }
    }
}