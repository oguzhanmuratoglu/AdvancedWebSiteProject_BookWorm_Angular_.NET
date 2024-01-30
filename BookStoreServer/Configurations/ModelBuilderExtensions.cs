using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using System.Reflection;

public static class ModelBuilderExtensions
{
    public static void ApplyAllConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly)
    {
        var configurations = assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
            .ToList();

        foreach (var configuration in configurations)
        {
            dynamic configInstance = Activator.CreateInstance(configuration);
            modelBuilder.ApplyConfiguration(configInstance);
        }
    }
}
