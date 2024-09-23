using Common.Interfaces;

namespace UserManagement.Infrastructure.Services
{
    public class DateTimeProvider: IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
        
    }
}