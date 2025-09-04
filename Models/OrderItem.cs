namespace BrigadeiroApp.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public OrderItem? Order { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } //congela o preço na época do pedido
    public decimal UnitCost { get; set; } // gongela o custo
}
