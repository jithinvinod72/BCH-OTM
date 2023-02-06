using BCMCH.OTM.Data.Booking;
using BCMCH.OTM.Data.Contract.Booking;
using BCMCH.OTM.Data.Contract.Master;
using BCMCH.OTM.Data.Master;
using BCMCH.OTM.Domain.Booking;
using BCMCH.OTM.Domain.Contract.Booking;
using BCMCH.OTM.Domain.Contract.Master;
using BCMCH.OTM.Domain.Master;
using BCMCH.OTM.External;
using BCMCH.OTM.Infrastucture.AppSettings;
using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BCMCH.OTM.Infrastucture.IOC
{
    public static class DIConfiguration
    {
        public static void AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            AddDomainDependency(services, configuration);

        }
        private static void AddDomainDependency(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IOTMDataClient, OTMDataClient>();
            services.AddTransient<IMasterDomainService, MasterDomainService>();
            services.AddTransient<IMasterDataAccess, MasterDataAccess>();
            services.AddTransient<IBookingDataAccess, BookingDataAccess>();
            services.AddTransient<IBookingDomainService, BookingDomainService>();
            services.AddTransient<ISqlDbHelper, SqlDbHelper>();
            services.AddTransient<IConnectionStrings, ConnectionStrings>();

        }
    }
}
