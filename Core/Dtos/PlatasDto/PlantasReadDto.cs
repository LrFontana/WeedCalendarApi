using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dtos.PlatasDto
{
    public class PlantasReadDto
    {
        //Propiedades.

        public string NombrePlanta { get; set; }        
        public string VariedadPlanta { get; set; }
        public string EstapaCrecimento { get; set; }        
        public string Notas { get; set; }    
        public string usuarioNombre { get; set; }         
        public DateTime FechaSiembre { get; set; }
        public DateTime FechaCosecha { get; set; }
    }
}