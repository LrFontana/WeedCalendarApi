using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dtos.NotificacionesDto
{
    public class NotificacionesUpsertDto
    {
        //Propiedades.

        public int Id { get; set; } 

        [Required(ErrorMessage = "El Tipo de Notificacion es Requrido.")]
        [MaxLength(25, ErrorMessage = "El Tipo de Notificacion no puede ser mayaor a 25 caracteres.")]        
        public string TipoDeNotificacion { get; set; }

        [Required(ErrorMessage = "El Mensaje es Requrido.")]
        [MaxLength(50, ErrorMessage = "El Mensaje no puede ser mayaor a 50 caracteres.")]        
        public string Mensaje { get; set; }
                     
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "El Id Usuario  es Requrido.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El Id Planta es Requrido.")]
        public int PlantasId { get; set; }

        [Required(ErrorMessage = "El Id Sistema de Riego es Requrido.")]
        public int SistemaRiegoId { get; set; }
    }
}