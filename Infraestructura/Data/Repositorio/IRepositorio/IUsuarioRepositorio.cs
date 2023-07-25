using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;

namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio : IRepositorioGlobal<Usuario>
    {
        //Metodos.
        Task<int> Register(Usuario user, string password);
        Task<string> Login(string userName, string password);
        Task<bool> UserExist(string userName);
        void Actualizar(Usuario usuario);
    }
}