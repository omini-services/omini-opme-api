using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Omini.Opme.Be.Application.QuestPdf.Default;

public static class Contents
{
    public static void LGPD(this IContainer container)
    {
        container.Text(text =>
        {
            text.Justify();
            text.Span("LGPD: ").FontColor(Colors.Red.Medium).Bold().Italic();
            text.Span("Nos termos da lei 13.709/2018, as informações deste documento são confidenciais, e devem ser lidas / compartilhadas exclusivamente pelo(s) destinatário(s), sujeitas a privilégio legal de comunicação. A distribuição, divulgação ou disseminação das informações aqui contidas ou anexadas é terminantemente proibida, sujeitando o(a) responsável às penalidades aplicáveis. Se você recebeu este orçamento por engano ou caso não seja o destinatário, por favor, nos informe o mais rápido o possível e a apague, inclusive da lixeira de e-mails. Qualquer dúvida e/ou denúcina, nos contatar através do e-mail: ")
                .FontColor(Colors.Grey.Medium).Italic();
            text.Span("dpo@fratermedical.com.br").FontColor(Colors.Grey.Darken1).Bold().Italic();
        });
    }

    public static void CompanyFooterInfo(this IContainer container)
    {
        container.Column(col =>
        {
            col.Item().PaddingTop(5).Element(QuestPdfContentStyles.FooterCompanyNameStyle).Row(r =>
            {
                r.AutoItem().Text("FRATER MEDICAL COMÉRCIO DE MATERIAS CIRÚRGICOS LTDA");
            });

            col.Item().Element(QuestPdfContentStyles.FooterCompanyDetailsStyle).Row(r =>
            {
                r.AutoItem().Text("CNPJ: 44.729.842/0001-54");
            });

            col.Item().Element(QuestPdfContentStyles.FooterCompanyDetailsStyle).Row(r =>
            {
                r.AutoItem().Text("ALAMEDA OLGA, 288 - BARRA FUNDA / SÃO PAULO - SP");
            });

            col.Item().Element(QuestPdfContentStyles.FooterSustentabilityStyle).Row(r =>
            {
                r.AutoItem().Text("Sustentabilidade: Antes de imprimir, reflita se realmente é necessário");
            });
        });
    }
}

public static class QuestPdfContentStyles
{

    public static IContainer FooterCompanyNameStyle(IContainer container)
    {
        return container
            .AlignCenter()
            .DefaultTextStyle(TextStyle.Default.FontColor(Colors.Black)
            .FontSize(10)
            .Bold());
    }

    public static IContainer FooterCompanyDetailsStyle(IContainer container)
    {
        return container
            .AlignCenter()
            .DefaultTextStyle(TextStyle.Default.FontColor(Colors.Black)
            .FontSize(10));
    }

    public static IContainer FooterSustentabilityStyle(IContainer container)
    {
        return container
            .PaddingTop(15)
            .AlignCenter()
            .DefaultTextStyle(TextStyle.Default.FontColor(Colors.Grey.Medium)
            .FontSize(10));
    }
}