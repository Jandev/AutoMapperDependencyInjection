using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using SomeDependencies;

namespace ConsoleApp1
{
    internal class Registration
    {
        public static IContainer Autofac()
        {
            var containerBuilder = new ContainerBuilder();

            // Register the convertor which is injected in the `MyProfile`
            containerBuilder.RegisterType<Convertor>().As<IConvertor>();
            containerBuilder.RegisterType<Startup>().As<IStartup>();

            var loadedProfiles = RetrieveProfiles();
            containerBuilder.RegisterTypes(loadedProfiles.ToArray());

            var container = containerBuilder.Build();

            //RegisterAutomapperDefault(container, assemblies);
            RegisterAutoMapper(container, loadedProfiles);

            return container;
        }

        /// <summary>
        /// Scan all referenced assemblies to retrieve all `Profile` types.
        /// </summary>
        /// <returns>A collection of <see cref="AutoMapper.Profile"/> types.</returns>
        private static List<Type> RetrieveProfiles()
        {
            var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                .Where(a => a.Name.StartsWith("Some"));
            var assemblies = assemblyNames.Select(an => Assembly.Load(an));
            var loadedProfiles = ExtractProfiles(assemblies);
            return loadedProfiles;
        }

        private static List<Type> ExtractProfiles(IEnumerable<Assembly> assemblies)
        {
            var profiles = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var assemblyProfiles = assembly.ExportedTypes.Where(type => type.IsSubclassOf(typeof(Profile)));
                profiles.AddRange(assemblyProfiles);
            }
            return profiles;
        }

        /// <summary>
        /// This is how you actually want to register your <see cref="Profile"/> types from all assemblies.
        /// Also uses Autofac to resolve services.
        /// </summary>
        private static void RegisterAutomapperDefault(IContainer container, IEnumerable<Assembly> assemblies)
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.ConstructServicesUsing(container.Resolve);

                //This is the easiest way. You just have to make sure all MappingProfiles have got an empty constructor. (recommended)
                cfg.AddProfiles(assemblies);
            });
        }

        /// <summary>
        /// Over here we iterate over all <see cref="Profile"/> types and resolve them via the <see cref="IContainer"/>.
        /// This way the `AddProfile` method will receive an instance of the found <see cref="Profile"/> type, which means
        /// all dependencies will be resolved via the <see cref="IContainer"/>.
        /// </summary>
        private static void RegisterAutoMapper(IContainer container, IEnumerable<Type> loadedProfiles)
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.ConstructServicesUsing(container.Resolve);
                
                foreach (var profile in loadedProfiles)
                {
                    var resolvedProfile = container.Resolve(profile) as Profile;
                    cfg.AddProfile(resolvedProfile);
                }
                
            });
        }

        
    }
}
