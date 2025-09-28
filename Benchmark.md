```

BenchmarkDotNet v0.15.4, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
13th Gen Intel Core i5-13420H 2.10GHz, 1 CPU, 12 logical and 8 physical cores
.NET SDK 9.0.301
  [Host]     : .NET 9.0.6 (9.0.6, 9.0.625.26613), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 9.0.6 (9.0.6, 9.0.625.26613), X64 RyuJIT x86-64-v3


```
| Method                    | Mean     | Error    | StdDev   |
|-------------------------- |---------:|---------:|---------:|
| NoHeatMaps                | 63.60 ms | 0.835 ms | 0.740 ms |
| HeatMapsLinearDecay       | 12.82 ms | 0.246 ms | 0.312 ms |
| HeatMapsLogarithmicDecay  | 32.24 ms | 0.561 ms | 0.525 ms |
| HeatMapsExponentialDecay  | 21.15 ms | 0.422 ms | 0.645 ms |
| HeatMapsInverseVisitDecay | 19.63 ms | 0.388 ms | 0.728 ms |
