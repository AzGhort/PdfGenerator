using Gotenberg.Sharp.API.Client;
using Gotenberg.Sharp.API.Client.Domain.Builders;
using Gotenberg.Sharp.API.Client.Domain.Builders.Faceted;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PdfGenerator.ActionResults;
using PdfGenerator.Options;
using PdfGenerator.Requests;
using System.Net.Mime;

namespace PdfGenerator.Controllers;

[Route("api/v1/pdf")]
[ApiController]
public class PdfController : ControllerBase
{
    private readonly GotenbergSharpClient fClient;

    public PdfController(IOptions<PdfOptions> pdfOptions)
    {
        fClient = new GotenbergSharpClient(pdfOptions.Value.GeneratorUrl);
    }

    [HttpPost("html")]
    public async Task<IActionResult> PostFromHtml([FromBody] PdfHtmlRequest pdfTemplate, CancellationToken cancellationToken)
    {
        var builder = new HtmlRequestBuilder(false)
         .AddDocument(x =>
           {
               x.SetBody(pdfTemplate.Body);
               x.SetHeader(pdfTemplate.Header);
               x.SetFooter(pdfTemplate.Footer);
           })
         .WithDimensions(x =>
           {
               x.SetPaperSize(PaperSizes.A4)
                .SetMargins(Margins.Normal)
                .SetScale(.99);
           })
         .ConfigureRequest(x =>
           {
               x.ChromeRpccBufferSize(1024)
                  .PageRanges("1");
           });

        var request = await builder.BuildAsync();

        var stream = await fClient.HtmlToPdfAsync(request, cancellationToken);

        return new StreamActionResult(stream, MediaTypeNames.Application.Octet, pdfTemplate.Name ?? $"{Guid.NewGuid()}.pdf");
    }

    [HttpPost("url")]
    public async Task<IActionResult> PostFromUrl([FromBody] PdfUrlRequest pdfTemplate, CancellationToken cancellationToken)
    {
        var builder = new UrlRequestBuilder()
         .SetUrl(pdfTemplate.Url)
         .ConfigureRequest(x =>
         {
             x.ChromeRpccBufferSize(1024)
                .PageRanges("1");
         });

        var request = await builder.BuildAsync();

        var stream = await fClient.UrlToPdfAsync(request, cancellationToken);

        return new StreamActionResult(stream, MediaTypeNames.Application.Octet, pdfTemplate.Name ?? $"{Guid.NewGuid()}.pdf");
    }
}
