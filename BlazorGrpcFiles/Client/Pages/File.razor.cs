using BlazorGrpcFiles.Client.Helper;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Protos;
using System;
using System.Text;
using System.Threading.Tasks;
using static Protos.FileSrv;

namespace BlazorGrpcFiles.Client.Pages
{
    public partial class FileBase : ComponentBase
    {
        [Inject] FileSrvClient FileClient { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }

        public async Task DownloadFile()
        {
            var fileName = "G:\\test 42mb.pdf";
            using var call = FileClient.DownloadStream(new FileRequest { Path = fileName });
            try
            {
                var result = new StringBuilder();
                Console.WriteLine("Descargando archivo.");
                await foreach (var message in call.ResponseStream.ReadAllAsync())
                {
                    result.Append(Encoding.UTF8.GetString(message.Content.ToByteArray()));
                }
                await JSRuntime.SaveFileAs("nuevoDescargado.pdf", Convert.FromBase64String(result.ToString()));
                Console.WriteLine("Descargó archivo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DownloadFile Exception => Message {ex.Message} - InnerExceptionMessage {(ex.InnerException != null ? ex.InnerException.Message : "No InnerException")}");
            }
        }
    }
}
