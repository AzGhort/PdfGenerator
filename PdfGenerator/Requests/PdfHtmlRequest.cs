using System.ComponentModel.DataAnnotations;

namespace PdfGenerator.Requests;

public class PdfHtmlRequest : PdfRequest
{
    public string Header { get; set; } = "<head></head>";

    [Required]
    public string Body { get; set; } = null!;

    public string Footer { get; set; } = "<footer></footer>";
}
