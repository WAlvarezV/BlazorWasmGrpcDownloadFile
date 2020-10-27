using Google.Protobuf;
using Grpc.Core;
using Protos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    internal class FileService : IFileService

    {
        public async Task DownloadStreamAsync(FileRequest fileRequest, IServerStreamWriter<Chunk> responseStream, CancellationToken cancelToken)
        {
            var bytes = File.ReadAllBytes(fileRequest.Path);
            var strBase64 = Convert.ToBase64String(bytes);
            var maxLength = 1024 * 1024;
            var chunkedStr = SplitByLength(strBase64, maxLength);
            foreach (var item in chunkedStr)
            {
                var byteString = ByteString.CopyFrom(Encoding.ASCII.GetBytes(item));
                await responseStream.WriteAsync(new Chunk { Content = byteString });
            }
        }

        private static IEnumerable<string> SplitByLength(string strBase64, int maxLength)
        {
            for (int i = 0; i < strBase64.Length; i += maxLength)
            {
                yield return strBase64.Substring(i, Math.Min(maxLength, strBase64.Length - i));
            }
        }
    }
}
