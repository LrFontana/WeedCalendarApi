using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.Extensions.Configuration;

namespace Infraestructura.Data.Repositorio
{
    public class PlantasRepositorio : RepositorioGlobal<Plantas>, IPlantasRepositorio
    {

        //Variable.
        public ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public PlantasRepositorio(ApplicationDbContext db, IConfiguration configuration) : base(db)
        {
            _db = db;
            _configuration = configuration;
        }

        public void Actualizar(Plantas plantas)
        {
            //Query.
            var queryActualizarPlantas = _db.TblPlantas.FirstOrDefault(p => p.Id == plantas.Id);
            
            //Verifica si query es diferente a null, y si lo es actualiza los datos y guarda los cambios.
            if(queryActualizarPlantas != null)
            {
                queryActualizarPlantas.NombrePlanta = plantas.NombrePlanta;
                queryActualizarPlantas.EstapaCrecimento = plantas.EstapaCrecimento;
                queryActualizarPlantas.FechaCosecha = plantas.FechaCosecha;
                queryActualizarPlantas.FechaSiembre = plantas.FechaSiembre;
                queryActualizarPlantas.Notas = plantas.Notas;
                queryActualizarPlantas.VariedadPlanta = plantas.VariedadPlanta;
                queryActualizarPlantas.UsuarioId = plantas.UsuarioId;
                _db.SaveChanges();
            }
        }
    }
}