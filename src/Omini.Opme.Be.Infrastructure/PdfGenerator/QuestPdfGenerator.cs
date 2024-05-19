using QuestPDF.Drawing;

namespace Omini.Opme.Be.Infrastructure;

public static class QuestPdfGenerator
{
    public static void RegisterFontsFromPath(string path)
    {
        var fontPaths = Directory.GetFiles(path);

        foreach (var fontPath in fontPaths)
        {
            FontManager.RegisterFont(File.OpenRead(fontPath));
        }
    }
}