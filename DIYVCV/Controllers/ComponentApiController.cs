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

namespace DIYVCV.Controllers
{
    public class ComponentApiController : ApiController
    {
        private DIYVCVDataContext db = new DIYVCVDataContext();

        // GET: api/Component/GetComponents
        public IQueryable<Component> GetComponents() 
        {
            return db.Components;
        }

        // GET: api/Component/GetComponent/5
        [ResponseType(typeof(Component))]
        public IHttpActionResult GetComponent(int id)
        {
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return NotFound();
            }

            return Ok(component);
        }

        // PUT: api/Components/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComponent(int id, Component component)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != component.ComponentId)
            {
                return BadRequest();
            }

            db.Entry(component).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponentExists(id))
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

        // POST: api/Components/PostComponent/
        [ResponseType(typeof(Component))]
        public IHttpActionResult PostComponent(Component component)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Components.Add(component);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = component.ComponentId }, component);
        }

        // DELETE: api/Components/DeleteComponent/5
        [ResponseType(typeof(Component))]
        public IHttpActionResult DeleteComponent(int id)
        {
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return NotFound();
            }

            db.Components.Remove(component);
            db.SaveChanges();

            return Ok(component);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComponentExists(int id)
        {
            return db.Components.Count(e => e.ComponentId == id) > 0;
        }
    }
}