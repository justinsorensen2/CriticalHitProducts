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
    public bool OutOfStock { get; set; } = false;
    public int TypeId { get; set; }
    public Category Category { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }

  }
}