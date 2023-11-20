using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestauranteService.Data;
using RestauranteService.Dtos;
using RestauranteService.ItemServiceHttpClient;
using RestauranteService.Models;

namespace RestauranteService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RestauranteController : ControllerBase
    {
        private readonly IRestauranteRepository _restauranteRepository;
        private readonly IMapper _mapper;
        private IItemServiceHttpClient _itemServiceHttpClient;

        public RestauranteController(
            IRestauranteRepository restauranteRepository,
            IMapper mapper,
            IItemServiceHttpClient itemServiceHttpClient)
        {
            _restauranteRepository = restauranteRepository;
            _mapper = mapper;
            _itemServiceHttpClient = itemServiceHttpClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestauranteReadDto>> GetAllRestaurantes() 
        { 
            var restaurantes = _restauranteRepository.GetAllRestaurantes();
            return Ok(_mapper.Map<IEnumerable<RestauranteReadDto>>(restaurantes));
        }

        [HttpGet("{id}", Name = "GetRestauranteById")]
        public ActionResult<RestauranteReadDto> GetRestauranteById(int id)
        {
            var restaurante = _restauranteRepository.GetRestauranteById(id); 
            
            if (restaurante != null)
            {
                return Ok(restaurante);
            }
            
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<RestauranteReadDto>> CreateRestaurante(RestauranteCreateDto createDto)
        {
            var restaurante = _mapper.Map<Restaurante>(createDto);

            _restauranteRepository.CreateRestaurante(restaurante);
            _restauranteRepository.SaveChanges();

            var restauranteDto = _mapper.Map<RestauranteReadDto>(restaurante);

            _itemServiceHttpClient.EnviaRestauranteParaItemService(restauranteDto);

            return CreatedAtRoute(nameof(GetRestauranteById), new {restauranteDto.Id}, restauranteDto);
        }
    }
}
