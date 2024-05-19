using System.Reflection;
using Omini.Opme.Be.Application.QuestPdf.Default;
using Omini.Opme.Be.Application.QuestPdf.Extensions;
using Omini.Opme.Be.Domain.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace Omini.Opme.Be.Application.Services;

public sealed class QuotationPdfGenerator : IPdfGenerator
{
    const string DefaultFont = "Noto Sans";
    private readonly string _basePath;
    private readonly string _fontsPath;
    private readonly string _imagesPath;
    private readonly string _pdfLogoFullPath;

    public QuotationPdfGenerator()
    {
        _basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _fontsPath = Path.Combine(_basePath, "Fonts");
        _imagesPath = Path.Combine(_basePath, "Images");
        _pdfLogoFullPath = Path.Combine(_imagesPath, "pdf-logo.png");
    }
    public void GenerateDocument()
    {
        FontManagerExtensions.RegisterFromPath(_fontsPath);
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.DefaultTextStyle(TextStyle.Default.FontFamily(DefaultFont));
                page.Size(PageSizes.A4);

                page.Margin(25);

                page.Header().Element(ComposeHeader);

                page.Content().Element(ComposeContent);

                page.Footer().Element(ComposeFooter);
            });
        }).ShowInPreviewer();
    }

    private void ComposeHeader(IContainer container)
    {
        container.ShowIf(ctx => ctx.PageNumber == 1).Column(col =>
        {
            col.Item().Row(row =>
            {
                row.ConstantItem(200).Image(_pdfLogoFullPath);

                row.RelativeItem()
                    .Column(col =>
                    {
                        col.Item().AlignRight()
                            .Text("Orçamento Nº: 000000").FontSize(14).Bold();
                        col.Item()
                            .AlignRight()
                            .DefaultTextStyle(TextStyle.Default.FontColor(Colors.Grey.Medium).FontSize(9))
                            .Text(text =>
                            {
                                text.Span("Gerado em: ");
                                text.Span($"15/05/2024");
                                // text.Span($"{Model.IssueDate:d}");
                            });
                    });
            });

            col.Item().PaddingTop(20).Row(row =>
            {
                row.RelativeItem()
                    .DefaultTextStyle(TextStyle.Default.FontSize(10))
                    .Column(col =>
                    {
                        col.Item().Table(t =>
                        {
                            t.ColumnsDefinition(def =>
                            {
                                def.ConstantColumn(80);
                                def.ConstantColumn(160);
                                def.ConstantColumn(100);
                                def.RelativeColumn();
                            });

                            t.Header(header =>
                            {
                                header.Cell().Element(QuotationPdfStyles.HeaderTableTitleStyle).Text("Setor");
                                header.Cell().Element(QuotationPdfStyles.HeaderTableTitleStyle).Text("Responsável Frater");
                                header.Cell().Element(QuotationPdfStyles.HeaderTableTitleStyle).Text("Telefone");
                                header.Cell().Element(QuotationPdfStyles.HeaderTableTitleStyle).Text("E-mail");
                            });

                            t.Cell().Element(QuotationPdfStyles.HeaderTableCellStyle).Text("Pré / Pós");
                            t.Cell().Element(QuotationPdfStyles.HeaderTableCellStyle).Text("Nathália Camelo");
                            t.Cell().Element(QuotationPdfStyles.HeaderTableCellStyle).Text("(11) 95947-0779");
                            t.Cell().Element(QuotationPdfStyles.HeaderTableCellStyle).Text("comercial@fratermedical.com.br");
                        });
                    });
            });

            col.Item().PaddingTop(15).Row(row =>
            {
                row.RelativeItem()
                    .Element(QuotationPdfStyles.LGPDStyle)
                    .Column(col =>
                    {
                        col.Item().Element(Contents.LGPD);
                    });
            });
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(20)
            .DefaultTextStyle(TextStyle.Default.FontSize(10))
            .Column(col =>
            {
                col.Item().AddRowWithTitle("Paciente:", "JOAO");
                col.Item().WithHorizontalLine();

                col.Item().AddRowWithTitle("Cirurgiã(o):", "JOAO");
                col.Item().WithHorizontalLine();

                col.Item().AddRowWithTitle("Hospital:", "JOAO");
                col.Item().WithHorizontalLine();

                col.Item().AddRowWithTitle("Convênio:", "JOAO");
                col.Item().WithHorizontalLine();

                col.Item().AddRowWithTitle("Fonte Pagadora:", "JOAO");
                col.Item().WithHorizontalLine();

                col.Item().PaddingTop(20).Row(row =>
                {
                    row.RelativeItem()
                        .DefaultTextStyle(TextStyle.Default.FontSize(10))
                        .Column(col =>
                        {
                            col.Item().Table(t =>
                            {
                                t.ColumnsDefinition(def =>
                                {
                                    def.ConstantColumn(35);
                                    def.RelativeColumn();
                                    def.ConstantColumn(80);
                                    def.ConstantColumn(75);
                                    def.ConstantColumn(85);
                                    def.ConstantColumn(85);
                                });

                                t.Header(header =>
                                {
                                    header.Cell().Element(QuotationPdfStyles.ContentTableTitleStyle).Text("Qtd.");
                                    header.Cell().Element(QuotationPdfStyles.ContentTableTitleStyle).Text("Descrição do item");
                                    header.Cell().Element(QuotationPdfStyles.ContentTableTitleStyle).Text("Cód / Ref");
                                    header.Cell().Element(QuotationPdfStyles.ContentTableTitleStyle).Text("Anvisa");
                                    header.Cell().Element(QuotationPdfStyles.ContentTableTitleStyle).Text("Valor unitário");
                                    header.Cell().Element(QuotationPdfStyles.ContentTableTitleStyle).Text("Valor total");
                                });

                                for (var i = 0; i <= 100; i++)
                                {
                                    t.AddItemRow();
                                }

                                t.Footer(footer =>
                                {
                                    footer.Cell().ColumnSpan(6)
                                        .ShowIf(ctx => ctx.PageNumber == ctx.TotalPages).Row(tr =>
                                        {
                                            tr.RelativeItem().Element(QuotationPdfStyles.ContentTableFooterStyle);
                                            tr.AutoItem().Element(QuotationPdfStyles.ContentTableFooterTotalStyle).Text("R$ 112.100,00").Bold().FontSize(14).AlignRight();
                                        });
                                });
                            });
                        });
                });
            });
    }

    private void ComposeFooter(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().Row(r =>
            {
                r.RelativeItem().AlignRight().Text(text =>
                {
                    text.Span("Página ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });

            col.Item().ShowIf(ctx => ctx.PageNumber == ctx.TotalPages)
                      .Element(Contents.CompanyFooterInfo);
        });
    }
}

public static class QuotationPdfExtensions
{
    public static IContainer AddRowWithTitle(this IContainer container, string title, string content)
    {
        container.Row(row =>
            {
                row.ConstantItem(100)
                    .Element(QuotationPdfStyles.LabelTitleStyle)
                    .Text(title);

                row.RelativeItem(60)
                    .Element(QuotationPdfStyles.TextContentStyle)
                    .Text(content);
            });

        return container;
    }

    public static IContainer WithHorizontalLine(this IContainer container)
    {
        container.PaddingVertical(2).LineHorizontal(1).LineColor(Colors.Grey.Lighten3);
        return container;
    }

    public static void AddItemRow(this TableDescriptor container)
    {
        container.Cell().Element(QuotationPdfStyles.ContentTableCellStyle).AlignRight().Text("1");
        container.Cell().Element(QuotationPdfStyles.ContentTableCellStyle).Text("PLACA ZIGOMÁTICA 6 FUROS");
        container.Cell().Element(QuotationPdfStyles.ContentTableCellStyle).Text("PA.02.03.0048");
        container.Cell().Element(QuotationPdfStyles.ContentTableCellStyle).Text("00000000000");
        container.Cell().Element(QuotationPdfStyles.ContentTableCellStyle).AlignRight().Text("R$ 10.000,00");
        container.Cell().Element(QuotationPdfStyles.ContentTableCellStyle).AlignRight().Text("R$ 10.000,00");
    }
}

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