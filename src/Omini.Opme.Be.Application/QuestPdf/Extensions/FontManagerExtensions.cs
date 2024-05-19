using QuestPDF.Drawing;

namespace Omini.Opme.Be.Application.QuestPdf.Extensions;

public static class FontManagerExtensions
{
    public static void RegisterFromPath(string path)
    {
        var fontPaths = Directory.GetFiles(path);

        foreach (var fontPath in fontPaths)
        {
            FontManager.RegisterFont(File.OpenRead(fontPath));
        }
    }
}