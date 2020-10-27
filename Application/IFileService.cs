using Grpc.Core;
using Protos;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    internal interface IFileService
    {
        Task DownloadStreamAsync(FileRequest fileRequest, IServerStreamWriter<Chunk> responseStream, CancellationToken cancelToken);
    }
}
