namespace CleanArchMvc.Application.Products.Handlers
{
    public class GetProductQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
            => _productRepository = productRepository;

        public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
            => await _productRepository.GetProductsAsync();
    }
}
