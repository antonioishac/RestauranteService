using ItemService.Models;

namespace ItemService.Data
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateItem(int restauranteId, Item item)
        {
            item.Id = restauranteId;
            _context.Items.Add(item);
        }

        public void CreateRestaurante(Restaurante restaurante)
        {
            _context.Restaurants.Add(restaurante);
        }

        public bool ExisteRestauranteExterno(int restauranteIdExterno)
        {
            return _context.Restaurants.Any(restaurante => restaurante.IdExterno == restauranteIdExterno);
        }

        public IEnumerable<Restaurante> GetAllRestaurantes()
        {
            return _context.Restaurants.ToList();
        }

        public Item GetItem(int restauranteId, int itemId) => _context.Items
            .Where(item => item.IdRestaurante == restauranteId && item.Id == itemId).FirstOrDefault();

        public IEnumerable<Item> GetItensDeRestaurante(int restauranteId)
        {
            return _context.Items.Where(item => item.IdRestaurante == restauranteId);
        }

        public bool RestauranteExiste(int restauranteId)
        {
            return _context.Restaurants.Any(restaurante => restaurante.Id == restauranteId);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
