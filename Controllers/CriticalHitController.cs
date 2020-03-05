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
    //create get to view all prods at a location
    [HttpGet("location/{locationId}")]
    public async Task<ActionResult<List<Product>>> ViewAllProducts(int locationId)
    {
      var location = await db.Locations.FirstOrDefaultAsync(l => l.Id == locationId);
      var pList = location.Products;
      if (pList == null)
      {
        return NotFound();
      }
      else
      {
        return Ok(pList.ToList());
      }
    }
    //create get to view item by sku
    [HttpGet("sku/{sku}")]
    public async Task<ActionResult<Product>> ViewProductBySKU(string sku)
    {
      var product = await db.Products.Select(p => p.SKU == sku).ToListAsync();
      if (product == null)
      {
        return NotFound();
      }
      else
      {
        return Ok(product.ToList());
      }
    }
    //create get to view item by id
    [HttpGet("{productId}/{locationId}")]
    public async Task<ActionResult<Product>> ViewProductById(int productId, int locationId)
    {
      var productLocation = db.Products.Where(p => p.LocationId == locationId);
      var product = await productLocation.FirstOrDefaultAsync(p => p.Id == productId);
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
      await db.Products.AddAsync(newProduct);
      await db.SaveChangesAsync();
      var locationId = newProduct.LocationId;
      //call method to update list of products
      //find location
      var location = await db.Locations.FirstOrDefaultAsync(l => l.Id == locationId);
      //add new product
      location.Products.Add(newProduct);
      return Ok(newProduct);
    }
    //replace all properties of existing prod with new props
    [HttpPut("{locationId}/update/{productId}")]
    public async Task<ActionResult<Product>> UpdateProduct(int locationId, Product alteredProduct, int productId)
    {
      var originalProduct = await db.Products.FirstOrDefaultAsync(p => p.Id == productId);
      alteredProduct.Id = productId;
      db.Entry(alteredProduct).State = EntityState.Modified;
      await db.SaveChangesAsync();
      //find location
      var location = await db.Locations.FirstOrDefaultAsync(l => l.Id == locationId);
      //remove original product from list
      location.Products.Remove(originalProduct);
      //add updated product to list
      location.Products.Add(alteredProduct);
      return Ok(alteredProduct);
    }
    //delete a product by location and product id
    [HttpDelete("{productId}/{locationId}")]
    public async Task<ActionResult<Product>> DeleteProduct(int productId, int locationId)
    {
      var productLocation = await db.Locations.FirstOrDefaultAsync(l => l.Id == locationId);
      var deleteProduct = await db.Products.FirstOrDefaultAsync(p => p.Id == productId);
      if (deleteProduct == null)
      {
        return NotFound();
      }
      else
      {
        //find location
        var location = await db.Locations.FirstOrDefaultAsync(l => l.Id == locationId);
        //delete product from list
        location.Products.Remove(deleteProduct);
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
        return Ok(pList.ToList());
      }
    }
    //create get to view all out of stock items by location
    [HttpGet("location/stock/{locationId}")]
    public async Task<ActionResult<Product>> ViewProductsOutOfStockByLocation(int locationId)
    {
      var pList = await db.Products.Where(p => p.NumberInStock == 0).Select(l => l.LocationId == locationId).ToListAsync();
      if (pList == null)
      {
        return NotFound();
      }
      else
      {
        return Ok(pList.ToList());
      }
    }
  }
}