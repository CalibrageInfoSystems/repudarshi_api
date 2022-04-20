using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace Utilities
{
    public class FileRepository
    {
        public byte[] ConvertHtmlToPdf(string HtmlString)
        {

            //Create a byte array that will eventually hold our final PDF
            byte[] bytes;

            //Boilerplate iTextSharp setup here
            //Create a stream that we can write to, in this case a MemoryStream
            using (var ms = new MemoryStream())
            {

                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                using (var doc = new Document())
                {

                    //Create a writer that's bound to our PDF abstraction and our stream
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {

                        //Open the document for writing
                        doc.Open();

                        //Our sample HTML and CSS
                        //var example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";

                        var example_html = HtmlString;

                        //var example_css = @".headline{font-size:200%}";

                        /**************************************************
                         * Example #1                                     *
                         *                                                *
                         * Use the built-in HTMLWorker to parse the HTML. *
                         * Only inline CSS is supported.                  *
                         * ************************************************/

                        //Create a new HTMLWorker bound to our document
                        using (var htmlWorker = new HTMLWorker(doc))
                        {

                            //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
                            using (var sr = new StringReader(example_html))
                            {

                                //Parse the HTML
                                htmlWorker.Parse(sr);
                            }
                        }

                        doc.Close();
                    }
                }

                //After all of the PDF "stuff" above is done and closed but **before** we
                //close the MemoryStream, grab all of the active bytes from the stream
                bytes = ms.ToArray();
            }

            return bytes;
        }

        public string UploadFile(string ModuleName, byte[] Bytes, string Extension, string FolderLocation)
        {
            try
            {
                var now = DateTime.UtcNow;
                //var yearName = now.Year.ToString(); now.ToString("yyyy");
                //var monthName = now.Month.ToString();
                //var dayName = now.Day.ToString();
                string FileName = DateTime.UtcNow.ToString("yyyyMMddhhmmssfff");

                var FilePath = Path.Combine(FolderLocation, ModuleName);

                //FolderLocation += "FolderLocation/";
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                //byte[] ByteArray = null;

                if (Bytes == null)
                    return null;
                else
                {
                    var testFile = Path.Combine(FilePath, FileName + Extension);
                    File.WriteAllBytes(testFile, Bytes);
                    return FileName;
                }
            }

            catch (Exception ex)
            {
                // throw;
                return ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }
        }

    }
}
