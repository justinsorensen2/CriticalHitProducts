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
  public class LocationsController : ControllerBase
  {
    //create public db context accessible by entire class
    public DatabaseContext db { get; set; } = new DatabaseContext();
    [HttpPost]
    public async Task<ActionResult<Location>> AddStoreLocation(Location newLoc)
    {
      await db.Locations.AddAsync(newLoc);
      await db.SaveChangesAsync();
      return Ok(newLoc);
    }
    //create get to view all categories
    [HttpGet]
    public async Task<ActionResult<List<Location>>> ViewAllLocations()
    {
      var pList = await db.Locations.OrderBy(l => l.Address).ToListAsync();
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