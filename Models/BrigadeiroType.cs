namespace BrigadeiroApp.Models;

public class BrigadeiroType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; } //preço de venda por unidade
    public decimal UnitCost { get; set; } // custo por unidade
    public bool Active { get; set; }
}