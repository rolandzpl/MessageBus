using System;
using Microsoft.Extensions.DependencyInjection;

namespace DDD
{
    public static class MessageHandlingExtensions
    {
        public static void UseMessageHandlers(this IServiceProvider sp, MessageHandlersConfigurationBuilder builder)
        {
            var cfg = new MessageHandlersConfigurationBuilder(sp);
            cfg.Build();
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
