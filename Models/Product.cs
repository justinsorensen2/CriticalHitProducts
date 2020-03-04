using System;

namespace CriticalHitProducts
{
  public class Product
  {
    public int Id { get; set; }
    public string SKU { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int NumberInStock { get; set; }
    public string Description { get; set; }
    public DateTime DateOrdered { get; set; }
    public DateTime? OutOfStock { get; set; }
    public int TypeId { get; set; }
    public Type Type { get; set; }
  }
}