using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DDD
{
    public static class MessageHandlingExtensions
    {
        public static void UseMessageHandlers(this IApplicationBuilder app, Action<MessageHandlersConfigurationBuilder> builder)
        {
            var cfg = new MessageHandlersConfigurationBuilder(app.ApplicationServices);
            cfg.Build();
        }

        public static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, MessageBus>();
            return services;
        }

        public static IServiceCollection AddMessageHandler(this IServiceCollection services, Type handlerType)
        {
            services.AddSingleton(handlerType);
            return services;
        }

        public static IServiceCollection AddMessageHandler<TMessageHandler>(this IServiceCollection services)
            where TMessageHandler : class
        {
            services.AddSingleton<TMessageHandler>();
            return services;
        }

        public static IServiceCollection AddMessageHandler<TMessageHandler>(this IServiceCollection services,
            Func<IServiceProvider, TMessageHandler> factory)
            where TMessageHandler : class
        {
            services.AddSingleton<TMessageHandler>(factory);
            return services;
        }
    }
}
