using System.ComponentModel.DataAnnotations;

namespace PdfGenerator.Options;

public sealed class PdfOptions
{
    public const string _SectionKey = "Pdf";

    [Required]
    public string GeneratorUrl { get; set; } = null!;
}
