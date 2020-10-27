using BlazorGrpcFiles.Client.Helper;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Protos;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorDownloadFile;
using Microsoft.Extensions.Options;
using static Protos.FileSrv;
using System.IO;

namespace BlazorGrpcFiles.Client.Pages
{
    public partial class FileBase : ComponentBase
    {
        
        [Inject] IBlazorDownloadFileService BlazorDownloadFileService { get; set; }
        [Inject] FileSrvClient FileClient { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        

        public async Task DownloadFile()
        {
            var fileName = "/Users/themakers/Documents/PREDICAS/archivo1.PDF";
            using var call = FileClient.DownloadStream(new FileRequest { Path = fileName });
            try
            {
                var result = new StringBuilder();
                Console.WriteLine("Descargando archivo.");
                await foreach (var message in call.ResponseStream.ReadAllAsync())
                {
                    result.Append(Encoding.UTF8.GetString(message.Content.ToByteArray()));
                }
               // byte[] file = Encoding.ASCII.GetBytes(result.ToString());
             //  await JSRuntime.SaveFileAs("nuevoDescargado.pdf", Convert.FromBase64String(result.ToString()));
                await JSRuntime.SaveFileAs("nuevoDescargado.pdf",Convert.FromBase64String(result.ToString()) );
                
               Console.WriteLine("Descargó archivo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DownloadFile Exception => Message {ex.Message} - InnerExceptionMessage {(ex.InnerException != null ? ex.InnerException.Message : "No InnerException")}");
            }
        }
        
        public async Task OnClickDownloadViaHttpClientButton()
        {
            // Please imagine the situation that the downloading files API is protected by
            // token-based authorization (not cookie-based authorization).
            // In that case, the user can not download it from the href link of the anchor tag directly.
            // In this scenario, the application has to get a byte array of the file from
            // the API endpoint by HttpClient with token and make the byte array can be downloadable.
            var fileName = "/Users/themakers/Documents/PREDICAS/archivo1.PDF";//"/Users/themakers/Documents/PREDICAS/Nivel Biblico 1.pdf";
            using var call = FileClient.DownloadStream(new FileRequest { Path = fileName });
           // var bytes = await HttpClient.GetByteArrayAsync("pictures/1");
            try
            {
                var result = new StringBuilder();
                Console.WriteLine("Descargando archivo.");
                await foreach (var message in call.ResponseStream.ReadAllAsync())
                {
                    result.Append(Encoding.UTF8.GetString(message.Content.ToByteArray()));
                }
               // byte[] file = Encoding.ASCII.GetBytes(result.ToString());
                
                
            
                Console.WriteLine("iniciando descarga local archivo.");

               // int bufferSize =65536;// 32768; new MemoryStream(Convert.FromBase64String(Base64Nyan)), "application/octet-stream"))>Dowload Nyan From Stream</button>
               // string contentType = "application/octet-stream";
                await  BlazorDownloadFileService.DownloadFile("nombrearchivo.pdf",
                    new MemoryStream(Convert.FromBase64String(result.ToString())), "application/pdf");
             //   await JSRuntime.SaveAs( fileName, Convert.FromBase64String(result.ToString()));
                Console.WriteLine("termina descarga ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DownloadFile Exception => Message {ex.Message} - InnerExceptionMessage {(ex.InnerException != null ? ex.InnerException.Message : "No InnerException")}");
            }

            
        }
    }
}
