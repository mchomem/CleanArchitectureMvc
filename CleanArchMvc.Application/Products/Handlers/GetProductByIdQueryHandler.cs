namespace CleanArchMvc.Application.Products.Handlers;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        => await _productRepository.GetByIdAsync(request.Id);
}
