using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;

namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface IPlantasRepositorio : IRepositorioGlobal<Plantas>
    {
        //Metodos.
        void Actualizar(Plantas plantas);
    }
}