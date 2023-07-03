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
[MemoryDiagnoser]
public class FunctitectBenchmarks
{
  private IFunctitect _functitect;
  private readonly Type _serviceType = typeof(ITestService);
  private const string _methodName = "RunAsync";
  private const string _nextParamName = null;
  private readonly IArgs _params
    = new Dictionary<string, IMsg>
    {
      { "strArg", new Msg<string>("") },
      { "boolArg", new Msg<bool>(true) },
    };

  [GlobalSetup]
  public void GlobalSetup()
  {
    this._functitect = new Functitect(ServiceCollectionFactory.CreateServiceProvider());
  }

  [Benchmark]
  public IFunctoryInvoker Build()
  {
    return this._functitect.Build(_serviceType, _methodName, _nextParamName);
  }

  [Benchmark]
  public async Task Run()
  {
    var builder = this._functitect.Build(_serviceType, _methodName, _nextParamName) as IAsyncFunctoryInvoker;
    await builder.InvokeFunc(_params)();
  }
}
