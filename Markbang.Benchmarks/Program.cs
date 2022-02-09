using BenchmarkDotNet.Running;
using Markbang.Benchmarks;

var summaries = BenchmarkRunner.Run<MarkdownBenchmarks>();