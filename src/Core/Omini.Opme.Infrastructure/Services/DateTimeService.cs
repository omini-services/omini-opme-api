using Omini.Opme.Domain.Services;

namespace Omini.Opme.Infrastructure.Services;

internal sealed class DateTimeService : IDateTimeService
{
    const string BrazilianTimeZone = "Central Brazilian Standard Time";
    public DateTime TimeZoneNow()
    {
        var timeUtc = DateTime.UtcNow;
        TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById(BrazilianTimeZone);
        DateTime brTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, istZone);

        return brTime;
    }
}