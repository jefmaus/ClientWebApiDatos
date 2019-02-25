using ClientWebApiDatos.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ClientWebApiDatos.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient client = new HttpClient();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var users = await GetUsers();
            return View(users);
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ModifyUser(int id, string user, string estado)
        {
            ViewBag.id = id;
            ViewBag.user = user;
            ViewBag.estado = estado;
            return View();
        }

        /// <summary>
        /// Cliente webapi para crear un nuevo usuario
        /// </summary>
        /// <param name="usuario">objeto de tipo usuario</param>
        /// <returns>Devuelve el id del usuario creado si tiene éxito, 2 si falta completar algún campo y 0 si no pudo guardar, </returns>
        [HttpPost]
        public async Task<ActionResult> GuardarUsuario(Usuario usuario)
        {
            if (usuario != null && usuario.usuario_red != null)
            {
                int res = 0;
                var client = new HttpClient();
                var ressponse = await client.PostAsJsonAsync("http://localhost:8088/api/Usuario/", usuario);
                if (ressponse.IsSuccessStatusCode)
                {
                    res = await ressponse.Content.ReadAsAsync<int>();
                    if (res > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else if (res == -1)
                    {                        
                        ViewBag.Msg = "El nombre de usuario ya existe";
                        return View("CreateUser");
                    }
                    else
                    {
                        ViewBag.Msg = "Error al intentar crear el usuario";
                        return View("CreateUser");
                    }
                }
            }
            return View("CreateUser");
        }

        [HttpPost]
        public async Task<ActionResult> ModificarUsuario(Usuario usuario)
        {
            if (usuario != null && usuario.usuario_red != null)
            {
                int res = 0;
                var client = new HttpClient();
                var ressponse = await client.PutAsJsonAsync("http://localhost:8088/api/Usuario/", usuario);
                if (ressponse.IsSuccessStatusCode)
                {
                    res = await ressponse.Content.ReadAsAsync<int>();
                    if (res == 1)
                    {
                        return RedirectToAction("Index");
                    }
                    else if (res == -1)
                    {
                        ViewBag.user = usuario.usuario_red;
                        ViewBag.Msg = "El nombre de usuario ya existe.";
                        return View("ModifyUser");
                    }
                    else
                    {
                        ViewBag.user = usuario.usuario_red;
                        ViewBag.Msg = "Error al intentar modificar el usuario";
                        return View("ModifyUser");
                    }
                }
            }
            ViewBag.id = usuario.id_usuario;
            ViewBag.estado = usuario.estado;
            return View("ModifyUser");
        }

        /// <summary>
        /// Cliente WebApi para obtener uno o todos los usuarios
        /// </summary>
        /// <param name="id">id del usuario</param>
        /// <returns>Un usuario o una lista de usuarios</returns>
        public async Task<List<Usuario>> GetUsers(int id = 0)
        {
            string json;
            var client = new HttpClient();
            if(id == 0) { 
                 json = await client.GetStringAsync("http://localhost:8088/api/Usuario/");
            } else {
                json = await client.GetStringAsync("http://localhost:8088/api/Usuario/"+id);
            }
            var userList = JsonConvert.DeserializeObject<List<Usuario>>(json); // usando paquete NewtonSoft.json
            return userList;
        }

        /// <summary>
        /// Deshabilita un usuario
        /// </summary>
        /// <param name="id">id de usuario</param>
        /// <returns>Devuelve 1 si lo deshabilitó, 2 para debe completar todos los campos y 0 si hubo un error</returns>
        public async Task<ActionResult> DisableUser(int id)
        {
            var client = new HttpClient();
            var ressponse = await client.DeleteAsync("http://localhost:8088/api/Usuario/"+id);
            if (ressponse.IsSuccessStatusCode)
            {
                int res = await ressponse.Content.ReadAsAsync<int>();

                if (res == 1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Msg = "Error al intentar eliminar el usuario";
                    return View("CreateUser");
                }
            } else
            {
                ViewBag.Msg = "Error al intentar eliminar el usuario";
                return View("CreateUser");
            }
            
        }


    }
}