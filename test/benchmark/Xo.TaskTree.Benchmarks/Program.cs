using BenchmarkDotNet.Running;
using System.Diagnostics.CodeAnalysis;
using System;

namespace Xo.TaskTree.Benchmarks;

[ExcludeFromCodeCoverage]
public class Program
{
  public static void Main(string[] args)
  {
    BenchmarkRunner.Resolve<FnFactoryBenchmarks>();
  }
}
