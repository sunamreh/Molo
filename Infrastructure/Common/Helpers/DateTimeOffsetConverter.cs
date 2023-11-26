using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Molo.Infrastructure.Common.Helpers
{
    public class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
    {
        public DateTimeOffsetConverter()
            : base(
                d => d.ToUniversalTime(),
                d => d.ToUniversalTime())
        {
        }
    }
}
