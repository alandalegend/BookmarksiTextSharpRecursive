using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GeneracionPDF.Negocio.CreatePDF
{
    /// <summary>
    /// <para>(ESP) Esta clase tendrá un método que permite crear los documentos</para>
    /// <para>(ENG) This class will have a method that allows you to create the documents</para>
    /// </summary>
    public class CreatePDF
    {
        /// <summary>
        /// <para>(ESP) Este metodo te ayudará a generar los PDFs, realmente este es el metodo que genera toda la estructura del documento y te entrega el documento pdf con todos tus archivos adjuntos, y tu barra de marcadores</para>
        /// <para>(ENG) this method provide you the final PDF, with all your PDF's, and the bookmark</para>
        /// </summary>
        /// <param name="nombreLibroPDF">
        /// <para>(ESP> Será el nombre de tu libro</para>
        /// <para>(ENG) Set the name for your pdf file</para>
        /// </param>
        /// <param name="niveles">
        /// <para>(ESP) Define el número de niveles padres</para>
        /// <para>(ENG) Set the father's number</para>
        /// </param>
        /// <param name="subniveles">
        /// <para>(ESP) Define el número de sub-niveles </para>
        /// <para>(ENG) Set the child's number</para>
        /// </param>
        /// <param name="rutaPDFs">
        /// <para>(ESP) Define la ruta donde se creará el documento PDF</para>
        /// <para>(ENG) Set the path for create your pdf file</para>
        /// </param>
        /// <returns></returns>
        public Modelo.POCOs.Transaccion CrearPDF(String nombreLibroPDF, Int32 niveles, Int32 subniveles, string rutaPDFs)
        {
            Modelo.POCOs.Transaccion resultado = new Modelo.POCOs.Transaccion();
            try
            {
                //(ESP) Valida si la ruta ingresada existe
                //(ENG) Validate the path, if isn't exist, create it.
                if (!System.IO.Directory.Exists(rutaPDFs))
                {
                    System.IO.Directory.CreateDirectory(rutaPDFs);
                }

                //(ESP) Crea el documento inicial
                ///(ENG) Create the document
                Document doc = new Document(PageSize.A4);

                using (FileStream stream = new FileStream(rutaPDFs+"/"+nombreLibroPDF + ".pdf", FileMode.Create))
                {
                    PdfSmartCopy pdfCopy = new PdfSmartCopy(doc, stream);

                    doc.Open();

                    // (ESP) Ingresa las propiedades de tu documento
                    // (ENG) Add meta information to the document
                    doc.AddAuthor("daLegend.net");
                    doc.AddCreator("Ejemplo para crear documentos PDF en iTextSharp");
                    doc.AddKeywords("PDF iTextSharp, Bookmarks");
                    doc.AddSubject("Te indicará como crear pdfs y como crear el bookmark para este ejemplo");
                    doc.AddTitle("daLegend - Crea bookmarks en pdf");



                    //(ESP) Se declara la raiz del libro y se crea el marcador para la raíz
                    //(ENG) Declare the root of the book, and create the bookmark
                    PdfOutline root = null; root = pdfCopy.RootOutline; root.Title = nombreLibroPDF;
                    PdfOutline marcadorNombreLibro = new PdfOutline(root, PdfAction.GotoLocalPage(nombreLibroPDF, false), nombreLibroPDF);

                    //(ESP) Obtiene todos los documentos del libro
                    //(ENG) Get the files for your book
                    var streams = GetAll(niveles, subniveles, rutaPDFs);

                    //(ESP) Genera el documento, haciendo todo esto recurrente, asigna el documento parent
                    //Generate the document, this is recursive, assing the parent document
                    foreach (var percorsoFilePdf in streams)
                    {
                        GenerarHijosEstructura(doc, pdfCopy, marcadorNombreLibro, percorsoFilePdf, rutaPDFs);
                    }
                    //Clean up
                    doc.Close();

                    resultado.Exitosa = true;
                    resultado.Mensaje = "Se ha creado el documento " + nombreLibroPDF + ", puedes visualizarlo en la carpeta designada para recopilar tus documentos";
                    resultado.URL = rutaPDFs + "/" + nombreLibroPDF + ".pdf";
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
            return resultado;
        }


        /// <summary>
        /// Este metodo obtendrá la estructura de tu libro, su contenido son "n" niveles, esto es por simple ejemplo
        /// </summary>
        /// <param name="niveles">
        /// (ESP) Será el número de padres que contendrá el libro
        /// (ENG) the parent numbers
        /// </param>
        /// <param name="subniveles">
        /// (ESP) Será el número de hijos
        /// (ENG) Number for the child items
        /// </param>
        /// <param name="rutaPDFs">
        /// (ESP) Será la ruta donde se guardarán los archivos
        /// (ENG) the path for save the files
        /// </param>
        /// <returns></returns>
        public List<Modelo.POCOs.EstructuraPDF> GetAll(Int32 niveles, Int32 subniveles, string rutaPDFs)
        {
            List<Modelo.POCOs.EstructuraPDF> resultado = new List<Modelo.POCOs.EstructuraPDF>();
            try
            {
              
                for (Int32 i = 1; i <= niveles; i++)
                {
                    //(ESP) Creamos los nodos padres
                    //(ENG) Create the fathers items
                    string nombrePadre = "Dcto" + i.ToString() + ".pdf";
                    string rutaDcto = String.Format("{0}/{1}", rutaPDFs, nombrePadre);

                    //(ESP) se crean los archivos con itextSharp
                    //(ENG) creation for the fathers items, with itextsharp
                    var creacionDocumento = CrearDocumentos(nombrePadre, rutaDcto, nombrePadre);


                    var padre = new Modelo.POCOs.EstructuraPDF
                    {
                        EstructuraPDFId = Guid.NewGuid(),
                        Nombre = nombrePadre,
                        UrlDocumento =creacionDocumento.Mensaje
                    };
                   

                    for (Int32 h = 1; h <= subniveles; h++)
                    {
                        //(ESP) Creamos los nodos hijos
                        //(ENG) Create the sons items
                        string nombreHijo = "DctoHijo" + i.ToString() + "-" + h.ToString() + ".pdf";
                        string rutaDctoHijo = String.Format("{0}/{1}", rutaPDFs ,nombreHijo);

                        //(ESP) se crean los archivos hijos con itextSharp
                        //(ENG) creation for the sons items, with itextsharp
                        var creacionDocumentoHijo = CrearDocumentos(rutaDctoHijo, rutaPDFs, nombreHijo);
                        var hijo = new Modelo.POCOs.EstructuraPDF
                        {
                            EstructuraPDFId = Guid.NewGuid(),
                            Nombre = nombreHijo,
                            UrlDocumento =  creacionDocumentoHijo.Mensaje
                        };
                        
                        


                        Random numeroDeHijosRandom = new Random();
                        Int32 hijos = numeroDeHijosRandom.Next(1, 10);

                        for (Int32 s = 1; s <= hijos; s++)
                        {
                            //(ESP) Creamos los nodos nietos
                            //(ENG) Create the grandsons items
                            string nombreNieto = "DctoNieto" + s.ToString() + "-" + s.ToString() + ".pdf";
                            string rutaDctoNieto = String.Format("{0}/{1}", rutaPDFs, nombreNieto);

                            //(ESP) se crean los archivos nietos con itextSharp
                            //(ENG) creation for the grandsons items, with itextsharp
                            var creacionDocumentoNieto = CrearDocumentos(rutaDctoNieto, rutaPDFs, nombreNieto);


                            var nieto = new Modelo.POCOs.EstructuraPDF
                            {
                                EstructuraPDFId = Guid.NewGuid(),
                                Nombre = nombreNieto,
                                UrlDocumento = creacionDocumentoNieto.Mensaje
                            };
                          
                            hijo.Hijos.Add(nieto);
                        }
                        padre.Hijos.Add(hijo);
                    }
                    resultado.Add(padre);
                }
            }
            catch (Exception error)
            {

            }
            return resultado;
        }


        /// <summary>
        /// <para>(ESP) Este metodo es recursivo, se encarga de asignar los bookmarks y asignarles su parent</para>
        /// <para>(ENG) this method is recursive, assign the parent for each file</para>
        /// </summary>
        /// <param name="nombreDocumento"></param>
        /// <param name="rutaPDFs"></param>
        /// <param name="contenido"></param>
        /// <returns></returns>
        public Modelo.POCOs.Transaccion CrearDocumentos(string nombreDocumento, string rutaPDFs, string contenido)
        {
            Modelo.POCOs.Transaccion resultado = new Modelo.POCOs.Transaccion();
            try
            {

                System.IO.FileStream fs = new FileStream(nombreDocumento, FileMode.Create);
                // Create an instance of the document class which represents the PDF document itself.
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                // Create an instance to the PDF file by creating an instance of the PDF
                // Writer class using the document and the filestrem in the constructor.

                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                // Add meta information to the document
                document.AddAuthor("daLegend.net");
                document.AddCreator("Ejemplo para crear documentos PDF en iTextSharp");
                document.AddKeywords("PDF iTextSharp, Bookmarks");
                document.AddSubject("Te indicará como crear pdfs y como crear el bookmark para este ejemplo");
                document.AddTitle("daLegend - Crea bookmarks en pdf");

                // Open the document to enable you to write to the document
                document.Open();


                ///this is completly irrelevant, its my pdf format
                #region formato del PDFs
                float[] ancho = new float[] { 20, 60, 20 };
                float[] anchoEncabezado = new float[] { 20.5f, 0.5f, 60, 20 };
                var romanoTitulo = String.Empty;

                PdfPTable tableTitulo = new PdfPTable(ancho.Length);
                float anchoPorLetra = 1;
                float matriz = 0, espacio = 0;
                PdfPTable tableEncabezado = new PdfPTable(1);
                float[] anchoEncabezadoTitulo = new float[] { 20.5f, 0.5f, 79 };
                PdfPTable tableEncabezadoTitulo = new PdfPTable(anchoEncabezadoTitulo.Length);


                #region tabla de Titulo
                ancho = new float[] { 20, 60, 20 };
                tableTitulo = new PdfPTable(ancho.Length);
                tableTitulo.SpacingBefore = 30f;
                tableTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
                tableTitulo.SetTotalWidth(ancho);
                tableTitulo.DefaultCell.Border = PdfPCell.NO_BORDER;



                tableTitulo.AddCell(new PdfPCell()
                {
                    Border = PdfPCell.NO_BORDER,
                });


                tableTitulo.AddCell(new PdfPCell()
                {
                    FixedHeight = 80f,
                    Border = PdfPCell.NO_BORDER,
                    Image = Image.GetInstance("http://dalegend.net/Content/favicon/android-chrome-144x144.png"),

                    //Phrase = new iTextSharp.text.Phrase(caratula.Titulo, fuenteElegida(12, true, new BaseColor(caratula.ColorLetraTitulo))),
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

                tableTitulo.AddCell(new PdfPCell()
                {
                    Border = PdfPCell.NO_BORDER,
                });

                tableTitulo.SpacingAfter = 10f;
                tableTitulo.CompleteRow();
                document.Add(tableTitulo);
                #endregion

                #region tabla de Encabezado

                anchoPorLetra = Convert.ToInt32("daLegend.Net".Length * 3.5);
                matriz = anchoPorLetra + 21;
                espacio = 100 - matriz;

                anchoEncabezado = new float[] { 20.5f, 0.5f, anchoPorLetra, espacio };


                tableEncabezado = new PdfPTable(anchoEncabezado.Length);
                tableEncabezado.SpacingBefore = 45f;
                tableEncabezado.HorizontalAlignment = Element.ALIGN_LEFT;
                tableEncabezado.SetTotalWidth(anchoEncabezado);
                tableEncabezado.DefaultCell.Border = PdfPCell.NO_BORDER;


                tableEncabezado.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(223, 106, 19), Border = PdfPCell.NO_BORDER, FixedHeight = 47f, VerticalAlignment = Element.ALIGN_BOTTOM });
                tableEncabezado.AddCell(new PdfPCell() { BackgroundColor = iTextSharp.text.BaseColor.WHITE, Border = PdfPCell.NO_BORDER });
                tableEncabezado.AddCell(new PdfPCell() { BackgroundColor = BaseColor.GRAY, Phrase = new iTextSharp.text.Phrase("daLegend.Net", FontFactory.GetFont("Arial", 28, BaseColor.WHITE)), Border = PdfPCell.NO_BORDER, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT });
                tableEncabezado.AddCell(new PdfPCell() { Border = PdfPCell.NO_BORDER });

                tableEncabezado.CompleteRow();
                document.Add(tableEncabezado);


                anchoEncabezadoTitulo = new float[] { 20.5f, 0.5f, 79 };
                tableEncabezadoTitulo = new PdfPTable(anchoEncabezadoTitulo.Length);
                tableEncabezadoTitulo.SpacingBefore = 25f;

                tableEncabezadoTitulo.HorizontalAlignment = Element.ALIGN_LEFT;
                tableEncabezadoTitulo.SetTotalWidth(anchoEncabezadoTitulo);
                tableEncabezadoTitulo.DefaultCell.Border = PdfPCell.NO_BORDER;

                tableEncabezadoTitulo.AddCell(new PdfPCell() { Border = PdfPCell.NO_BORDER });
                tableEncabezadoTitulo.AddCell(new PdfPCell() { Border = PdfPCell.NO_BORDER });

                tableEncabezadoTitulo.AddCell(new PdfPCell() { HorizontalAlignment = Element.ALIGN_LEFT, Phrase = new iTextSharp.text.Phrase("DaLegend.net", FontFactory.GetFont("Arial", 28, BaseColor.WHITE)), BorderColorLeft = BaseColor.WHITE, BorderColorBottom = BaseColor.WHITE, BorderColorTop = BaseColor.WHITE, BorderColorRight = BaseColor.WHITE, BorderWidthRight = 0, BorderWidthTop = 0 });

                tableEncabezadoTitulo.CompleteRow();
                document.Add(tableEncabezadoTitulo);

                anchoEncabezadoTitulo = new float[] { 20.5f, 0.5f, 79 };
                tableEncabezadoTitulo = new PdfPTable(anchoEncabezadoTitulo.Length);
                tableEncabezadoTitulo.HorizontalAlignment = Element.ALIGN_LEFT;
                tableEncabezadoTitulo.SetTotalWidth(anchoEncabezadoTitulo);
                tableEncabezadoTitulo.DefaultCell.Border = PdfPCell.NO_BORDER;

                tableEncabezadoTitulo.AddCell(new PdfPCell() { Border = PdfPCell.NO_BORDER });
                tableEncabezadoTitulo.AddCell(new PdfPCell() { Border = PdfPCell.NO_BORDER });

                tableEncabezadoTitulo.AddCell(new PdfPCell() { FixedHeight = 20f, BorderColorLeft = BaseColor.WHITE, BorderColorBottom = BaseColor.BLACK, BorderColorTop = BaseColor.WHITE, BorderColorRight = BaseColor.WHITE, BorderWidthRight = 0, BorderWidthTop = 0 });

                tableEncabezadoTitulo.CompleteRow();
                tableEncabezadoTitulo.SpacingAfter = 20f;
                document.Add(tableEncabezadoTitulo);
                #endregion

                #region tabla de Contenido
                float[] anchoContenido = new float[] { 25, 50, 25 };
                PdfPTable tableContenido = new PdfPTable(anchoContenido.Length);
                tableContenido.HorizontalAlignment = Element.ALIGN_LEFT;
                tableContenido.SetTotalWidth(anchoContenido);
                tableContenido.DefaultCell.Border = PdfPCell.NO_BORDER;
                tableContenido.AddCell(new PdfPCell() { Border = PdfPCell.NO_BORDER, FixedHeight = 10f });
                tableContenido.AddCell(new PdfPCell()
                {
                    Phrase = new Phrase(new Chunk("Esta es la sección " + contenido)),
                    Border = PdfPCell.NO_BORDER,
                    VerticalAlignment = Element.ALIGN_BOTTOM,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });
                tableContenido.CompleteRow();
                document.Add(tableContenido);
                #endregion

                #endregion
                // Close the document
                document.Close();
                // Close the writer instance
                writer.Close();
                // Always close open filehandles explicity
                fs.Close();
                resultado.Exitosa = true;
                resultado.Mensaje = nombreDocumento;
            }
            catch (Exception error)
            {

            }
            return resultado;
        }


        /// <summary>
        /// <para>(ESP) Este metodo es recursivo, se encarga de asignar los bookmarks y asignarles su parent</para>
        /// <para>(ENG) this method is recursive, assign the parent for each file</para>
        /// </summary>
        /// <param name="doc">
        /// <para>(ESP) Este es el documento actual</para>
        /// <para>(ENG) this is the current document</para>
        /// </param>
        /// <param name="pdfCopy">
        /// <para>(ESP)es el documento que hará el smart copy</para>
        /// <para>(ENG) this is the pdfsmartcopy for the append another pdf</para>
        /// </param>
        /// <param name="marcadorNombreLibro">
        /// <para>(ESP) este es el parent</para>
        /// <para>(ENG) this is the parent</para>
        /// </param>
        /// <param name="enlace">
        /// <para>(ESP) Esta es la estructura del documento, donde contiene nombre y url del documento</para>
        /// <para>(ENG) This is the structure's document, this contains the name and url's file </para>
        /// </param>
        /// <param name="rutaPDFs">
        /// <para>(ESP)Es donde se generan los documentos</para>
        /// <para>(ENG)Its the path for create the files</para>
        /// </param>
        public static void GenerarHijosEstructura(Document doc, PdfSmartCopy pdfCopy, PdfOutline marcadorNombreLibro, Modelo.POCOs.EstructuraPDF enlace, string rutaPDFs)
        {
            try
            {
                PdfContentByte pb = pdfCopy.DirectContent;

                //Crea el link para la sección
                Anchor section1 = new Anchor(enlace.Nombre) { Name = enlace.Nombre };

                Paragraph psecton1 = new Paragraph();
                psecton1.Add(section1);

                //mostrar la sección 1 en una ubicación específica del documento
                ColumnText.ShowTextAligned(pb, Element.ALIGN_LEFT, psecton1, 36, PageSize.A4.Height - 100, 0);

                
                //(ESP) Se crea el marcador para este documento, se hace referencia al documento padre (parent)
                //(ENG) create the bookmark for this document, and create the reference with the parent
                var mbot = new PdfOutline(marcadorNombreLibro, PdfAction.GotoLocalPage(enlace.Nombre, false), enlace.Nombre);

                //(ESP) Se lee el documento
                //(ENG) read the file
                PdfReader reader = new PdfReader(enlace.UrlDocumento);
                //(ESP) Se adjuntan las paginas al documento
                //(ENG) Copy each page in the current pdfcopy
                for (int I = 1; I <= reader.NumberOfPages; I++)
                {
                    doc.SetPageSize(reader.GetPageSizeWithRotation(1));
                    PdfImportedPage page = pdfCopy.GetImportedPage(reader, I);
                    pdfCopy.AddPage(page);
                }
                //Clean up
                pdfCopy.FreeReader(reader);
                reader.Close();

                if (enlace.Hijos.Any())
                {
                    foreach (var cadaHijo in enlace.Hijos)
                    {
                        //(ESP) aquí está la clave, esto es recurisvo
                        //(ENG) this is the magic, its recursive
                        GenerarHijosEstructura(doc, pdfCopy, mbot, cadaHijo, rutaPDFs);
                    }
                }
            }
            catch (Exception error)
            {

            }

            
        }
    }
}
