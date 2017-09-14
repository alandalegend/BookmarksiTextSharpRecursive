using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneracionPDF.Modelo.ViewModels
{
    public class vmGeneracionPDF : _vmBase
    {
        public vmGeneracionPDF()
        {

        }
        /// <summary>
        /// <para>(ESP) Este inicializador prentede obtener información inicial</para>
        /// <para>(ENG) This method set the principal values</para>
        /// </summary>
        /// <param name="nombre">
        /// <para>(ESP) el nombre será el nombre de tu libro</para>
        /// <para>(ENG) the name of your book</para>
        /// </param>
        /// <param name="niveles">
        /// <para>(ESP) será el número de niveles padres</para>
        /// <para>(ENG) set the fathers count</para>
        /// </param>
        /// <param name="subNiveles">
        /// <para>(ESP) Ingresa el número de subniveles</para>
        /// <para>(ENG) Set the child nodes count</para>
        /// </param>
        /// <param name="rutaPdf">
        /// <para>(ESP) Ingresa la ruta donde se enviará tu documento realizado</para>
        /// <para>(ENG) Set the path for your pdf file</para>
        /// </param>
        public vmGeneracionPDF(string nombre, Int32 niveles, Int32 subNiveles, string rutaPdf)
        {
            this.NombreLibroPDF = nombre;
            this.Niveles = niveles;
            this.SubNiveles = subNiveles;
            this.RutaPDF = rutaPdf;
        }


        /// <summary>
        /// <para>(ESP) Este será el nombre de tu libro</para>
        /// <para>(ENG) This will be the name for your new book</para>
        /// </summary>
        [Required(ErrorMessage = "Debes de ingresar el nombre de tu PDF")]
        [Display(Name = "Nombre de tu PDF", Description = "Ingresa el nombre de tu PDF")]
        public String NombreLibroPDF { get; set; }

        /// <summary>
        /// <para>(ESP) Aquí defines cuantos elementos padre deseas tener</para>
        /// <para>(ENG) Set the num fathers </para>
        /// </summary>

        [Range(1, Double.PositiveInfinity, ErrorMessage = "Debes de tener mínimo un nodo padre")]
        [Required(ErrorMessage = "Debes de ingresar un número de nodos padre")]
        [Display(Name = "¿Cuantos nodos padre quieres?", Description = "Ingresa el número de nodos padres deseas tener")]
        public int Niveles { get; set; }


        /// <summary>
        /// <para>(ESP) Ingresa el número de nodos dependientes del padre</para>
        /// <para>(ENG) Set the child nodes count</para>
        /// </summary>
        [Range(1, Double.PositiveInfinity, ErrorMessage = "Debes de tener mínimo un nodo hijo")]
        [Required(ErrorMessage = "Debes de ingresar un número de nodos hijos")]
        [Display(Name = "¿Cuantos nodos hijos deseas tener?", Description = "Ingresa el número de nodos hijos que deseas tener")]
        public Int32 SubNiveles { get; set; }


        /// <summary>
        /// <para>(ESP) Ingresa la ruta de donde quieres que se guarde tu documento</para>
        /// <para>(ENG) set the path for save your document</para>
        /// </summary>
        [Required(ErrorMessage = "Debes de ingresar una ruta dentro de tu equipo, ahí se generarán los archivos")]
        [Display(Name = "Ruta de tu PDF", Description = "Ingresa la ruta donde se guardará tu PDF")]
        public String RutaPDF { get; set; }
    }
}
