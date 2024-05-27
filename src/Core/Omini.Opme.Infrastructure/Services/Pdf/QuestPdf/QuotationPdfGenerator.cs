using System.Globalization;
using System.Reflection;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Domain.Services;
using Omini.Opme.Domain.Services.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Omini.Opme.Infrastructure.Pdf.QuestPdf;

public sealed class QuotationPdfGenerator : IQuotationPdfGenerator
{
    const string DefaultFont = "Noto Sans";
    private readonly string _basePath;
    private readonly string _imagesPath;
    private readonly string _pdfLogoFullPath;
    private readonly IDateTimeService _dateTimeService;
    private readonly CultureInfo _culture;

    public QuotationPdfGenerator(IDateTimeService dateTimeService)
    {
        _basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        _imagesPath = Path.Combine(_basePath, "Images");
        _pdfLogoFullPath = Path.Combine(_imagesPath, "pdf-logo.png");
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

                page.Header().Element(e => ComposeHeader(e, quotation));

                page.Content().Element(e => ComposeContent(e, quotation));

                page.Footer().Element(e => ComposeFooter(e));
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
                AddLabelAndContent(col, "Paciente:", quotation.Patient.Name.FullName);
                AddHorizontalLine(col);

                AddLabelAndContent(col, "Cirurgiã(o):", quotation.Physician.Name.FullName);
                AddHorizontalLine(col);

                AddLabelAndContent(col, "Hospital:", quotation.Hospital.Name.TradeName);
                AddHorizontalLine(col);

                AddLabelAndContent(col, "Convênio:", quotation.InsuranceCompany.Name.TradeName);
                AddHorizontalLine(col);

                AddLabelAndContent(col, "Fonte Pagadora:", quotation.PayingSource.Name);
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
                      .Element(DefaultContent.CompanyFooterInfo);
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