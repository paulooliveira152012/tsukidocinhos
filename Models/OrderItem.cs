namespace BrigadeiroApp.Models;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }                 // FK
    public Order Order { get; set; } = default!;     // nav obrigatória

    public int BrigadeiroTypeId { get; set; }        // FK
    public BrigadeiroType? BrigadeiroType { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal UnitCost  { get; set; }
}
