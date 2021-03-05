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
using DIYVCV.Models.ViewModels;
using System.Diagnostics;
using System.Web;
using System.IO;

namespace DIYVCV.Controllers
{
    public class ModuleDataController : ApiController
    {

        private DIYVCVDataContext db = new DIYVCVDataContext();

        /// <summary>
        /// Returns a list of Modules in the system
        /// </summary>
        /// <returns>
        /// A list of Module Objects with fields mapped to the database column values (ModuleName, ModuleCategory).
        /// </returns>
        /// <example>GET api/ModuleData/GetModules -> {Module Object, Module Object, Module Object...}</example>
        [ResponseType(typeof(IEnumerable<ModuleDto>))]
        public IHttpActionResult GetModules()
        {
            List<Module> Modules = db.Modules.ToList();
            List<ModuleDto> ModuleDtos = new List<ModuleDto> { };

            foreach (var Module in Modules)
            {
                ModuleDto NewModule = new ModuleDto
                {
                    ModuleId = Module.ModuleId,
                    ModuleName = Module.ModuleName,
                    ModuleBrand = Module.ModuleBrand,
                    ModuleCategory = Module.ModuleCategory,
                    ModuleDescription = Module.ModuleDescription,
                    ModuleLink = Module.ModuleLink,
                    ModuleSchematic = Module.ModuleSchematic,
                    ModuleHasPic = Module.ModuleHasPic,
                    PicExtension = Module.PicExtension
                };
                ModuleDtos.Add(NewModule);
            }
            return Ok(ModuleDtos);
        }

        /// <summary>
        /// Finds a module from the Database through an id.
        /// </summary>
        /// <param name="id">The Module ID</param>
        /// <returns>Module object containing information about the module with a matching ID. Empty Module Object if the ID does not match any Modules in the system.</returns>
        /// <example>api/ModuleData/FindModule/6 -> {Module Object}</example>
        /// <example>api/ModuleData/FindModule/10 -> {Module Object}</example>
        [ResponseType(typeof(ModuleDto))]
        [HttpGet]
        public IHttpActionResult FindModule(int id)
        {
            Module Module = db.Modules.Find(id);

            if (Module == null)
            {
                return NotFound();
            }

            ModuleDto ModuleDto = new ModuleDto
            {
                ModuleId = Module.ModuleId,
                ModuleName = Module.ModuleName,
                ModuleBrand = Module.ModuleBrand,
                ModuleCategory = Module.ModuleCategory,
                ModuleDescription = Module.ModuleDescription,
                ModuleLink = Module.ModuleLink,
                ModuleSchematic = Module.ModuleSchematic,
                ModuleHasPic = Module.ModuleHasPic,
                PicExtension = Module.PicExtension
            };
            return Ok(ModuleDto);
        }

        [ResponseType(typeof(IEnumerable<ComponentDto>))]
        public IHttpActionResult GetComponentsForModules(int id)
        {
            List<Component> Components = db.Components
                .Where( p => p.ModuleId == id.ToString() )
                .ToList();
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
        /// Adds a Module to the Database.
        /// </summary>
        /// <param name="NewModule">An object with fields that map to the columns of the modules's table. </param>
        /// <example>
        /// POST api/ModuleData/AddModule 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"ModuleId":"1",
        ///	"ComponentName":"Plaits",
        /// }
        /// </example>
        [ResponseType(typeof(Module))]
        [HttpPost]
        public IHttpActionResult AddModule([FromBody] Module module)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Modules.Add(module);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = module.ModuleId }, module);
        }

        /// <summary>
        /// Updates an Module on the Database.
        /// </summary>
        /// <param name="ModuleInfo">An object with fields that map to the columns of the modules's table.</param>
        /// <example>
        /// POST api/ModuleData/UpdateModule/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"ModuleId":"1",
        ///	"ComponentName":"Plaits",
        /// }
        /// </example>
        [ResponseType(typeof(ModuleDto))]
        [HttpPost]
        public IHttpActionResult UpdateModule(int id, [FromBody] Module module)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != module.ModuleId)
            {
                return BadRequest();
            }

            db.Entry(module).State = EntityState.Modified;
            // Picture update is handled by another method
            db.Entry(module).Property(p => p.ModuleHasPic).IsModified = false;
            db.Entry(module).Property(p => p.PicExtension).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
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
        /// Receives module picture data, uploads it to the webserver and updates the module's HasPic option
        /// </summary>
        /// <param name="id">the module id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F modulepic=@file.jpg "https://localhost:xx/api/moduledata/updatemodulepic/1"
        /// POST: api/ModuleData/UpdateModulePic/1
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        [HttpPost]
        public IHttpActionResult UpdateModulePic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var ModulePic = HttpContext.Current.Request.Files[0];
                    if (ModulePic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(ModulePic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/modules/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/modules/"), fn);

                                //save the file
                                ModulePic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the player haspic and picextension fields in the database
                                Module SelectedModule = db.Modules.Find(id);
                                SelectedModule.ModuleHasPic = haspic;
                                SelectedModule.PicExtension = extension;
                                db.Entry(SelectedModule).State = EntityState.Modified;

                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Module Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                            }
                        }
                    }

                }
            }

            return Ok();
        }

        /// <summary>
        /// Deletes an Module from the connected Database if the ID of that module exists.
        /// </summary>
        /// <param name="id">The ID of the module.</param>
        /// <example>POST /api/ModuleData/DeleteModule/3</example>
        [HttpPost]
        public IHttpActionResult DeleteModule(int id)
        {
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return NotFound();
            }

            db.Modules.Remove(module);
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

        private bool ModuleExists(int id)
        {
            return db.Modules.Count(e => e.ModuleId == id) > 0;
        }
    }
}