using devchat3.Data;
using devchat3.Models;
using devchat3.UOW;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using System.Text.Json;
using Nancy.Json;
using System.Runtime.Serialization.Json;
using System.Net;
using Newtonsoft.Json;
using devchat3.Models.facebookUser;

namespace devchat3.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IUOW uow;

        public AccesoController(IUOW uow)
        {
            this.uow = uow;
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
           
            var identity = result.Principal.Identities.FirstOrDefault();
            var currentSidClaim = identity.FindFirst(x => x.Type == ClaimTypes.Sid);
            var email = identity.FindFirst(x => x.Type == ClaimTypes.Email);
            var userName = identity.FindFirst(x => x.Type == ClaimTypes.Name);
            var avatar = identity.FindFirst("urn:google:image");
            if (currentSidClaim != null)
                identity.RemoveClaim(currentSidClaim);

            identity.AddClaim(new Claim(ClaimTypes.Sid, "claim"));

            Usuario user = new Usuario();

            if (avatar != null)
            {
                user.photo = avatar.Value;
            }
            else
            {
                user.photo = "";
            }

            user.userName = userName.Value;
            user.email = email.Value;
            user.isGoogle = true;
            user.password = "";
            user.confirmar = "";
            user.fullname = userName.Value;
            user.nac = DateTime.Today;
            Usuario userAux = uow.Repousuario.Get(email.Value);
            if (userAux == null)
            {
                uow.Repousuario.Insert(user);
                uow.Save();
                userAux = uow.Repousuario.Get(email.Value);
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userAux.userName),
                        new Claim(ClaimTypes.Email, userAux.email),
                        new Claim(ClaimTypes.NameIdentifier, userAux.Id.ToString()),
                    };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }
            else
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userAux.userName),
                        new Claim(ClaimTypes.Email, userAux.email),
                        new Claim(ClaimTypes.NameIdentifier, userAux.Id.ToString()),
                    };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }
            return RedirectToAction("index", "Home", user);
        }

        public async Task LoginFacebook()
        {
            await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = Url.Action("FacebookResponse") });
        }

        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var token = await HttpContext.GetTokenAsync("access_token");

            var identity = result.Principal.Identities.FirstOrDefault();
            var currentSidClaim = identity.FindFirst(x => x.Type == ClaimTypes.Sid);

            string cons = "https://graph.facebook.com/me?fields=birthday,email,id,picture{url},name,first_name&access_token="+token;
            WebRequest req = WebRequest.Create(cons);
            WebResponse res = req.GetResponse();
            StreamReader dat = new StreamReader(res.GetResponseStream());
            var json = dat.ReadToEnd().Trim();
            FacebookUser datos = JsonConvert.DeserializeObject<FacebookUser>(json);
            
            Usuario user = new Usuario();
            user.userName = datos.name;
            user.nac = datos.birthday;
            user.fullname = datos.name;
            user.email = datos.email;
            user.photo = datos.picture.data.url;
            user.isFacebook = true;
            user.password = "";
            user.confirmar = "";

            if (currentSidClaim != null)
                identity.RemoveClaim(currentSidClaim);

            identity.AddClaim(new Claim(ClaimTypes.Sid, "claim"));
            Usuario userAux = uow.Repousuario.Get(datos.email);
            if (userAux == null)
            {
                uow.Repousuario.Insert(user);
                uow.Save();
                userAux = uow.Repousuario.Get(datos.email);
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userAux.userName),
                        new Claim(ClaimTypes.Email, userAux.email),
                        new Claim(ClaimTypes.NameIdentifier, userAux.Id.ToString()),
                    };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }
            else
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userAux.userName),
                        new Claim(ClaimTypes.Email, userAux.email),
                        new Claim(ClaimTypes.NameIdentifier, userAux.Id.ToString()),
                    };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }
            return RedirectToAction("index", "Home", user);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Usuario usuario)
        {
            if (usuario.password == usuario.confirmar)
            {
                usuario.password = Sha256(usuario.password);
            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            if (uow.Repousuario.ExisteUsername(usuario.userName))
            {
                ViewData["Mensaje"] = "El username ya existe!";
                return View();
            }

            if (uow.Repousuario.ExisteEmail(usuario.email))
            {
                ViewData["Mensaje"] = "El email ya se encuentra registrado";
                return View();
            }

            uow.Repousuario.Insert(usuario);
            uow.Save();
            ViewData["Mensaje"] = "Usuario registrado";
            return RedirectToAction("Login", "Acceso");

        }

        [HttpPost]
        public async Task <IActionResult> Login(string dato, string password)
        {
            password = Sha256(password);
            if(uow.Repousuario.ExisteUsername(dato) || uow.Repousuario.ExisteEmail(dato))
            {
                Usuario user = uow.Repousuario.Get(dato);
                if (user.password == password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.userName),
                        new Claim(ClaimTypes.Email, user.email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("index","Home",user);
                }
                else
                {
                    ViewData["Mensaje"] = "Clave invalida";
                    return View();
                }
            }
            ViewData["Mensaje"] = "Usuario no encontrado";
            return View();

        }

        public async Task <IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }

        public static string Sha256(string password)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(password));

                foreach(byte b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

    }
}
