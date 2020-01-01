using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestrialObject)
        {
            _context.CelestialObjects.Add(celestrialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestrialObject.Id }, celestrialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var existingObject = _context.CelestialObjects.Find( id);

            if (existingObject == null)
                return NotFound();

            existingObject.Name = celestialObject.Name;
            existingObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            existingObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(existingObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string Name)
        {
            var existingObject = _context.CelestialObjects.Find(id);

            if (existingObject == null)
                return NotFound();

            existingObject.Name = Name;
            _context.CelestialObjects.Update(existingObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingObjects = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id);

            if (!existingObjects.Any())
                return NotFound();

            _context.CelestialObjects.RemoveRange(existingObjects);
            _context.SaveChanges();
            return NoContent();
        }
    } 
}
