using Microsoft.Extensions.DependencyInjection;
using MusicPlatform.Services;
using MusicPlatform.Services.Implementatiotion;
using MusicPlatform.Services.Interfaces;

namespace MusicPlatform.Dependency
{
    public static class ServicesRegistrator
    {
        public static void RegisterDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IImageFileConverter, ImageFileConverter>();
            services.AddScoped<IChartBuilder, ChartBuilder>();
            services.AddTransient<IReleaseToApiConverter, ReleaseToApiConverter>();
            services.AddScoped<DataCreator>();
            services.AddTransient<IFileSaver, FileSaver>();
        }
    }
}
