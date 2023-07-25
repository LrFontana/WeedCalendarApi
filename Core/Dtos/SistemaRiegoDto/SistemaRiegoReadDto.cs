using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dtos.SistemaRiegoDto
{
    public class SistemaRiegoReadDto
    {
        //Propiedades.
              
        public int CantidadAgua { get; set; }        
        public string Descripcion { get; set; }        
        public string TipoSistema { get; set; }        
        public string ProporcionNutrientes { get; set; }
        public string usuarioNombre { get; set; }
        public string plantasNombre { get; set; }   
    }
}