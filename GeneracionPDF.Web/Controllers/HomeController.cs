using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeneracionPDF.Web.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            var modelo = new Modelo.ViewModels.vmGeneracionPDF("DaLegend net", 3, 4, @"C:\Exportado");
            return View(modelo);
        }


        /// <summary>
        /// Este método generará tu pdf, lo hará con respecto al post que hayas realizado
        /// </summary>
        /// <param name="modelo">
        /// (ESP) En esta sección se envia el nombre de tu libro, numero de niveles, subniveles y la ruta donde se guarará el PDF
        /// (ENG) this section gets the name, nivels and path
        /// </param>
        /// <returns>
        /// (ESP) Regresará el mensaje satisfactorio donde podrás encontrar tu documento
        /// (ENG) Return the transaction messages
        /// </returns>
        [HttpPost]
        public ActionResult GeneracionPDFAction(Modelo.ViewModels.vmGeneracionPDF modelo)
        {
            modelo.Transaccion = new Negocio.CreatePDF.CreatePDF().CrearPDF(modelo.NombreLibroPDF, modelo.Niveles, modelo.SubNiveles, modelo.RutaPDF);
            return PartialView("Partials/_partialGeneracionPDF", modelo);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}