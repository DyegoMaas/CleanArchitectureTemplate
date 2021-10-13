using System.Linq;
using System.Reflection;
using CleanArchitectureTemplate.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddFluentValidation(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            
            return services;
        } 
        
        public static IServiceCollection AddFluentValidation(this IServiceCollection services, Assembly assembly)
        {
            var type = typeof(IValidator<>);

            var exportedTypes = assembly.GetExportedTypes()
                .Where(t =>
                    t.GetInterfaces()
                        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type))
                .ToList();

            foreach (var exportedType in exportedTypes)
            {
                var interfaceType = exportedType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == type)
                    .Select(i => i.GetGenericArguments()[0])
                    .First();

                var genericType = type.MakeGenericType(interfaceType);

                services.AddTransient(genericType, exportedType);
            }

            return services;
        }
    }
}