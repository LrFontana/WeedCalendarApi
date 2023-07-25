using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class SistemaRiego
    {
        //Propiedades.

        [Key]       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]  
        public int Id { get; set; } 

        [Required(ErrorMessage = "La Cantidad de Agua es Requerida")]        
        public int CantidadAgua { get; set; }

        [Required(ErrorMessage = "La Descripcion es Requerida")]
        [MaxLength(300, ErrorMessage = "La Descripcion no puede superar los 300 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El Tipo de Sistema De Riego es Requerido")]
        [MaxLength(30, ErrorMessage = "El Tipo de Sistema De Riego no puede exceder los 30 caracteres.")]
        public string TipoSistema { get; set; }

        [Required(ErrorMessage = "La Proporcion de los Nutrientes es Requerida")]
        [MaxLength(30, ErrorMessage = "La Proporcion de los Nutrientes no puede exceder los 30 caracteres.")]
        public string ProporcionNutrientes { get; set; }

        [Required(ErrorMessage = "El Id Usuario  es Requrido.")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")] 
        public Usuario Usuario { get; set; }
        
        [Required(ErrorMessage = "El Id Planta es Requrido.")]
        public int PlantasId { get; set; }

        [ForeignKey("PlantasId")] 
        public Plantas Plantas { get; set; }
    }
}