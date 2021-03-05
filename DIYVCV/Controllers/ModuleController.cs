using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DIYVCV.Models;
using DIYVCV.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace DIYVCV.Controllers
{
    public class ModuleController : Controller
    {

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static ModuleController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44300/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Routes to a dynamically generated "Module List" Page. Gathers information from the database.
        /// </summary>
        /// <param></param>
        /// <returns>A dynamic "List Module" webpage which provides a list of all the modules in the database.</returns>
        /// <example>GET : /Module/List/5</example>
        public ActionResult List()
        {
            string url = "moduledata/getmodules";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ModuleDto> SelectedModules = response.Content.ReadAsAsync<IEnumerable<ModuleDto>>().Result;
                return View(SelectedModules);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Routes to a dynamically generated "Module Show" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Module</param>
        /// <returns>A dynamic "Module Show" webpage which provides detailes for a selected module.</returns>
        /// <example>GET : /Modle/Show/5</example>
        public ActionResult Show(int id)
        {
            ShowModule ViewModel = new ShowModule();
            string url = "moduledata/findmodule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {

                ModuleDto SelectedModule = response.Content.ReadAsAsync<ModuleDto>().Result;
                ViewModel.module = SelectedModule;

                url = "moduledata/GetComponentsForModules/" + id;
                response = client.GetAsync(url).Result;
                IEnumerable<ComponentDto> SelectedComponents = response.Content.ReadAsAsync<IEnumerable<ComponentDto>>().Result;
                ViewModel.component = SelectedComponents;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }



        // GET: Module/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Module/Create/5
        [HttpPost]
  
        public ActionResult Create(Module ModuleInfo)
        {
            Debug.WriteLine(ModuleInfo.ModuleName);
            string url = "moduledata/addmodule";
            Debug.WriteLine(jss.Serialize(ModuleInfo));
            HttpContent content = new StringContent(jss.Serialize(ModuleInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        /// <summary>
        /// Routes to a dynamically generated "Module Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Module</param>
        /// <returns>A dynamic "Module Show" webpage which provides a form to input new module information.</returns>
        /// <example>GET : /Modle/Update/5</example>
        public ActionResult Update(int id)
        {
            UpdateModule ViewModel = new UpdateModule();

            string url = "moduledata/findmodule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {

                ModuleDto SelectedModule = response.Content.ReadAsAsync<ModuleDto>().Result;
                ViewModel.module = SelectedModule;

                url = "moduledata/GetComponentsForModules/" + id;
                response = client.GetAsync(url).Result;
                IEnumerable<ComponentDto> SelectedComponents = response.Content.ReadAsAsync<IEnumerable<ComponentDto>>().Result;
                ViewModel.component = SelectedComponents;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Routes to a dynamically generated "Module Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Module</param>
        /// <returns>A dynamic "Module Show" webpage which provides a form to input new module information.</returns>
        /// <example>GET : /Modle/Update/5</example>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Update(int id, Module ModuleInfo, Component ComponentInfo, HttpPostedFileBase ModulePic)
        {
            Debug.WriteLine(ModuleInfo.ModuleName);
            string url = "moduledata/updatemodule/" + id;
            Debug.WriteLine(jss.Serialize(ModuleInfo));
            HttpContent content = new StringContent(jss.Serialize(ModuleInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                if (ModulePic != null)
                {
                    Debug.WriteLine("Calling Update Image method.");
                    url = "moduledata/updatemodulepic/" + id;

                    MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                    HttpContent imagecontent = new StreamContent(ModulePic.InputStream);
                    requestcontent.Add(imagecontent, "ModulePic", ModulePic.FileName);
                    response = client.PostAsync(url, requestcontent).Result;
                }

                return RedirectToAction("Show", new { id = id } );
            }
            else
            {
                return RedirectToAction("Error");
            }
        }



        /// <summary>
        /// Routes to a dynamically generated "Module Delete" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Module</param>
        /// <returns>A dynamic "Module Show" webpage which provides information on a module that can be deleted.</returns>
        /// <example>GET : /Modle/Delete/5</example>
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "moduledata/findmodule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Module SelectedModule = response.Content.ReadAsAsync<Module>().Result;
                return View(SelectedModule);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Routes to a dynamically generated "Module Delete" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Module</param>
        /// <returns>A dynamic "Module Show" webpage which provides information on a module that can be deleted.</returns>
        /// <example>GET : /Modle/Delete/5</example>
        [HttpPost]

        public ActionResult Delete(int id)
        {
            string url = "moduledata/deletemodule/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}