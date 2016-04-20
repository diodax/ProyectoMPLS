using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;
using ProyectoMPLS.Controllers;
using ProyectoMPLS.Models.Accounts;
using ProyectoMPLS.Models.Comunicacion;
using ProyectoMPLS.Models.Topologia;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace ProyectoMPLS.Controllers
{
    public class RaspberryController : ApiController
    {
        // GET api/raspberry
        // Just for testing
        [System.Web.Http.AllowAnonymous]
        public IEnumerable<Proyecto> Get()
        {
            string cUsuarioActual = "diodax";
            List<Proyecto> listaProyectos = new List<Proyecto>();
            //listaProyectos = Proyecto.GetListaProyectos(cUsuarioActual);
            return listaProyectos;
        }

        public class InMemoryMultipartFormDataStreamProvider : MultipartStreamProvider
        {
            private NameValueCollection _formData = new NameValueCollection();
            private List<HttpContent> _fileContents = new List<HttpContent>();

            // Set of indexes of which HttpContents we designate as form data
            private Collection<bool> _isFormData = new Collection<bool>();

            /// <summary>
            /// Gets a <see cref="NameValueCollection"/> of form data passed as part of the multipart form data.
            /// </summary>
            public NameValueCollection FormData
            {
                get { return _formData; }
            }

            /// <summary>
            /// Gets list of <see cref="HttpContent"/>s which contain uploaded files as in-memory representation.
            /// </summary>
            public List<HttpContent> Files
            {
                get { return _fileContents; }
            }

            public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
            {
                // For form data, Content-Disposition header is a requirement
                ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
                if (contentDisposition != null)
                {
                    // We will post process this as form data
                    _isFormData.Add(String.IsNullOrEmpty(contentDisposition.FileName));

                    return new MemoryStream();
                }

                // If no Content-Disposition header was present.
                throw new InvalidOperationException(string.Format("Did not find required '{0}' header field in MIME multipart body part..", "Content-Disposition"));
            }

            /// <summary>
            /// Read the non-file contents as form data.
            /// </summary>
            /// <returns></returns>
            public override async Task ExecutePostProcessingAsync()
            {
                // Find instances of non-file HttpContents and read them asynchronously
                // to get the string content and then add that as form data
                for (int index = 0; index < Contents.Count; index++)
                {
                    if (_isFormData[index])
                    {
                        HttpContent formContent = Contents[index];
                        // Extract name from Content-Disposition header. We know from earlier that the header is present.
                        ContentDispositionHeaderValue contentDisposition = formContent.Headers.ContentDisposition;
                        string formFieldName = UnquoteToken(contentDisposition.Name) ?? String.Empty;

                        // Read the contents as string data and add to form data
                        string formFieldValue = await formContent.ReadAsStringAsync();
                        FormData.Add(formFieldName, formFieldValue);
                    }
                    else
                    {
                        _fileContents.Add(Contents[index]);
                    }
                }
            }

            /// <summary>
            /// Remove bounding quotes on a token if present
            /// </summary>
            /// <param name="token">Token to unquote.</param>
            /// <returns>Unquoted token.</returns>
            private static string UnquoteToken(string token)
            {
                if (String.IsNullOrWhiteSpace(token))
                {
                    return token;
                }

                if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
                {
                    return token.Substring(1, token.Length - 2);
                }

                return token;
            }
        }

        [System.Web.Http.Route("api/raspberry")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> PostFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/uploads");
            //var provider = new MultipartFormDataStreamProvider(root);
            //var provider = new MultipartMemoryStreamProvider();
            //var provider = new MultipartFormDataStreamProvider<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());

            //await Request.Content.ReadAsMultipartAsync(provider);

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());

            //access form data
            NameValueCollection formData = provider.FormData;

            //access files
            IList<HttpContent> files = provider.Files;

            string userName = formData.GetValues("cUserName")[0];
            string projectName = formData.GetValues("cFileName")[0];

            // This illustrates how to get the file names.
            //foreach (MultipartFileData file in provider.FileData)
            //{
            //    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
            //    Trace.WriteLine("Server file path: " + file.LocalFileName);
            //}

            // Show all the key-value pairs.
            //foreach (var key in provider.FormData.AllKeys)
            //{
            //    foreach (var val in provider.FormData.GetValues(key))
            //    {
            //        Debug.WriteLine(string.Format("{0}: {1}", key, val));
            //    }
            //}

            //provider.Contents[0] --> Archivo CSV
            //provider.Contents[1] --> cUserName
            //provider.Contents[2] --> cFileName (Nombre del proyecto)
            //var memFile = provider.Contents[0];
            //string filename = memFile.Headers.ContentDisposition.FileName.Trim('\"');
            //byte[] buffer = await memFile.ReadAsByteArrayAsync();

            //var userNameContainer = provider.Contents[1].ReadAsFormDataAsync();
            //var userName = userNameContainer.GetValues("cUserName");
            //var projectName = provider.Contents[2].ToString();

            //byte[] buffer;
            string fileString = await provider.Files[0].ReadAsStringAsync();
            //Stream stream = new MemoryStream();
            //await provider.Files[0].CopyToAsync(stream);

            byte[] byteArray = Convert.FromBase64String(fileString);
            MemoryStream myStream = new MemoryStream(byteArray);

            //Proceso de parsing del archivo CSV
            List<Tabla> tablaDatos = new List<Tabla>();
            StreamReader csvreader = new StreamReader(myStream);

            //Primera linea
            var line = csvreader.ReadLine();
            var values = line.Split(';');

            while (!csvreader.EndOfStream)
            {
                Tabla row = new Tabla();
                line = csvreader.ReadLine();
                values = line.Split(';');

                row.Hostname = values[0];
                row.OSPFRouterID = values[1];
                row.OSPFNeighborRouterID = values[2];
                row.OSPFNeighborIP = values[3];
                tablaDatos.Add(row);
            }

            ////Llama al SP para crear el nuevo proyecto en la DB
            //Data.dsTopologiaTableAdapters.ProyectosTableAdapter Adapter = new Data.dsTopologiaTableAdapters.ProyectosTableAdapter();
            //int idProyecto = (int)Adapter.CrearProyectoLocal(cUsername, cFileName);

            ////Creación del nuevo objeto de proyecto
            //Proyecto newProyecto = new Proyecto(idProyecto);
            //newProyecto.GenerarTopologia(tablaDatos);
            //return Request.CreateResponse(HttpStatusCode.OK);

            //File.Delete(@"~/App_Data/uploads/" + fil);

            return Ok();
        }
        // POST api/raspberry
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Post([FromBody]HttpPostedFileBase fArchivoCSV, string cUsername, string cFileName)
        {
            if (fArchivoCSV == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            try
            {
                //Llama al SP para crear el nuevo proyecto en la DB
                Data.dsTopologiaTableAdapters.ProyectosTableAdapter Adapter = new Data.dsTopologiaTableAdapters.ProyectosTableAdapter();
                int idProyecto = (int)Adapter.CrearProyectoLocal(cUsername, cFileName);

                //Proceso de parsing del archivo CSV
                List<Tabla> tablaDatos = new List<Tabla>();
                StreamReader csvreader = new StreamReader(fArchivoCSV.InputStream);    // Use the InputStream to get the actual stream sent.

                //Primera linea
                var line = csvreader.ReadLine();
                var values = line.Split(';');

                while (!csvreader.EndOfStream)
                {
                    Tabla row = new Tabla();
                    line = csvreader.ReadLine();
                    values = line.Split(';');

                    row.Hostname = values[0];
                    row.OSPFRouterID = values[1];
                    row.OSPFNeighborRouterID = values[2];
                    row.OSPFNeighborIP = values[3];
                    tablaDatos.Add(row);
                }

                //Creación del nuevo objeto de proyecto
                Proyecto newProyecto = new Proyecto(idProyecto);
                newProyecto.GenerarTopologia(tablaDatos);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }

    public class ApiLoginController : ApiController
    {
        // POST api/apilogin
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Post(string cUsername, string cHashedPassword)
        {
            try
            {
                if (Usuario.EsLoginValido(cUsername, cHashedPassword))
                {
                    FormsAuthentication.SetAuthCookie(cUsername, false);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "El nombre de usuario o contraseña están incorrectos.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}