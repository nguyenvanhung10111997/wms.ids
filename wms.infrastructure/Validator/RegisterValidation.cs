using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace wms.infrastructure.Validator
{
    public static class RegisterValidation
    {
        public static void InitFluentValidation(this IServiceCollection services)
        {
            var AssemblyGetByNames = Assembly.GetEntryAssembly().GetReferencedAssemblies().Where(x => x.Name.ToLower().Contains("dto"));
            foreach (var assembly in AssemblyGetByNames)
            {
                var referenceAssembly = Assembly.Load(assembly); ;
                services.AddMvc().AddFluentValidation(x => x.RegisterValidatorsFromAssembly(referenceAssembly));
            }
        }
    }
}
