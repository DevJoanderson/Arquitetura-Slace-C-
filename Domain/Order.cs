namespace Api.Domain
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();


        public string Code { get; set; } = default!;


        public string CustomerName { get; set; } = default!;

        public decimal TotalAmount { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}