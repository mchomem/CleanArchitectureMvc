using CleanArchMvc.Domain.Entities;
using MediatR;

namespace CleanArchMvc.Application.Products.Commands
{
    public abstract class ProductCommand : IRequest<Product>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }

        // Em uma aplicação mais complexa, poderia ser utilizado outras propriedades de outras entidades.

        /* Considerações sobre CQRS
           
        1. A abordagem utilizada aqui é bem simples.
        2. Não foi utlizado mensagens para processar comandos e publicar os eventos de atualização.
        3. Não foi usado outro padrão como o Event Sourcing.
        4. Foi utilizado MediatR, mas poderia ser implementado sem, porém isso aumentaria as implementações.
        5. A implementação não está 100% aderente ao CQRS, porém não está errada.
        6. A implementação foi feita na camada Application e não na Domain, pois os métodos e consultas estão atuando
           como métodos do serviço e são clientes do projeto Domain que retornam uma 'visão' otimizada do domínio.
        7. Poderia ser implementado na camada Domain se a mesma fosse mais complexa
        */
    }
}
