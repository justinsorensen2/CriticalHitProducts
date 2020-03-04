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
  public class CategoriesController : ControllerBase
  {
    //create public db context accessible by entire class
    public DatabaseContext db { get; set; } = new DatabaseContext();
    //create post to add a product category
    [HttpPost]
    public async Task<ActionResult<Category>> AddProductCategory(Category newCat)
    {
      await db.Categories.AddAsync(newCat);
      await db.SaveChangesAsync();
      return Ok(newCat);
    }
    //create get to view all categories
    [HttpGet]
    public async Task<ActionResult<List<Category>>> ViewAllCategories()
    {
      var pList = await db.Categories.OrderBy(l => l.CategoryName).ToListAsync();
      if (pList == null)
      {
        return NotFound();
      }
      else
      {
        return pList;
      }
    }
  }
}