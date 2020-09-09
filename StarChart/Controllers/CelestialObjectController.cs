﻿using System.Collections.Generic;
using System.Linq;
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

        [HttpGet( "{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var stored = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (stored == null)
            {
                return NotFound();
            }

            if (stored.Satellites == null)
            {
                stored.Satellites = new List<CelestialObject>();
            }

            foreach (var obj in _context.CelestialObjects.Where(x=> x.OrbitedObjectId == stored.Id))
            {
                stored.Satellites.Add(obj);
            }

            return Ok(stored);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var matches = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (matches.Count==0)
            {
                return NotFound();
            }

            foreach (var o in matches)
            {
                if (o.Satellites == null)
                {
                    o.Satellites = new List<CelestialObject>();
                }
                foreach (var obj in _context.CelestialObjects.Where(x => x.OrbitedObjectId == o.Id))
                {
                    o.Satellites.Add(obj);
                }
            }

            return Ok(matches);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var matches = _context.CelestialObjects.ToList();
            if (matches.Count == 0)
            {
                return NotFound();
            }

            foreach (var o in matches)
            {
                if (o.Satellites == null)
                {
                    o.Satellites = new List<CelestialObject>();
                }
                foreach (var obj in _context.CelestialObjects.Where(x => x.OrbitedObjectId == o.Id))
                {
                    o.Satellites.Add(obj);
                }
            }

            return Ok(matches);
        }

    }
}
