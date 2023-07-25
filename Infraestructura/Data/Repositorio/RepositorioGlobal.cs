using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Especificaciones;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Data.Repositorio
{
    public class RepositorioGlobal<T> : IRepositorioGlobal<T> where T : class
    {
        //Variables.
        private readonly ApplicationDbContext _db;
        public DbSet<T> dbSet; // enlaza y hace el seteo de la T con la entidad que recibe ej "usuario", "plantas", "etc".

        
        public RepositorioGlobal(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad);
        }

        public async Task<T> Obtenerprimero(Expression<Func<T, bool>> filto = null, string incluirPropiedades = null)
        {
            //variabel
            IQueryable<T> query = dbSet;

            //Verifica si filtro no esta null y sino filtra por el parametro que ricibio.
            if(filto != null)
            {   
                query = query.Where(filto);
            }
            
            //Verifica si incluirPropiedades no esta null y sino incluye las propiedades por el parametro que ricibio.
            if(incluirPropiedades != null)
            {
                //recorre el arreglo, los separa por , y crea un nuevo array.
                foreach(var ip in incluirPropiedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(ip);
                }
            }            

            // Y si no retorna una lista del query.
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filto = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null)
        {
            //variabel
            IQueryable<T> query = dbSet;

            //Verifica si filtro no esta null y sino filtra por el parametro que ricibio.
            if(filto != null)
            {   
                query = query.Where(filto);
            }
            
            //Verifica si incluirPropiedades no esta null y sino incluye las propiedades por el parametro que ricibio.
            if(incluirPropiedades != null)
            {
                //recorre el arreglo, los separa por , y crea un nuevo array.
                foreach(var ip in incluirPropiedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(ip);
                }
            }

            //Verifica si orderBy no esta null y sino ordena por el parametro que ricibio.
            if(orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            // Y si no retorna una lista del query.
            return await query.ToListAsync();
        }

        public async Task<PagedList<T>> ObtenerTodosPaginado(Parametros parametros, Expression<Func<T, bool>> filto = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null)
        {
            //variabel
            IQueryable<T> query = dbSet;

            //Verifica si filtro no esta null y sino filtra por el parametro que ricibio.
            if(filto != null)
            {   
                query = query.Where(filto);
            }
            
            //Verifica si incluirPropiedades no esta null y sino incluye las propiedades por el parametro que ricibio.
            if(incluirPropiedades != null)
            {
                //recorre el arreglo, los separa por , y crea un nuevo array.
                foreach(var ip in incluirPropiedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(ip);
                }
            }

            //Verifica si orderBy no esta null y sino ordena por el parametro que ricibio.
            if(orderBy != null)
            {
                await orderBy(query).ToListAsync();
                return PagedList<T>.ToPageList(query, parametros.PageNumber, parametros.PageSize);
            }

            // Y si no retorna una lista del query.
            return PagedList<T>.ToPageList(query, parametros.PageNumber, parametros.PageSize);

        }

        public void Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }
    }
}