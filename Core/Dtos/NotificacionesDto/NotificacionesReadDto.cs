using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dtos.NotificacionesDto
{
    public class NotificacionesReadDto
    {
        //Propiedades.               
        public string TipoDeNotificacion { get; set; }              
        public string Mensaje { get; set; }                     
        public DateTime FechaHora { get; set; }
        public string usuarioNombre { get; set; }
        public string plantasNombre { get; set; }
        public string sistemaRiegoNombre { get; set; }              
    }
}