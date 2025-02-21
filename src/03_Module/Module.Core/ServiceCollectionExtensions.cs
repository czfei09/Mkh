﻿using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mkh.Module.Abstractions;

namespace Mkh.Module.Core;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加模块核心功能
    /// </summary>
    /// <param name="services"></param>
    /// <param name="environment"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IModuleCollection AddModulesCore(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
        var modules = new ModuleCollection(configuration);
        modules.Load();

        services.AddSingleton<IModuleCollection>(modules);

        return modules;
    }

    /// <summary>
    /// 添加模块相关服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="modules"></param>
    /// <param name="environment"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddModuleServices(this IServiceCollection services, IModuleCollection modules, IHostEnvironment environment, IConfiguration configuration)
    {
        foreach (var module in modules)
        {
            if (module == null)
                continue;

            //加载模块初始化器
            if (module.ServicesConfigurator != null)
            {
                var context = new ModuleConfigureContext
                {
                    Modules = modules,
                    Services = services,
                    Environment = environment,
                    Configuration = configuration
                };

                module.ServicesConfigurator.Configure(context);
            }

            services.AddApplicationServices(module);
        }

        return services;
    }

    /// <summary>
    /// 添加应用服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="module"></param>
    private static void AddApplicationServices(this IServiceCollection services, ModuleDescriptor module)
    {
        var assembly = module.LayerAssemblies.Core;
        //按照约定，应用服务必须采用Service结尾
        var implementationTypes = assembly.GetTypes().Where(m => m.Name.EndsWith("Service") && !m.IsInterface).ToList();

        foreach (var implType in implementationTypes)
        {
            //按照约定，服务的第一个接口类型就是所需的应用服务接口
            var serviceType = implType.GetInterfaces()[0];

            services.AddScoped(implType);

            module.ApplicationServices.Add(serviceType, implType);
        }
    }
}