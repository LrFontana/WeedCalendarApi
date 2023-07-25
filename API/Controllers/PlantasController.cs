using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos.PlatasDto;
using Core.Dtos.ResponseDto;
using Core.Especificaciones;
using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlantasController : ControllerBase
    {
        //variables.        
        private readonly IUnidadDeTrabajo _unidadTrabajo;
        private ResponsePaginatorDto _responsePaginador;
        private readonly IMapper _mapper;
        private readonly ILogger<PlantasController> _logger;

        //Constructor.
        public PlantasController(IUnidadDeTrabajo unidadTrabajo, ILogger<PlantasController> logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;            
            _responsePaginador = new ResponsePaginatorDto();
        }

        //Metodos.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Plantas>>> GetPlantas([FromQuery] Parametros parametros)
        {
            
            _logger.LogInformation("Listado de Plantas");

            //Query.
            var lista = await _unidadTrabajo.Plantas.ObtenerTodosPaginado(parametros, 
                                                                          incluirPropiedades:"Usuario",
                                                                          orderBy: p => p.OrderBy(p=>p.NombrePlanta).ThenBy(p=>p.FechaSiembre));// retorna la lista de plantas y tambien cluye todos los datos del usuario.
            

            //mapeo.
            _responsePaginador.TotalPaginas = lista.MetaData.TotalPages;
            _responsePaginador.TotalRegistros = lista.MetaData.TotalCount;
            _responsePaginador.PageSize = lista.MetaData.PageSize;
            _responsePaginador.Resultado = _mapper.Map<IEnumerable<Plantas>, IEnumerable<PlantasReadDto>>(lista);         
            _responsePaginador.StatusCode = HttpStatusCode.OK;
            _responsePaginador.Mensaje = "Listado de Plantas.";
            

            //Devuelve una lista de tipo plantas.
            return Ok(_responsePaginador); //El Ok() me garantiza que el status code que devuelve sea igual a 200
        }

        
        [HttpGet("{id}", Name = "GetPlantasPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Plantas>> GetPlantasPorId(int id)
        {
            
            //Verifica si el id que recibe es diferente a 0.
            if(id == 0){
                _logger.LogError("Debe de Enviar el ID ");
                _responsePaginador.Mensaje = "Debe de Enviar el ID";
                _responsePaginador.IsExitoso = false;
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }

            //Query.
            var plan = await _unidadTrabajo.Plantas.Obtenerprimero(p => p.Id == id, incluirPropiedades: "Usuario"); 

            //Verifica si existe ese id en la tabla
            if(plan==null){
                _logger.LogError("No existe ese ID");
                _responsePaginador.Mensaje ="No existe ese ID";
                _responsePaginador.IsExitoso = false;
                _responsePaginador.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_responsePaginador);
            }           


            //mapeo. 
            _responsePaginador.Resultado = _mapper.Map<Plantas, PlantasReadDto>(plan);
            //si todo sale bien.
            _responsePaginador.Mensaje = "Datos de la Planta." + plan.Id;   
            _responsePaginador.IsExitoso= true;     
            _responsePaginador.StatusCode = HttpStatusCode.OK; 

            //Devuelve una lista de tipo planta por id.
            return Ok(_responsePaginador); //El Ok() me garantiza que el status code que devuelve sea igual a 200
        }
        

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<ActionResult<Plantas>> PostPlantas([FromBody] PlantasUpsertDto plantasDto){

            //Verifica si lo recibido por input no es nulo.
            if(plantasDto == null){
                _responsePaginador.Mensaje = "La informacion ingresada es incorrecta";
                _responsePaginador.IsExitoso=false;
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }

            //Verifica que cada una de las propiedades esten completas.
            //trabaja directamente con el modelo.
            if(!ModelState.IsValid){

                 _responsePaginador.Mensaje = "La informacion ingresada es incorrecta";
                _responsePaginador.IsExitoso=false;
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }
            
            //Query.
            var plantaExiste = await _unidadTrabajo.Plantas.Obtenerprimero(p => p.NombrePlanta.ToLower() == plantasDto.NombrePlanta.ToLower() && 
                                                                           p.FechaSiembre.ToShortDateString() == plantasDto.FechaSiembre.ToShortDateString());

            //Verifica si el nombre ya existe.
            if(plantaExiste != null){                
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El Nombre del Usuario ya Existe.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }
            
            //relaciona el objeto que contiene la clase usuarioDto y lo adapta para que se pueda grabar en la base de datos correctamente.
            //variable.
            Plantas planta = _mapper.Map<Plantas>(plantasDto);

            //Agrega un nuevo regristo en la tabla Plantas.
            await _unidadTrabajo.Plantas.Agregar(planta);
            //Graba el nuevo registro en la tabla.
            await _unidadTrabajo.Guardar();   

            _responsePaginador.IsExitoso = true;
            _responsePaginador.Mensaje = "Planta Guardada con EXITO!";
            _responsePaginador.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetPlantasPorId", new {id=plantasDto}, _responsePaginador); //El CreatedAtRoute() me garantiza que el status code que devuelve sea igual a 201 por ser un metodo de tipo POST.   
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Plantas>> PutPlantas(int id, [FromBody] PlantasUpsertDto plantasDto){

            //Verifica si el id que recibe del end point es igual al id que tiene compania.
            if(id != plantasDto.Id)
            {
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El ID ingresado no es correcto.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }

            //Verifica si todos los datos del objeto son validos.
            if(!ModelState.IsValid)
            {   
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El Nombre de la Planta ya Existe.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador); 
            }

            //Verifica si el nombre existe.
            //Query.
            var plantaExiste = await _unidadTrabajo.Plantas.Obtenerprimero(p => p.NombrePlanta.ToLower() == plantasDto.NombrePlanta.ToLower()
                                                                              && p.Id != plantasDto.Id);

            if(plantasDto != null){
                
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El nombre de la Planta ya existe.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }            

            //relaciona el objeto que contiene la clase plantasDto y lo adapta para que se pueda grabar en la base de datos correctamente.
            //variable.
            Plantas planta = _mapper.Map<Plantas>(plantasDto);

            //actualiza los datos.
            _unidadTrabajo.Plantas.Actualizar(planta);

            //graba los datos.
            await _unidadTrabajo.Guardar();

            _responsePaginador.IsExitoso = true;
            _responsePaginador.Mensaje = "Planta Actualizada";
            _responsePaginador.StatusCode = HttpStatusCode.OK;

            //devuelve una lista de tipo plantas.
            return Ok(_responsePaginador);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePlantas(int id){

            //Query.(consulta en la base de datos si existe ese id)
            var planta = await _unidadTrabajo.Plantas.Obtenerprimero(p => p.Id == id);
            
            //Si no encontro ningun registro retontar "No Encontrado".
            if(planta == null){

                _responsePaginador.IsExitoso = false; 
                _responsePaginador.Mensaje = "Planta No Encontrado";
                _responsePaginador.StatusCode = HttpStatusCode.NotFound;
                return NotFound();
            }

            //Si encuentra uno lo elimina
            _unidadTrabajo.Plantas.Remover(planta);

            //Actualiza la base de datos.
            await _unidadTrabajo.Guardar();
            _responsePaginador.IsExitoso = true;
            _responsePaginador.Mensaje = "Planta Eliminada";
            _responsePaginador.StatusCode = HttpStatusCode.NoContent;

            return Ok(_responsePaginador);

        }
       
    }
}