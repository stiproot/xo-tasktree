using Xo.TaskTree.Abstractions;
using Xo.TaskTree.Core;
using Xo.TaskTree.DependencyInjection.Extensions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System;

namespace Xo.TaskTree.Benchmarks;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionFactory
{
  public static IServiceCollection CreateServiceCollection()
  {
    var services = new ServiceCollection()
      .AddTaskFlowServices();

    services.TryAddSingleton<ITestService, TestService>();

    return services;
  }

  public static IServiceProvider CreateServiceProvider()
  {
    return CreateServiceCollection().BuildServiceProvider();
  }
}
