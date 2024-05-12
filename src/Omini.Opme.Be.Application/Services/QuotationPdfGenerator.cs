using System.Reflection;
using Omini.Opme.Be.Domain.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace Omini.Opme.Be.Application.Services;

public sealed class QuotationPdfGenerator : IPdfGenerator
{
    public void GenerateDocument()
    {
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(1, QuestPDF.Infrastructure.Unit.Centimetre);

                page.Header().Element(ComposeHeader);

                page.Content().Element(ComposeContent);

                page.Footer().Element(ComposeFooter);
            });
        }).ShowInPreviewer();
    }

    void ComposeFooter(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().Row(r =>
            {
                r.RelativeItem().AlignRight().Text(text => {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
            col.Item().Row(r =>
            {
                r.RelativeItem().AlignCenter().Text("FRATER MEDICAL COMÉRCIO DE MATERIAS CIRÚRGICOS LTDA");
            });
            col.Item().Row(r =>
            {
                r.RelativeItem().AlignCenter().Text("CNPJ: 44.729.842/0001-54");
            });
            col.Item().Row(r =>
            {
                r.RelativeItem().AlignCenter().Text("ALAMEDA OLGA, 288 - BARRA FUNDA / SÃO PAULO - SP");
            });
        });
    }

    void ComposeHeader(IContainer container)
    {
        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        container.Column(col =>
        {
            col.Item()
                .AlignCenter()
                .MaxWidth(15, Unit.Centimetre)
                .Image(Path.Combine(basePath!, "Images", "pdf-logo.png"));

            col.Spacing(1, Unit.Centimetre);

            col.Item().Row(r =>
            {
                r.RelativeItem().Row(i =>
                {
                    i.AutoItem()
                        .Text("Paciente:")
                        .FontColor(Colors.Grey.Darken2)
                        .LetterSpacing(0.15f);
                    i.Spacing(2, Unit.Centimetre);
                    i.AutoItem()
                        .Text("Paciente")
                        .FontColor(Colors.Black)
                        .Bold()
                        .LetterSpacing(0.15f);
                });

                r.RelativeItem()
                    .AlignRight()
                    .Row(i =>
                    {
                        i.AutoItem()
                            .Text("Data:")
                            .FontColor(Colors.Grey.Darken2);
                        i.Spacing(2, Unit.Centimetre);
                        i.AutoItem()
                            .Text("Paciente")
                            .Bold();
                    });
            });

            // col.RelativeItem().Column(column =>
            // {
            //     column
            //         .Item().Text($"Invoice #")
            //         .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

            //     column.Item().Text(text =>
            //     {
            //         text.Span("Issue date: ").SemiBold();
            //         text.Span($"");
            //     });

            //     column.Item().Text(text =>
            //     {
            //         text.Span("Due date: ").SemiBold();
            //         text.Span($"");
            //     });
            // });
        });
    }

    private IContainer DefaultHeaderFooterCellStyle(IContainer container, TextStyle textStyle, Color backgroundColor)
    {
        return container
            .Border(1)
            .Background(backgroundColor)
            .PaddingVertical(5)
            .PaddingHorizontal(2)
            .AlignCenter()
            .AlignMiddle()
            .DefaultTextStyle(textStyle);
    }

    private IContainer DefaultCellStyle(IContainer container, TextStyle textStyle, Color backgroundColor)
    {
        return container
            .Border(1)
            .Background(backgroundColor)
            .PaddingVertical(5)
            .PaddingHorizontal(2)
            .AlignMiddle()
            .DefaultTextStyle(textStyle);
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Item().Table(t =>
            {
                t.ColumnsDefinition(def =>
                {
                    def.ConstantColumn(3, Unit.Centimetre);
                    def.ConstantColumn(10, Unit.Centimetre);
                    def.ConstantColumn(3, Unit.Centimetre);
                    def.RelativeColumn();
                });

                t.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Setor Frater");
                    header.Cell().Element(CellStyle).Text("Especialista Frater");
                    header.Cell().Element(CellStyle).Text("Telefone");
                    header.Cell().Element(CellStyle).Text("E-mail");

                    IContainer CellStyle(IContainer container) => DefaultHeaderFooterCellStyle(container, TextStyle.Default.Black().Bold(), Colors.Grey.Lighten3);
                });

                t.Cell().Element(CellStyle).Text("PRÉVIA / PÓS");
                t.Cell().Element(CellStyle).Text("NATHÁLIA CAMELO");
                t.Cell().Element(CellStyle).Text("(11) 95947-0779");
                t.Cell().Element(CellStyle).Text("COMERCIAL@FRATERMEDICAL.COM.BR");

                IContainer CellStyle(IContainer container) => DefaultHeaderFooterCellStyle(container, TextStyle.Default.Black().FontSize(10).NormalWeight(), Colors.White);
            });

            column.Spacing(20);

            // column.Item().Row(row =>
            // {
            //     //row.RelativeItem().Component(new AddressComponent("From", Model.SellerAddress));
            //     row.ConstantItem(50);
            //    // row.RelativeItem().Component(new AddressComponent("For", Model.CustomerAddress));
            // });

            column.Item().Element(ComposeTable);

            // var totalPrice = Model.Items.Sum(x => x.Price * x.Quantity);
            // column.Item().PaddingRight(5).AlignRight().Text($"Grand total: {totalPrice:C}").SemiBold();

            //if (!string.IsNullOrWhiteSpace(Model.Comments))
            //    column.Item().PaddingTop(25).Element(ComposeComments);
        });
    }

    void ComposeTable(IContainer container)
    {
        container.Table(t =>
        {
            t.ColumnsDefinition(def =>
            {
                def.ConstantColumn(3, Unit.Centimetre);
                def.ConstantColumn(10, Unit.Centimetre);
                def.ConstantColumn(3, Unit.Centimetre);
                def.ConstantColumn(3, Unit.Centimetre);
                def.ConstantColumn(2.5f, Unit.Centimetre);
                def.ConstantColumn(3, Unit.Centimetre);
                def.RelativeColumn();
            });

            t.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Quantidade");
                header.Cell().Element(CellStyle).Text("Descrição do Item");
                header.Cell().Element(CellStyle).Text("Cód. / Ref.");
                header.Cell().Element(CellStyle).Text("Anvisa");
                header.Cell().Element(CellStyle).Text("Val. Anvisa");
                header.Cell().Element(CellStyle).Text("Valor Unitário");
                header.Cell().Element(CellStyle).Text("Valor Total");

                IContainer CellStyle(IContainer container) => DefaultHeaderFooterCellStyle(container, TextStyle.Default.Black().Bold(), Colors.Grey.Lighten3);
            });

            t.Cell().Element(CellStyle).Text("1").AlignCenter();
            t.Cell().Element(CellStyle).Text("DISSECTOR PEQUENO");
            t.Cell().Element(CellStyle).Text("PA02030007").AlignCenter();
            t.Cell().Element(CellStyle).Text("80455630023").AlignCenter();
            t.Cell().Element(CellStyle).Text("VIGENTE").AlignCenter();
            t.Cell().Element(CellStyle).Row(tr =>
            {
                tr.RelativeItem().Text("R$");
                tr.RelativeItem().Text("2.100,00");
            });
            t.Cell().Element(CellStyle).Row(tr =>
            {
                tr.RelativeItem().Text("R$");
                tr.RelativeItem().Text("2.100,00");
            });

            IContainer CellStyle(IContainer container) => DefaultCellStyle(container, TextStyle.Default.Black().FontSize(10).NormalWeight(), Colors.White);

            t.Footer(footer =>
            {
                footer.Cell().ColumnSpan(6).Element(FooterNoContentStyle);
                footer.Cell().Element(FooterTotalStyle).Row(tr =>
                {
                    tr.RelativeItem().Text("R$");
                    tr.RelativeItem().Text("2.100,00");
                });

                IContainer FooterNoContentStyle(IContainer container) => DefaultHeaderFooterCellStyle(container, TextStyle.Default.Black().Bold(), Colors.Grey.Lighten3);
                IContainer FooterTotalStyle(IContainer container) => DefaultCellStyle(container, TextStyle.Default.Black().FontSize(10).NormalWeight(), Colors.White);
            });
        });
    }
}