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
    public class ComponentDataController : ApiController
    {
        private DIYVCVDataContext db = new DIYVCVDataContext();

        /// <summary>
        /// Returns a list of Component in the system
        /// </summary>
        /// <returns>
        /// A list of Component Objects with fields mapped to the database column values (ComponenteName, ComponentValue).
        /// </returns>
        /// <example>GET api/ComponentData/GetComponents -> {Component Object, Component Object, Component Object...}</example>
        [ResponseType(typeof(IEnumerable<ComponentDto>))]
        public IHttpActionResult GetComponents()
        {
            List<Component> Components = db.Components.ToList();
            List<ComponentDto> ComponentDtos = new List<ComponentDto> { };

            foreach (var Component in Components)
            {
                ComponentDto NewComponent = new ComponentDto
                {
                    ComponentId = Component.ComponentId,
                    ComponentName = Component.ComponentName,
                    ComponentValue = Component.ComponentValue,
                    ModuleId = Component.ModuleId,
                    ComponentQuantity = Component.ComponentQuantity
                };
                ComponentDtos.Add(NewComponent);
            }

            return Ok(ComponentDtos);
        }

        /// <summary>
        /// Finds a Component from the Database through an id.
        /// </summary>
        /// <param name="id">The Component ID</param>
        /// <returns>Component object containing information about the Component with a matching ID. Empty Component Object if the ID does not match any Components in the system.</returns>
        /// <example>api/ComponentData/FindComponent/6 -> {Component Object}</example>
        /// <example>api/ComponentData/FindComponent/10 -> {Component Object}</example>
        [ResponseType(typeof(ComponentDto))]
        [HttpGet]
        public IHttpActionResult FindComponent(int id)
        {
            Component Component = db.Components.Find(id);
            if (Component == null)
            {
                return NotFound();
            }

            ComponentDto ComponentDto = new ComponentDto
            {
                ComponentId = Component.ComponentId,
                ComponentName = Component.ComponentName,
                ComponentValue = Component.ComponentValue,
                ModuleId = Component.ModuleId,
                ComponentQuantity = Component.ComponentQuantity
            };

            return Ok(ComponentDto);
        }

        /// <summary>
        /// Adds a component to the Database.
        /// </summary>
        /// <param name="NewComponent">An object with fields that map to the columns of the component's table. </param>
        /// <example>
        /// POST api/ComponentData/AddComponent 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"ComponentId":"1",
        ///	"ComponentName":"Resistor"
        /// }
        /// </example>
        [ResponseType(typeof(Component))]
        [HttpPost]
        public IHttpActionResult AddComponent([FromBody] Component component)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Components.Add(component);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = component.ComponentId }, component);
        }

        /// <summary>
        /// Updates a Component on the Database.
        /// </summary>
        /// <param name="ComponentInfo">An object with fields that map to the columns of the Component's table.</param>
        /// <example>
        /// POST api/ComponentData/UpdateComponent/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"ComponentId":"1",
        ///	"ComponentName":"Resistor"
        /// }
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateComponent(int id, [FromBody] Component component)
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

        /// <summary>
        /// Deletes an component from the connected Database if the ID of that component exists.
        /// </summary>
        /// <param name="id">The ID of the component.</param>
        /// <example>POST /api/ComponentData/DeleteComponent/3</example>
        [HttpPost]
        public IHttpActionResult DeleteComponent(int id)
        {
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return NotFound();
            }

            db.Components.Remove(component);
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

        private bool ComponentExists(int id)
        {
            return db.Components.Count(e => e.ComponentId == id) > 0;
        }
    }
}
