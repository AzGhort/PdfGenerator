using System.ComponentModel.DataAnnotations;

namespace PdfGenerator.Requests;

public class PdfUrlRequest : PdfRequest
{
    [Required]
    public string Url { get; set; } = null!;
}
