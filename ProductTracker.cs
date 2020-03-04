using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CriticalHitProducts.Models;
using CriticalHitProducts.Controllers;

namespace CriticalHitProducts
{
  public class ProductTracker
  {
    //create public db context accessible by entire class
    public DatabaseContext db { get; set; } = new DatabaseContext();
    //create method to update products list in Location
    public async void UpdateLocationProducts(int locationId, Product newProduct)
    {
      var location = await db.Locations.FirstOrDefaultAsync(l => l.Id == locationId);
      location.Products.Add(newProduct);
      await db.SaveChangesAsync();
    }
  }
}