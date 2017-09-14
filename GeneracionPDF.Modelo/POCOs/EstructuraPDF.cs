using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneracionPDF.Modelo.POCOs
{
    /// <summary>
    /// (ESP) Esta es la estructura que tendrá el PDF
    /// (ENG) This is the structure that covers the PDF
    /// </summary>
    public class EstructuraPDF
    {

        public EstructuraPDF()
        {
            this.Hijos = new List<EstructuraPDF>();
        }
        /// <summary>
        /// (ESP) El Id de nuestro documento
        /// (ENG) the id document
        /// </summary>
        public Guid EstructuraPDFId { get; set; }

        /// <summary>
        /// (ESP) Este será el nombre de nuestro documento
        /// (ENG) The name for our document
        /// </summary>
        public String Nombre { get; set; }

        /// <summary>
        /// (ESP) Será la ruta de donde se encuentre alojado nuestro documento
        /// (ENG) The file url for the document
        /// </summary>
        public String UrlDocumento { get; set; }

        /// <summary>
        /// (ESP) Podrá tener documentos en cascada
        /// (ENG) this element can have child elements
        /// </summary>
        public List<EstructuraPDF> Hijos { get; set; }
    }
}