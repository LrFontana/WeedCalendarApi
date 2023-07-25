using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Especificaciones;

namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface IRepositorioGlobal<T> where T : class
    {
        // metodo que retorna una lista.
        Task<IEnumerable<T>> ObtenerTodos(

            // recibe una expresion boleana y la usa de filtro.
            Expression<Func<T, bool>> filto = null, 
            // ordena la lista o lo que se necesite ordenar.
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            // incluye lo recibido en la query. 
            string incluirPropiedades = null
        ); 

        // metodo que retorna una lista.
        Task<PagedList<T>> ObtenerTodosPaginado(

            // recibe la paginacion.
            Parametros parametros,
            // recibe una expresion boleana y la usa de filtro.
            Expression<Func<T, bool>> filto = null, 
            // ordena la lista o lo que se necesite ordenar.
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            // incluye lo recibido en la query. 
            string incluirPropiedades = null
        ); 

        // metodo que retorna un solo registro.
        Task<T> Obtenerprimero(

            // recibe una expresion boleana y la usa de filtro.
            Expression<Func<T, bool>> filto = null,             
            // incluye lo recibido en la query. 
            string incluirPropiedades = null
        );

        // metodo que agrega un registro a la base de datos dependiendo la entidad que reciba ej: "Empleado" o "Compañia".
        Task Agregar(T entidad);

        //metodo para remover un registro
        void Remover(T entidad);

        //el metodo update no lo uso de forma generica por que es un metodo que necesita
        // actualizar cada una de las propiedades por separado.
    }
}