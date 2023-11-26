using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Molo.Application.Common.Interfaces;
using Molo.Application.Molo.Collection.Command;
using Molo.Application.Molo.Subscription.Commands;
using Molo.Application.Molo.Transact.Commands;
using Molo.Domain.Entities;
using Molo.Infrastructure.Common.Http;
using Molo.Infrastructure.Common.Interfaces;
using Molo.Infrastructure.Common.Models;
using Molo.Infrastructure.Database;
using Molo.Infrastructure.MessageBroker;
using Molo.Infrastructure.Services.Molo.Collect;
using Molo.Infrastructure.Services.Molo.Subscription;
using Molo.Infrastructure.Services.Molo.Transact;
using Molo.Infrastructure.Services.Momo.Collection;
using Molo.Infrastructure.Services.Momo.Transfer;
using Polly;
using Polly.Extensions.Http;
using RabbitMQ.Client;
using System.Reflection;
using System;

namespace Molo.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MomoSettings>(options => configuration.GetSection("Momo").Bind(options));

            services.AddScoped<IMoloDbRepository<Subscriber>, MoloDbRepository<Subscriber>>();
            services.AddScoped<IMoloDbRepository<Client>, MoloDbRepository<Client>>();
            services.AddScoped<IMoloDbRepository<Loan>, MoloDbRepository<Loan>>();
            services.AddScoped<IMoloDbRepository<Transaction>, MoloDbRepository<Transaction>>();
            services.AddScoped<IMoloDbRepository<InterestRate>, MoloDbRepository<InterestRate>>();
            services.AddScoped<ISubscribeService, SubscribeService>();
            services.AddScoped<IMessageBrokerProducer<SubscribeCommand>, SubscriptionProducer>();
            services.AddScoped<IMessageBrokerProducer<CreateTransactionCommand>, TransactionProducer>();
            services.AddScoped<IMessageBrokerProducer<CollectCommand>, CollectProducer>();
            services.AddScoped<ITransactService, TransactService>();
            services.AddScoped<ICollectionService, CollectionService>();
            services.AddScoped<IDisbursementService, DisbursementService>();
            services.AddScoped<ICollectService, CollectService>();

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => (int)response.StatusCode == 500)
                .OrResult(response => (int)response.StatusCode == 503)
                .RetryAsync(5);

            services.AddHttpClient<IRestClient, RestClient>().ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();

                handler.ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                return handler;
            }).AddPolicyHandler(retryPolicy);

            services.AddDbContext<MoloDbContext>(options => options.UseLazyLoadingProxies().UseNpgsql(configuration.GetConnectionString("MoloDbContext")));
            
            services.AddMemoryCache();

            services.AddSingleton(provider =>
            {
                return new ConnectionFactory
                {
                    Uri = new Uri(configuration.GetConnectionString("RabbitMq")),
                    DispatchConsumersAsync = true
                };
            });

            return services;
        }
    }
}
