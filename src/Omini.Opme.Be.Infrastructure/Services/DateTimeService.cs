using Omini.Opme.Be.Domain.Services;

namespace Omini.Opme.Be.Infrastructure.Services;

internal class DateTimeService : IDateTimeService
{
    const string BrazilianTimeZone = "Central Brazilian Standard Time";
    public DateTime Now()
    {
        var timeUtc = DateTime.UtcNow;
        TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById(BrazilianTimeZone);
        DateTime brTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, istZone);

        return brTime;
    }
}