namespace BrigadeiroApp.Models;

public enum OrderStatus { New, InProgress, Delivered, Cancelled }

public class Order
{
    public int Id { get; set; }
    public string CustomerName  { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;

    public DateTime OrderDateUtc { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveryDate { get; set; }

    public List<OrderItem> Items { get; set; } = new();   // coleção

    public decimal TotalRevenue { get; set; }
    public decimal TotalCost    { get; set; }
    public decimal Profit       { get; set; }
    public decimal ProfitMarcos => Math.Round(Profit * 0.30m, 2);
    public decimal ProfitGabi   => Math.Round(Profit * 0.70m, 2);

    public string CreatedBy { get; set; } = "Marcos";
    public OrderStatus Status { get; set; } = OrderStatus.New;
}
