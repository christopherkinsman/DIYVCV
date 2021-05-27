using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DIYVCV.Models;
using DIYVCV.Models.ViewModels;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;

namespace DIYVCV.Controllers
{
    public class ComponentController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static ComponentController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("http://diyvcv-env.eba-pza4kszm.us-east-2.elasticbeanstalk.com/api/");
            //client.BaseAddress = new Uri("https://localhost:44300/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Routes to a dynamically generated "Component List" Page. Gathers information from the database.
        /// </summary>
        /// <param></param>
        /// <returns>A dynamic "List Component" webpage which provides a list of all the components in the database.</returns>
        /// <example>GET : /Component/List/5</example>
        public ActionResult List()
        {
            string url = "componentdata/getcomponents";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ComponentDto> SelectedComponents = response.Content.ReadAsAsync<IEnumerable<ComponentDto>>().Result;
                return View(SelectedComponents);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Routes to a dynamically generated "Component Show" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Component</param>
        /// <returns>A dynamic "Component Show" webpage which provides detailes for a selected component.</returns>
        /// <example>GET : /Component/Show/5</example>
        public ActionResult Show(int id)
        {
            ShowComponent ViewModel = new ShowComponent();
            string url = "componentdata/findcomponent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                ComponentDto SelectedComponent = response.Content.ReadAsAsync<ComponentDto>().Result;
                ViewModel.component = SelectedComponent;

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

        // POST: Module/Create
        [HttpPost]
        public ActionResult Create(Component ComponentInfo)
        {
            Debug.WriteLine(ComponentInfo.ComponentName);
            string url = "componentdata/addcomponent";
            Debug.WriteLine(jss.Serialize(ComponentInfo));
            HttpContent content = new StringContent(jss.Serialize(ComponentInfo));
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
        /// Routes to a dynamically generated "Component Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Component</param>
        /// <returns>A dynamic "Module Show" webpage which provides a form to input new component information.</returns>
        /// <example>GET : /Component/Update/5</example>
        public ActionResult Update(int id)
        {
            UpdateComponent ViewModel = new UpdateComponent();

            string url = "componentdata/findComponent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                ComponentDto SelectedComponent = response.Content.ReadAsAsync<ComponentDto>().Result;
                ViewModel.component = SelectedComponent;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Routes to a dynamically generated "Component Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Component</param>
        /// <returns>A dynamic "Module Show" webpage which provides a form to input new component information.</returns>
        /// <example>GET : /Component/Update/5</example>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Update(int id, Component ComponentInfo)
        {
            Debug.WriteLine(ComponentInfo.ComponentName);
            string url = "componentdata/updatecomponent/" + id;
            Debug.WriteLine(jss.Serialize(ComponentInfo));
            HttpContent content = new StringContent(jss.Serialize(ComponentInfo));
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
        /// Routes to a dynamically generated "Component Delete" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Component</param>
        /// <returns>A dynamic "Module Show" webpage which provides information on a component that can be deleted.</returns>
        /// <example>GET : /Component/Delete/5</example>
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "componentdata/findcomponent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Component SelectedComponent = response.Content.ReadAsAsync<Component>().Result;
                return View(SelectedComponent);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Routes to a dynamically generated "Component Delete" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Component</param>
        /// <returns>A dynamic "Module Show" webpage which provides information on a component that can be deleted.</returns>
        /// <example>GET : /Component/Delete/5</example>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "componentdata/deletecomponent/" + id;
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