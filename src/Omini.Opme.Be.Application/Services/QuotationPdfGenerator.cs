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
            });

        }).ShowInPreviewer();
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

       
            col.Item().Row(r=> {
                r.RelativeItem().Row(r1=> {
                    r1.AutoItem()
                        .Text("Paciente:")
                        .FontColor(Colors.Grey.Darken2)
                        .LetterSpacing(0.15f);
                    r1.Spacing(2, Unit.Centimetre);
                    r1.AutoItem()
                        .Text("Paciente")
                        .FontColor(Colors.Black)
                        .Bold()
                        .LetterSpacing(0.15f);
                });
                
                r.RelativeItem()
                    .AlignRight()
                    .Row(r1=> {
                    r1.AutoItem()
                        .Text("Data:")
                        .FontColor(Colors.Grey.Darken2)
                        .LetterSpacing(0.15f);
                    r1.Spacing(2, Unit.Centimetre);
                    r1.AutoItem()
                        .Text("Paciente")
                        .Bold()
                        .LetterSpacing(0.1f);
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

    void ComposeContent(IContainer container){
       container.PaddingVertical(40).Column(column => 
            {
                column.Spacing(20);
                
                column.Item().Row(row =>
                {
                    //row.RelativeItem().Component(new AddressComponent("From", Model.SellerAddress));
                    row.ConstantItem(50);
                   // row.RelativeItem().Component(new AddressComponent("For", Model.CustomerAddress));
                });

                column.Item().Element(ComposeTable);

               // var totalPrice = Model.Items.Sum(x => x.Price * x.Quantity);
               // column.Item().PaddingRight(5).AlignRight().Text($"Grand total: {totalPrice:C}").SemiBold();

                //if (!string.IsNullOrWhiteSpace(Model.Comments))
                //    column.Item().PaddingTop(25).Element(ComposeComments);
            }); 
    }

    void ComposeTable(IContainer container)
        {
            var headerStyle = TextStyle.Default.SemiBold();
            
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });
                
                table.Header(header =>
                {
                    header.Cell().Text("#");
                    header.Cell().Text("Product").Style(headerStyle);
                    header.Cell().AlignRight().Text("Unit price").Style(headerStyle);
                    header.Cell().AlignRight().Text("Quantity").Style(headerStyle);
                    header.Cell().AlignRight().Text("Total").Style(headerStyle);
                    
                    header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
                });
                
                // foreach (var item in Model.Items)
                // {
                //     var index = Model.Items.IndexOf(item) + 1;

                //     table.Cell().Element(CellStyle).Text($"{index}");
                //     table.Cell().Element(CellStyle).Text(item.Name);
                //     table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price:C}");
                //     table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity}");
                //     table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price * item.Quantity:C}");
                    
                //     static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                // }
            });
        }
}