using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExemploDependencyResolver.Dependencias;

namespace ExemploDependencyResolver.Controllers
{
    public class TemDependenciasController : Controller
    {
        public IDependencia1 Dependencia1 { get; set; }
        public IDependencia2 Dependencia2 { get; set; }

        public TemDependenciasController(IDependencia1 dependencia1, IDependencia2 dependencia2)
        {
            Dependencia1 = dependencia1;
            Dependencia2 = dependencia2;
        }

        public ActionResult Index()
        {
            var b = this.Dependencia1.Metodo("foo");
            this.Dependencia2.AlgumMetodo();

            return View();
        }
    }
}
