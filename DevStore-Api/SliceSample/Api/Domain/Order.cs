namespace Api.Domain;

// Entidade de domínio simples: representa um Pedido (Order)
public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Ex.: número/identificação do pedido
    public string Code { get; set; } = default!;

    // Nome do cliente que fez o pedido
    public string CustomerName { get; set; } = default!;

    // Valor total do pedido
    public decimal TotalAmount { get; set; }

    // Data de criação (só leitura; setado ao salvar)
    public DateTime CreatedAt { get; set; }
}
