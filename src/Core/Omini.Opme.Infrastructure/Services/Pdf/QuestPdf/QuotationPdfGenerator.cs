using System.Globalization;
using System.Reflection;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Services.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace Omini.Opme.Infrastructure.Pdf.QuestPdf;

public sealed class QuotationPdfGenerator : IQuotationPdfGenerator
{
    const string DefaultFont = "Noto Sans";
    private readonly string _basePath;
    private readonly string _imagesPath;
    private readonly string _pdfLogoFullPath;
    private readonly string _linkedInIcoFullPath;
    private readonly string _instagramIcoFullPath;
    private readonly IDateTimeService _dateTimeService;
    private readonly CultureInfo _culture;

    public QuotationPdfGenerator(IDateTimeService dateTimeService)
    {
        _basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _imagesPath = Path.Combine(_basePath, "Images");
        _pdfLogoFullPath = Path.Combine(_imagesPath, "pdf-logo.png");
        _linkedInIcoFullPath = Path.Combine(_imagesPath, "linkedin.svg");
        _instagramIcoFullPath = Path.Combine(_imagesPath, "instagram.svg");
        _culture = new CultureInfo("pt-BR");

        _dateTimeService = dateTimeService;
    }

    public byte[] GenerateBytes(Quotation quotation)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.DefaultTextStyle(TextStyle.Default.FontFamily(DefaultFont));
                page.Size(PageSizes.A4);

                page.Margin(25);
                page.MarginBottom(0);

                page.Header().Element(e => ComposeHeader(e, quotation));

                page.Content().Element(e => ComposeContent(e, quotation));

