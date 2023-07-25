using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;

namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface ISistemaRiegoRepositorio : IRepositorioGlobal<SistemaRiego>
    {
        //Metodos.
        void Actualizar(SistemaRiego sistemaRiego);
        List<DateTime> DiasDeRiego(DateTime fecha, int diasRiego);
    }
}