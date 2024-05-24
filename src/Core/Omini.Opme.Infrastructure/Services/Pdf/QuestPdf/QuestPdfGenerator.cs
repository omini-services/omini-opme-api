using Microsoft.Extensions.Logging;
using QuestPDF.Drawing;

namespace Omini.Opme.Infrastructure.Pdf.QuestPdf;

public static class QuestPdfConfiguration
{
    public static void RegisterFontsFromPath(string path, ILogger logger)
    {
        if (!Directory.Exists(path))
        {
            logger.LogWarning("Font directory not found - {0}", path);
            return;
        }
        
        var fontPaths = Directory.GetFiles(path);

        foreach (var fontPath in fontPaths)
        {
            FontManager.RegisterFont(File.OpenRead(fontPath));
        }
    }
}