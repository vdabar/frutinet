﻿using Frutinet.Common.Commands;
using Frutinet.Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Frutinet.Common.RabbitMq
{
    public static class Extensions
    {
        public static Task WithCommandHandlerAsync<TCommand>(
            this IBusClient bus,
            ICommandHandlerAsync<TCommand> handler) where TCommand : ICommand
        {
            return bus.SubscribeAsync<TCommand>(msg => handler.HandleAsync(msg),
                ctx => ctx.UseConsumerConfiguration(
                    cfg => cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<TCommand>()))));
        }

        public static Task WithEventHandlerAsync<TEvent>(
            this IBusClient bus,
            IEventHandlerAsync<TEvent> handler) where TEvent : IEvent
        {
            return bus.SubscribeAsync<TEvent>(msg => handler.HandleAsync(msg),
                ctx => ctx.UseConsumerConfiguration(
                    cfg => cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<TEvent>()))));
        }

        public static void AddRabbitMq(this IServiceCollection service, IConfiguration configuration)
        {
            var options = new RabbitMqOptions();
            var section = configuration.GetSection("rabbitmq");
        }

        private static string GetQueueName<T>()
        {
            return $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";
        }
    }
}