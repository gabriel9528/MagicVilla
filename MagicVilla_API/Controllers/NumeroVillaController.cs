using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase
    {

        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numeroRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo, INumeroVillaRepositorio numeroRepo,IMapper mapper)
        {
            _logger = logger;
            _villaRepo= villaRepo;
            _numeroRepo= numeroRepo;
            _mapper = mapper;
            _response = new();
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                _logger.LogInformation("Obtener numeros de villas");

                IEnumerable<NumeroVilla> numeroVillalist = await _numeroRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillalist);
                _response.statusCode = HttpStatusCode.OK;


                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsExitoso= false;
                _response.ErrorMessages = new List<string>()
                {
                    e.ToString()
                };
            }

            return _response;
        }

        [HttpGet("id:int", Name = "GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer numero villa con Id" + id);
                    _response.statusCode=HttpStatusCode.BadRequest;
                    _response.IsExitoso= false;
                    return BadRequest(_response);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

                var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);

                if (numeroVilla == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso= false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<NumeroVillaDto>(numeroVilla);
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsExitoso= false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }

            return _response;

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _numeroRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NumeroExiste", "El numero de la villa ya existe");
                    return BadRequest(ModelState);
                }

                if(await _villaRepo.Obtener(v=>v.Id == createDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El id de la villa no existe");
                    return BadRequest(ModelState);
                }

                if(createDto == null)
                {
                    return BadRequest(createDto);
                }


                //villaDto.Id = _db.Villas.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
                //VillaStore.villaList.Add(villaDto);

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                //Villa modelo = new()
                //{
                //    //Id = villaDto.Id,
                //    Nombre = createDto.Nombre,
                //    Detalle = createDto.Detalle,
                //    ImagenUrl = createDto.ImagenUrl,
                //    Ocupantes = createDto.Ocupantes,
                //    Tarifa = createDto.Tarifa,
                //    MetrosCuadrados = createDto.MetrosCuadrados,
                //    Amenidad = createDto.Amenidad
                //};

                await _numeroRepo.Crear(modelo);
                _response.Resultado= modelo;
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetNumeroVilla", new { id = modelo.VillaNo }, _response);
            }
            catch (Exception e)
            {
                _response.IsExitoso= false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso= false;
                    _response.statusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var numeroVilla = await _numeroRepo.Obtener(villa => villa.VillaNo == id);
                if (numeroVilla == null)
                {
                    _response.IsExitoso= false;
                    _response.statusCode=HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _numeroRepo.Remover(numeroVilla);
                _response.statusCode= HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsExitoso= false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }

            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto) 
        {
            if(updateDto == null || id != updateDto.VillaNo)
            {
                _response.IsExitoso= false;
                _response.statusCode=HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if( await _villaRepo.Obtener(v =>v.Id == updateDto.VillaId) == null)
            {
                ModelState.AddModelError("ClaveForanea", "El Id de la villa no existe");
                return BadRequest(ModelState);

            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);
            
            //Villa modelo = new()
            //{
            //    Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    ImageUrl = villaDto.ImagenUrl,
            //    Ocupantes = villaDto.Ocupantes,
            //    Tarifa = villaDto.Tarifa,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //    Amenidad = villaDto.Amenidad
            //};

            await _numeroRepo.Actualizar(modelo);
            _response.statusCode=HttpStatusCode.NoContent;

            return Ok(_response);
        }

        //[HttpPatch("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        //public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        //{
        //    if(patchDto == null || id == 0)
        //    {
        //        return BadRequest();
        //    }

        //    var villa = await _villaRepo.Obtener(v => v.Id==id, tracked:false);


        //    VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
            
        //    //VillaUpdateDto villaDto = new()
        //    //{
        //    //    Id = villa.Id,
        //    //    Nombre = villa.Nombre,
        //    //    Detalle = villa.Detalle,
        //    //    ImagenUrl = villa.ImageUrl,
        //    //    Ocupantes = villa.Ocupantes,
        //    //    Tarifa = villa.Tarifa,
        //    //    MetrosCuadrados = villa.MetrosCuadrados,
        //    //    Amenidad = villa.Amenidad
        //    //};

        //    if(villa == null) return BadRequest();

        //    patchDto.ApplyTo(villaDto, ModelState);

        //    if(!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }


        //    Villa modelo = _mapper.Map<Villa>(villaDto);
            
        //    //Villa modelo = new()
        //    //{
        //    //    Id = villaDto.Id,
        //    //    Nombre = villaDto.Nombre,
        //    //    Detalle = villaDto.Detalle,
        //    //    ImageUrl = villaDto.ImagenUrl,
        //    //    Ocupantes = villaDto.Ocupantes,
        //    //    Tarifa = villaDto.Tarifa,
        //    //    MetrosCuadrados = villaDto.MetrosCuadrados,
        //    //    Amenidad = villaDto.Amenidad
        //    //};

        //    await _villaRepo.Actualizar(modelo);
        //    _response.statusCode=HttpStatusCode.NoContent;

        //    return Ok(_response);
        //}
    }
}
