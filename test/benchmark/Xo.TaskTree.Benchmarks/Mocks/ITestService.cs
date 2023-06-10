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

public interface ITestService
{
  Task<bool> RunAsync(string strArg, bool boolArg);
}
