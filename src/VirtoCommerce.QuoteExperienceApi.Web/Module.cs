using AutoMapper;
using GraphQL.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.QuoteExperienceApi.Data;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;
using VirtoCommerce.QuoteExperienceApi.Data.Authorization;
using VirtoCommerce.QuoteExperienceApi.Data.Services;

namespace VirtoCommerce.QuoteExperienceApi.Web;

public class Module : IModule, IHasConfiguration
{
    public ManifestModuleInfo ModuleInfo { get; set; }
    public IConfiguration Configuration { get; set; }

    public void Initialize(IServiceCollection serviceCollection)
    {
        var assemblyMarker = typeof(AssemblyMarker);
        var graphQlBuilder = new CustomGraphQLBuilder(serviceCollection);
        graphQlBuilder.AddGraphTypes(assemblyMarker);
        serviceCollection.AddMediatR(assemblyMarker);
        serviceCollection.AddAutoMapper(assemblyMarker);
        serviceCollection.AddSchemaBuilders(assemblyMarker);

        serviceCollection.AddTransient<IQuoteConverter, QuoteConverter>();
        serviceCollection.AddTransient<IQuoteAggregateRepository, QuoteAggregateRepository>();
        serviceCollection.AddSingleton<IAuthorizationHandler, QuoteAuthorizationHandler>();
    }

    public void PostInitialize(IApplicationBuilder appBuilder)
    {
        // Nothing to do here
    }

    public void Uninstall()
    {
        // Nothing to do here
    }
}
