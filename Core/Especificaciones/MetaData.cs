using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Especificaciones
{
    public class MetaData
    {
        //Propiedades.
        public int CurrentPage { get; set; } // pagina actual.
        public int TotalPages { get; set; } // total de las paginas (depende del total de registros y del tamaño de la pagina)
        public int PageSize { get; set; } // tamaño de la pagina.
        public int TotalCount { get; set; } // el total de registros de la tabla a la que le hace la consulta.

        public bool HasPrevious => CurrentPage > 1; // controla si hay o no paginas previas.
        public bool HasNext => CurrentPage < TotalPages; // controla/indica si hay paginas siguientes.
    }
}