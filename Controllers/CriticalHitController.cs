using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CriticalHitProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CriticalHitProducts.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CriticalHitController : ControllerBase
  {
    //create public db context accessible by entire class
    public DatabaseContext db { get; set; } = new DatabaseContext();
    //create get to view all prods
    [HttpGet]
    public async Task<ActionResult<List<Product>>> ViewAllProducts()
    {
      var pList = await db.Products.OrderBy(p => p.Name).ToListAsync();
      if (pList == null)
      {
        return NotFound();
      }
      else
      {
        return pList;
      }
    }
    //create get to view item by sku
    [HttpGet("sku/{sku}")]
    public async Task<ActionResult<Product>> ViewProductBySKU(string sku)
    {
      var product = await db.Products.FirstOrDefaultAsync(p => p.SKU == sku);
      if (product == null)
      {
        return NotFound();
      }
      else
      {
        return product;
      }
    }
    //create get to view item by id
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> ViewProductById(int id)
    {
      var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
      if (product == null)
      {
        return NotFound();
      }
      else
      {
        return Ok(product);
      }
    }
    //create post to add a product
    [HttpPost]
    public async Task<ActionResult<Product>> AddProduct(Product newProduct)
    {
      var tracker = new ProductTracker();
      await db.Products.AddAsync(newProduct);
      await db.SaveChangesAsync();
      var locationId = newProduct.LocationId;
      //call method to update list of products
      tracker.UpdateLocationProducts(locationId, newProduct);
      return Ok(newProduct);
    }
    //replace all properties of existing prod with new props
    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product alteredProduct)
    {
      alteredProduct.Id = id;
      db.Entry(alteredProduct).State = EntityState.Modified;
      await db.SaveChangesAsync();
      return Ok(alteredProduct);
    }
    //delete a product by id
    [HttpDelete("{id}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
      var deleteProduct = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
      if (deleteProduct == null)
      {
        return NotFound();
      }
      else
      {
        db.Products.Remove(deleteProduct);
        await db.SaveChangesAsync();
        return Ok(deleteProduct);
      }
    }
    //create get to view all out of stock
    [HttpGet("stock")]
    public async Task<ActionResult<Product>> ViewProductsOutOfStock()
    {
      var pList = await db.Products.Where(p => p.NumberInStock == 0).ToListAsync();
      if (pList == null)
      {
        return NotFound();
      }
      else
      {
        return Ok(pList);
      }
    }
  }
}