using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Omini.Opme.Infrastructure.Pdf.QuestPdf;

public static class QuotationPdfStyles
{
    public static IContainer HeaderTableTitleStyle(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten1)
            .Background(Colors.Grey.Lighten1)
            .AlignCenter()
            .AlignMiddle()
            .PaddingHorizontal(4)
            .PaddingVertical(3)
            .DefaultTextStyle(TextStyle.Default.Bold());
    }

    public static IContainer HeaderTableCellStyle(IContainer container)
    {
        return container
            .Background(Colors.Grey.Lighten2)
            .AlignCenter()
            .AlignMiddle();
    }

    public static IContainer LGPDStyle(IContainer container)
    {
        return container
            .DefaultTextStyle(TextStyle.Default.FontSize(9));
    }

    public static IContainer LabelTitleStyle(IContainer container)
    {
        return container
            .DefaultTextStyle(TextStyle.Default.FontColor(Colors.Grey.Medium));
    }

    public static IContainer TextContentStyle(IContainer container)
    {
        return container
            .DefaultTextStyle(TextStyle.Default.FontColor(Colors.Black).Bold());
    }

    public static IContainer ContentTableTitleStyle(IContainer container)
    {
        return container
            .Background(Colors.Grey.Lighten2)
            .Background(Colors.Grey.Lighten2)
            .AlignMiddle()
            .PaddingHorizontal(4)
            .PaddingVertical(3)
            .DefaultTextStyle(TextStyle.Default.Bold());
    }

    public static IContainer ContentTableCellStyle(IContainer container)
    {
        return container
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten1)
            .Background(Colors.White)
            .AlignMiddle()
            .PaddingHorizontal(4)
            .PaddingVertical(0.1f);
    }

    public static IContainer ContentTableFooterStyle(IContainer container)
    {
        return container
            .BorderTop(1)
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten1)
            .Background(Colors.Grey.Lighten2)
            .AlignMiddle();
    }

    public static IContainer ContentTableFooterTotalStyle(IContainer container)
    {
        return container
            .BorderTop(1)
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten1)
            .Background(Colors.White)
            .AlignMiddle()
            .PaddingHorizontal(4)
            .PaddingVertical(0.1f);
    }
}