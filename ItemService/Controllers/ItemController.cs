using AutoMapper;
using ItemService.Data;
using ItemService.Dtos;
using ItemService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItemService.Controllers;

[Route("api/item/restaurante/{restauranteId}/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;

    public ItemController(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ItemReadDto>> GetItensForRestaurantes(int restauranteId)
    {
        if (!_itemRepository.RestauranteExiste(restauranteId))
        {
            return NotFound();
        }

        var itens = _itemRepository.GetItensDeRestaurante(restauranteId);
        return Ok(_mapper.Map<IEnumerable<ItemReadDto>>(itens));
    }

    [HttpGet("{itemId}", Name = "GetItemForRestaurante")]
    public ActionResult<ItemReadDto> GetItemForRestaurante(int restauranteId, int itemId)
    {
        if (!_itemRepository.RestauranteExiste(restauranteId))
        {
            return NotFound();
        }

        var item = _itemRepository.GetItem(restauranteId, itemId);
        if (item == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ItemReadDto>(item));
    }

    [HttpPost]
    public ActionResult<ItemReadDto> CreateItemForRestaurante(int restauranteId, ItemCreateDto itemDto)
    {
        if (!_itemRepository.RestauranteExiste(restauranteId))
        {
            return NotFound();
        }

        var item = _mapper.Map<Item>(itemDto);
        _itemRepository.CreateItem(restauranteId, item);
        _itemRepository.SaveChanges();

        var itemReadDto = _mapper.Map<ItemReadDto>(item);

        return CreatedAtRoute(nameof(GetItemForRestaurante),
            new { restauranteId, ItemId = itemReadDto.Id }, itemReadDto);

    }
}
