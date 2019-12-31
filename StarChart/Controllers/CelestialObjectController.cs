using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]  
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var obj =  _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();

            if (obj != null)
            {
                obj.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
                return Ok(obj);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var obj = _context.CelestialObjects.Where(x => x.Name == name).ToList();

            if (obj.Any())
            {
                foreach(var o in obj)
                {
                    o.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == o.Id).ToList();
                }

                return Ok(obj);

            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var obj = _context.CelestialObjects.ToList();

            if (obj.Any())
            {
                foreach (var o in obj)
                {
                    o.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == o.Id).ToList();
                }

                return Ok(obj);

            }
            else
            {
                return NotFound();
            }
        }
    } 
}
