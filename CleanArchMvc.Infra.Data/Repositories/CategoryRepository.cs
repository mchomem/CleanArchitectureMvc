namespace CleanArchMvc.Infra.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
            => _context = context;


        public async Task<Category> CreateAsync(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> GetByIdAsync(int? id)
            => await _context.Category
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
            => await _context.Category.ToListAsync();

        public async Task<Category> RemoveAsync(Category category)
        {
            _context.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();

            return category;
        }
    }
}
