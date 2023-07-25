using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Especificaciones
{
    public class PagedList<T>: List<T>
    {
        //Propiedades.
        public MetaData MetaData { get; set; }

        //Constructor.
        public PagedList(List<T> lista, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize) // el metodo ceiling obtioene el resultado en decimales de la division y lo redonde al n° mas cercano. ej: 1.5 a 2
            };
            AddRange(lista); // agrega los elementos de la coleccion al final de la lista.
        }

        //Metodo.
        public static PagedList<T> ToPageList(IEnumerable<T> entidad, int pageNumber, int pageSize)
        {
            //variable.
            var count = entidad.Count(); // obtione la cantidad exacta de registros que hay en esa entidad.
            var lista = entidad.Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize).ToList();

            return new PagedList<T>(lista, count, pageNumber, pageSize); 
        }
    }
}