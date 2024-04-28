namespace CleanArchMvc.Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
            => _context = context;

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> GetByIdAsync(int? id)
            => await _context.Product
                .Include(x => x.Category) // Eager loader (carregamento adiantado)
                .SingleOrDefaultAsync(x => x.Id == id);
        //=> await _context.Product
        //    .Where(x => x.Id == id)
        //    .FirstAsync();

        public async Task<Product> GetProductCategoryAsync(int? id)
            => await _context.Product
                .Include(x => x.Category) // Eager loader (carregamento adiantado)
                .SingleOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Product>> GetProductsAsync()
            => await _context.Product
                .Include(x => x.Category)
                .ToListAsync();

        public async Task<Product> RemoveAsync(Product product)
        {
            _context.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
