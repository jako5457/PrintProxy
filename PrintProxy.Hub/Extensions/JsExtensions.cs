using Microsoft.AspNetCore.Components.Web.Infrastructure;
using Microsoft.JSInterop;

namespace PrintProxy.Hub.Extensions;

public static class JsExtensions
{
    public static async Task CreateDownloadFile(this IJSRuntime js,string Content,string FileName)
    {
       await js.InvokeVoidAsync("downloadFile", FileName, Content);
    }
}