                page.Footer().Height(60).Element(e => ComposeFooter(e));
            });
        }).GeneratePdf();
    }

    private void ComposeHeader(IContainer container, Quotation quotation)
    {
        var dateNow = _dateTimeService.TimeZoneNow();

        container.ShowIf(ctx => ctx.PageNumber == 1).Column(col =>
        {
            col.Item().Row(row =>
            {
                row.ConstantItem(200).Image(_pdfLogoFullPath);

                row.RelativeItem()
                    .Column(col =>
                    {
                        col.Item().AlignRight()
                            .Text($"Orçamento Nº: {quotation.Number.ToString().PadLeft(6, '0')}").FontSize(14).Bold();
                        col.Item()
                            .AlignRight()
                            .DefaultTextStyle(TextStyle.Default.FontColor(Colors.Grey.Medium).FontSize(9))
                            .Text(text =>
                            {
                                text.Span("Gerado em: ");
                                text.Span($"{dateNow:dd/MM/yyyy}");
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
                        col.Item().Element(DefaultContent.LGPD);
                    });
            });
        });
    }

    private void ComposeContent(IContainer container, Quotation quotation)
    {
        container.PaddingVertical(20)
            .DefaultTextStyle(TextStyle.Default.FontSize(10))
            .Column(col =>
            {
                AddLabelAndContent(col, "Paciente:", quotation.PatientName.FullName);
                AddHorizontalLine(col);

                AddLabelAndContent(col, "Cirurgiã(o):", quotation.PhysicianName.FullName);
                AddHorizontalLine(col);

                AddLabelAndContent(col, "Hospital:", quotation.HospitalName);
                AddHorizontalLine(col);

                AddLabelAndContent(col, "Convênio:", quotation.InsuranceCompanyName);
                AddHorizontalLine(col);

                AddLabelAndContent(col, "Fonte Pagadora:", quotation.PayingSourceName);
                AddHorizontalLine(col);

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

                                foreach (var quotationItem in quotation.Items)
                                {
                                    AddRow(t, quotationItem);
                                }

                                t.Footer(footer =>
                                {
                                    footer.Cell().ColumnSpan(6)
                                        .ShowIf(ctx => ctx.PageNumber == ctx.TotalPages).Row(tr =>
                                        {
                                            tr.RelativeItem().Element(QuotationPdfStyles.ContentTableFooterStyle);
                                            tr.AutoItem().Element(QuotationPdfStyles.ContentTableFooterTotalStyle).Text($"R$ {quotation.Total.ToString("N2", _culture)}").Bold().FontSize(14).AlignRight();
                                        });
                                });
                            });
                        });
                });

                col.Item().PaddingTop(10).Row(r =>
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
                          .Element(DefaultContent.CompanyFooterInfo);
            });
    }

    private void ComposeFooter(IContainer container)
    {
        container.ShowIf(ctx => ctx.PageNumber == ctx.TotalPages).Column(col =>
        {
            col.Item()
                .PaddingHorizontal(-25)
                .PaddingTop(10)
                .Layers(layers =>
                {
                    layers
                        .PrimaryLayer()
                        .AlignCenter()
                        .Height(50)
                        .AlignMiddle()
                        .Padding(0)
                        .Table(t =>
                        {
                            t.ColumnsDefinition(def =>
                               {
                                   def.ConstantColumn(150);
                                   def.RelativeColumn();
                                   def.ConstantColumn(150);
                               });
                            t.Cell().ExtendVertical();
                            t.Cell().AlignCenter().Row(row =>
                            {
                                row.AutoItem()
                                    .MaxHeight(50)
                                    .AlignMiddle()
                                    .AlignCenter()
                                    .Column(col =>
                                    {
                                        col.Item().Hyperlink("https://www.google.com.br").Row(r =>
                                        {
                                            r.AutoItem().AlignCenter().AlignMiddle().MaxHeight(15).Svg(SvgImage.FromFile(_linkedInIcoFullPath));
                                            r.AutoItem().PaddingLeft(3).AlignCenter().AlignMiddle().MaxHeight(20).Text("Frater Medical");

                                            r.AutoItem().PaddingLeft(10).AlignCenter().AlignMiddle().MaxHeight(15).Svg(SvgImage.FromFile(_instagramIcoFullPath));
                                            r.AutoItem().PaddingLeft(3).AlignCenter().AlignMiddle().MaxHeight(20).Text("@fratermedical");
                                        });
                                    });
                            });
                            t.Cell().AlignRight().AlignBottom().PaddingRight(5).Text("Gerado por Omini ®").FontSize(10).FontColor(Colors.Grey.Darken1);
                        });

                    layers.Layer().SkiaSharpCanvas((canvas, size) =>
                    {
                        DrawRoundedRectangle(Colors.Grey.Lighten3, false);

                        void DrawRoundedRectangle(string colorHex, bool isStroke)
                        {
                            // Parse the color and set the alpha for transparency
                            SKColor color = SKColor.Parse(colorHex).WithAlpha((byte)(0.5f * 255));

                            using var paint = new SKPaint
                            {
                                Color = color,
                                IsStroke = isStroke,
                                StrokeWidth = 100,
                                IsAntialias = true
                            };

                            float cornerRadius = 30;
                            float canvasHeight = 50;

                            SKRect rect = new SKRect(0, 0, canvas.LocalClipBounds.Width, canvasHeight);

                            // Create a path with a rounded top
                            SKPath path = new SKPath();
                            path.MoveTo(rect.Left, rect.Top + cornerRadius);
                            path.ArcTo(new SKRect(rect.Left, rect.Top, rect.Left + cornerRadius * 2, rect.Top + cornerRadius * 2), 180, 90, false);
                            path.LineTo(rect.Right - cornerRadius, rect.Top);
                            path.ArcTo(new SKRect(rect.Right - cornerRadius * 2, rect.Top, rect.Right, rect.Top + cornerRadius * 2), 270, 90, false);
                            path.LineTo(rect.Right, rect.Bottom);
                            path.LineTo(rect.Left, rect.Bottom);
                            path.Close();

                            canvas.DrawPath(path, paint);
                        }
                    });

                }
            );
        });
    }

    private static void AddLabelAndContent(ColumnDescriptor column, string title, string content)
    {
        column.Item().Row(row =>
            {
                row.ConstantItem(100)
                    .Element(QuotationPdfStyles.LabelTitleStyle)
                    .Text(title);

                row.RelativeItem(60)
                    .Element(QuotationPdfStyles.TextContentStyle)
                    .Text(content);
            });
    }

    private static void AddHorizontalLine(ColumnDescriptor column)
    {
        column.Item().PaddingVertical(2).LineHorizontal(1).LineColor(Colors.Grey.Lighten3);
    }

    private void AddRow(TableDescriptor container, QuotationItem quotationItem)
    {
        container.Cell().Element(QuotationPdfStyles.ContentTableCellStyle).AlignRight().Text(quotationItem.Quantity.ToString("F0", _culture));
        container.Cell().Element(QuotationPdfStyles.ContentTableCellStyle).Text(quotationItem.ItemName);
        container.Cell().Element(QuotationPdfStyles.ContentTableCellStyle).Text(quotationItem.ReferenceCode);
        container.Cell().Element(QuotationPdfStyles.ContentTableCellStyle).Text(quotationItem.AnvisaCode);
        container.Cell().Element(QuotationPdfStyles.ContentTableCellStyle).AlignRight().Text($"R$ {quotationItem.UnitPrice.ToString("N2", _culture)}");
        container.Cell().Element(QuotationPdfStyles.ContentTableCellStyle).AlignRight().Text($"R$ {quotationItem.LineTotal.ToString("N2", _culture)}");
    }
}