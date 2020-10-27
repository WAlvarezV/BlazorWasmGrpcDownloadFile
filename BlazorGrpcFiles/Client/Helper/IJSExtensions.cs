using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGrpcFiles.Client.Helper
{
    public static class IJSExtensions
    {
        public static ValueTask<object> SaveFileAs(this IJSRuntime js, string fileName, byte[] file)
            => js.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(file));

    }
}
