using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneracionPDF.Modelo.POCOs
{
    /// <summary>
    /// (ESP) Esta clase delimitará el resultado de alguna transacción
    /// (ENG) This class provide you the result for the transaction
    /// </summary>
    public class Transaccion
    {
        /// <summary>
        /// (ESP) Indicará si la operación fue exitosa
        /// (ENG) This indicate you if the transaction was successful
        /// </summary>
        public Boolean  Exitosa { get; set; }

        /// <summary>
        /// (ESP) Será el mensaje que te arrojará según el resultado de la transacción
        /// (ENG) This will help you, indicate the message for the transaction
        /// </summary>
        public String Mensaje { get; set; }
        public String URL { get; set; }
    }
}
