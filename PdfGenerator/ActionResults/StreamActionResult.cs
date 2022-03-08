using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace PdfGenerator.ActionResults;

public class StreamActionResult : IActionResult
{
    private const string AttachmentHeader = "attachment";

    private readonly string _mediaType;
    private readonly string _fileName;
    private readonly Stream _stream;

    public StreamActionResult(Stream objectsStream, string mediaType, string fileName)
    {
        _mediaType = mediaType;
        _fileName = fileName;
        _stream = objectsStream;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var httpContext = context.HttpContext;

        httpContext.Response.ContentType = _mediaType;

        var contentDisposition = new ContentDispositionHeaderValue(AttachmentHeader);
        contentDisposition.SetHttpFileName(_fileName);
        httpContext.Response.Headers.ContentDisposition = contentDisposition.ToString();

        var syncIoFeature = httpContext.Features.Get<IHttpBodyControlFeature>();
        syncIoFeature.AllowSynchronousIO = true;

        using var writer = new BinaryWriter(httpContext.Response.Body);
        using var ms = new MemoryStream();

        await _stream.CopyToAsync(ms);

        writer.Write(ms.ToArray());
    }
}
