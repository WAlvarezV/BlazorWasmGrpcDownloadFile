using Application;
using Grpc.Core;
using Protos;
using System.Threading;
using System.Threading.Tasks;
using static Protos.FileSrv;

namespace BlazorGrpcFiles.Server.GrpcServices
{
    internal class FileServiceGrpc : FileSrvBase
    {
        private readonly IFileService _service;
        public FileServiceGrpc(IFileService service) => _service = service;

        public override async Task DownloadStream(FileRequest request, IServerStreamWriter<Chunk> responseStream, ServerCallContext context)
        => await _service.DownloadStreamAsync(request, responseStream, CancellationToken.None);
    }
}
