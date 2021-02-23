using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DIYVCV.Models;
using System.Diagnostics;

namespace DIYVCV.Controllers
{
    public class ModulexcomponentDataController : ApiController
    {
        private DIYVCVDataContext db = new DIYVCVDataContext();

        // GET: api/ModulexcomponentApi/GetModulexcomponents
        public IEnumerable<ModulexcomponentDto> GetModulexcomponent()
        {
            List<Modulexcomponent> Modulexcomponents = db.Modulexcomponents.ToList();
            List<ModulexcomponentDto> ModulexcomponentDtos = new List<ModulexcomponentDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Modulexcomponent in Modulexcomponents)
            {
                ModulexcomponentDto NewModulexcomponent = new ModulexcomponentDto
                {
                    ModulesxcomponentsId = Modulexcomponent.ModulesxcomponentsId,
                    modulesxcomponentsmoduleid = Modulexcomponent.modulesxcomponentsmoduleid,
                    modulesxcomponentscomponentid = Modulexcomponent.modulesxcomponentscomponentid,
                    componentamount = Modulexcomponent.componentamount
                };
                ModulexcomponentDtos.Add(NewModulexcomponent);
            }

            return ModulexcomponentDtos;
        }

        // GET: api/ModulexcomponentApi/FindModulexcomponentApi/5
        [ResponseType(typeof(ModulexcomponentDto))]
        [HttpGet]
        public IHttpActionResult FindModulexcomponentApi(int id)
        {
            Modulexcomponent Modulexcomponent = db.Modulexcomponents.Find(id);
            if (Modulexcomponent == null)
            {
                return NotFound();
            }

            ModulexcomponentDto ModulexcomponentDto = new ModulexcomponentDto
            {
                ModulesxcomponentsId = Modulexcomponent.ModulesxcomponentsId,
                modulesxcomponentsmoduleid = Modulexcomponent.modulesxcomponentsmoduleid,
                modulesxcomponentscomponentid = Modulexcomponent.modulesxcomponentscomponentid,
                componentamount = Modulexcomponent.componentamount
            };

            return Ok(ModulexcomponentDto);
        }

        // POST: api/ModulexcomponentApi/AddModulexcomponentApi/
        [ResponseType(typeof(ModulexcomponentDto))]
        [HttpPost]
        public IHttpActionResult AddModulexcomponentApi([FromBody] Modulexcomponent modulexcomponent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Modulexcomponents.Add(modulexcomponent);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = modulexcomponent.ModulesxcomponentsId }, modulexcomponent);
        }

        // POST: api/ModulexcomponentApi/UpdateModulexcomponentApi/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateModulexcomponentApi(int id, [FromBody] Modulexcomponent modulexcomponent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modulexcomponent.ModulesxcomponentsId)
            {
                return BadRequest();
            }

            db.Entry(modulexcomponent).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModulexcomponentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ModulexcomponentApi/DeleteModulexcomponentApi/5
        [HttpPost]
        public IHttpActionResult DeleteModulexcomponentApi(int id)
        {
            Modulexcomponent modulexcomponent = db.Modulexcomponents.Find(id);
            if (modulexcomponent == null)
            {
                return NotFound();
            }

            db.Modulexcomponents.Remove(modulexcomponent);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModulexcomponentExists(int id)
        {
            return db.Components.Count(e => e.ComponentId == id) > 0;
        }

    }
}
