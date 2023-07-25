using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos.ResponseDto;
using Core.Dtos.SistemaRiegoDto;
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
    public class SistemaRiegoController : ControllerBase
    {                
        //variables.        
        private readonly IUnidadDeTrabajo _unidadTrabajo;
        private ResponsePaginatorDto _responsePaginador;
        private readonly IMapper _mapper;
        private readonly ILogger<SistemaRiegoController> _logger;
        
        //Constructor.
        public SistemaRiegoController(IUnidadDeTrabajo unidadTrabajo, ILogger<SistemaRiegoController> logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;            
            _responsePaginador = new ResponsePaginatorDto();
        }        

        //Metodos.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SistemaRiego>>> GetSistemaRiego([FromQuery] Parametros parametros)
        {
            
            _logger.LogInformation("Listado de Sistema Riego");

            //Query.
            var lista = await _unidadTrabajo.SistemaRiego.ObtenerTodosPaginado(
                                                                          parametros, 
                                                                          incluirPropiedades:"Usuario, Plantas",
                                                                          orderBy: sR => sR.OrderBy(sR=>sR.TipoSistema).ThenBy(sR=>sR.Descripcion)); // retorna la lista de empleados y tambien cluye todos los datos de la compañia.
            
            //mapeo.
            _responsePaginador.TotalPaginas = lista.MetaData.TotalPages;
            _responsePaginador.TotalRegistros = lista.MetaData.TotalCount;
            _responsePaginador.PageSize = lista.MetaData.PageSize;
            _responsePaginador.Resultado = _mapper.Map<IEnumerable<SistemaRiego>, IEnumerable<SistemaRiegoReadDto>>(lista);         
            _responsePaginador.StatusCode = HttpStatusCode.OK;
            _responsePaginador.Mensaje = "Listado de sistema Riego.";
            

            //Devuelve una lista de tipo sistema riego.
            return Ok(_responsePaginador); //El Ok() me garantiza que el status code que devuelve sea igual a 200
        }


        [HttpGet("{id}", Name = "GetSistemaRiegoPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SistemaRiego>> GetSistemaRiegoPorId(int id){
            
            //Verifica si el id que recibe es igual a 0.
            if(id == 0){
                _logger.LogError("Debe de Enviar el ID ");
                _responsePaginador.Mensaje = "Debe de Enviar el ID";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                _responsePaginador.IsExitoso = false;                
                return BadRequest(_responsePaginador);
            }

            //Query.            
            var sisRiego = await _unidadTrabajo.SistemaRiego.Obtenerprimero(sR => sR.Id == id); // retorna la lista de sistema riego 

            //Verifica si existe ese id en la tabla
            if(sisRiego==null){
                _logger.LogError("No existe ese ID");
                _responsePaginador.Mensaje ="No existe ese ID";
                _responsePaginador.IsExitoso = false;
                _responsePaginador.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_responsePaginador);
            }          

            //mapeo. 
            _responsePaginador.Resultado = _mapper.Map<SistemaRiego, SistemaRiegoReadDto>(sisRiego);
            //si todo sale bien.
            _responsePaginador.Mensaje = "Datos del Sistema Riego." + sisRiego.Id;   
            _responsePaginador.IsExitoso= true;     
            _responsePaginador.StatusCode = HttpStatusCode.OK;    

            //Devuelve una lista de tipo sistema riego por id.
            return Ok(_responsePaginador); //El Ok() me garantiza que el status code que devuelve sea igual a 200
        }

        [HttpGet]
        [Route("SistemaRiegoPorUsuario/{UsuarioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        public async Task<ActionResult<IEnumerable<SistemaRiegoReadDto>>> GetSistemaRiegoPorUsuario(int usuarioId)
        { 
            _logger.LogInformation("Listado de sistema Riego Por Usuario");   

            //Query.            
            var sRLista = await _unidadTrabajo.SistemaRiego.ObtenerTodos(sR=>sR.Id == usuarioId, incluirPropiedades: "Usuario");// retorna una lista de sistema riego filtadas por el parametro id.
            
            //mapeo.
            _responsePaginador.Resultado = _mapper.Map<IEnumerable<SistemaRiego>, IEnumerable<SistemaRiegoReadDto>>(sRLista);
            //si todo sale bien.
            _responsePaginador.IsExitoso = true;
            _responsePaginador.StatusCode = HttpStatusCode.OK;
            _responsePaginador.Mensaje = "Listado de sistema Riego por Usuario.";            
            return Ok(_responsePaginador);
        }

        [HttpGet]
        [Route("SistemaRiegoPorPlantas/{plantasId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        public async Task<ActionResult<IEnumerable<SistemaRiegoReadDto>>> GetSistemaRiegoPorPlanta(int plantasId)
        { 
            _logger.LogInformation("Listado de sistema Riego Por Planta");   

            //Query.            
            var sRLista = await _unidadTrabajo.SistemaRiego.ObtenerTodos(sR=>sR.PlantasId == plantasId, incluirPropiedades: "Plantas");// retorna una lista de sistema riego filtadas por el parametro id.
            
            //mapeo.
            _responsePaginador.Resultado = _mapper.Map<IEnumerable<SistemaRiego>, IEnumerable<SistemaRiegoReadDto>>(sRLista);
            //si todo sale bien.
            _responsePaginador.IsExitoso = true;
            _responsePaginador.StatusCode = HttpStatusCode.OK;
            _responsePaginador.Mensaje = "Listado de sistema Riego por Plantas.";            
            return Ok(_responsePaginador);
        }
        

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<ActionResult<SistemaRiego>> PostSistemaRiego([FromBody] SistemaRiegoUpsertDto sistemaRiegoDto){

            //Verifica si lo recibido por input no es nulo.
            if(sistemaRiegoDto == null){
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
            var sistemaRiegoExiste = await _unidadTrabajo.SistemaRiego.Obtenerprimero(sR => sR.TipoSistema.ToLower() == sistemaRiegoDto.TipoSistema.ToLower() &&
                                                                              sR.Descripcion.ToLower() == sistemaRiegoDto.Descripcion.ToLower());

            //Verifica si el nombre ya existe.
            if(sistemaRiegoExiste != null){
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El Nombre del Sistema Riego ya Existe.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }
            
            //relaciona el objeto que contiene la clase sistemaRiegoDto y lo adapta para que se pueda grabar en la base de datos correctamente.
            //mapeo.
            SistemaRiego sistemaRiego = _mapper.Map<SistemaRiego>(sistemaRiegoDto);

            //Agrega un nuevo regristo en la tabla Sistema Riego.
            await _unidadTrabajo.SistemaRiego.Agregar(sistemaRiego);
            //Graba el nuevo registro en la tabla.
            await _unidadTrabajo.Guardar(); 

            _responsePaginador.IsExitoso = true;
            _responsePaginador.Mensaje = "Sistema Riego Actualizado con EXITO!";
            _responsePaginador.StatusCode = HttpStatusCode.Created;
            _responsePaginador.Resultado = sistemaRiego;
            return CreatedAtRoute("GetSistemaRiegoPorId", new {id=sistemaRiegoDto}, _responsePaginador); //El CreatedAtRoute() me garantiza que el status code que devuelve sea igual a 201 por ser un metodo de tipo POST.   
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SistemaRiego>> PutSistemaRiego(int id, [FromBody] SistemaRiegoUpsertDto sistemaRiegoDto){

            //Verifica si el id que recibe del end point es igual al id que tiene compania.
            if(id != sistemaRiegoDto.Id){

                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El ID ingresado no es correcto.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }

            //Verifica si todos los datos del objeto son validos.
            if(!ModelState.IsValid)
            {   
                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El Nombre del sistema Riego ya Existe.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }
            
            //Query.
            var sistemaRiegoExiste = await _unidadTrabajo.SistemaRiego.Obtenerprimero(sR => sR.TipoSistema.ToLower() == sistemaRiegoDto.TipoSistema.ToLower()
                                                                             && sR.Descripcion.ToLower() == sistemaRiegoDto.Descripcion.ToLower()
                                                                             && sR.Id != sistemaRiegoDto.Id);

            //Verifica si el nombre, apellido y id ya existe.
            if(sistemaRiegoExiste != null){

                _responsePaginador.IsExitoso = false;
                _responsePaginador.Mensaje = "El nombre del Sistema Riego ya existe.";
                _responsePaginador.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responsePaginador);
            }            

            //relaciona el objeto que contiene la clase sistemaRiegoUpsertDtop y lo adapta para que se pueda grabar en la base de datos correctamente.
            //mapeo.
            SistemaRiego sistemaRiego = _mapper.Map<SistemaRiego>(sistemaRiegoDto);

            //actualiza los datos.
            _unidadTrabajo.SistemaRiego.Actualizar(sistemaRiego);

            //graba los datos.
            await _unidadTrabajo.Guardar();
            _responsePaginador.IsExitoso = true;
            _responsePaginador.Mensaje = "Sistema Riego Guardado con EXITO";
            _responsePaginador.StatusCode = HttpStatusCode.NoContent;

            //devuelve una lista de tipo sistema riego.
            return Ok(_responsePaginador);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletesistemaRiego(int id){

            //Query.(consulta en la base de datos si existe ese id)
            var sistemaRiego = await _unidadTrabajo.SistemaRiego.Obtenerprimero(sR=> sR.Id == id);
            
            //Si no encontro ningun registro retontar "No Encontrado".
            if(sistemaRiego == null){

                _responsePaginador.IsExitoso = false; 
                _responsePaginador.Mensaje = "Sistema Riego No Encontrado";
                _responsePaginador.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_responsePaginador);
            }

            //Si encuentra uno lo elimina
           _unidadTrabajo.SistemaRiego.Remover(sistemaRiego);

            //Actualiza la base de datos.
            await _unidadTrabajo.Guardar();
            _responsePaginador.IsExitoso = true;
            _responsePaginador.Mensaje = "sistema Riego Eliminado";
            _responsePaginador.StatusCode = HttpStatusCode.NoContent;

            return Ok(_responsePaginador);
        }
    }
}