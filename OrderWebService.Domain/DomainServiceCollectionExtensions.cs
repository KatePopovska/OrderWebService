using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using OrderWebService.Domain.Commands;
using OrderWebService.Domain.Data;
using OrderWebService.Domain.Helpers;

namespace OrderWebService.Domain
{
    public static class DomainServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, string connectionString)
        {
            return services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>())
                .AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString))
                .AddSingleton<IDateTimeProvider, DateTimeProvider>();
                
        }
    }
}
