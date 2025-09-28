using BenchmarkDotNet.Attributes;
using Labyrinthian;

namespace Benchmarks;

[SimpleJob]
public class OriginShiftBenchmarks
{
    private Maze _maze = new OrthogonalMaze(100, 100);

    public const int Seed = 1241244;

    private OriginShiftParams GetParams(INeighborSelector neighborSelector)
    {
        return new()
        {
            GenerateUntilAllCellsAreVisited = true,
            MaxIterations = -1,
            NeighborSelector = neighborSelector
        };
    }

    [Benchmark]
    public Maze NoHeatMaps()
    {
        UnweightedNeighborSelector selector = new();
        OriginShiftGeneration generator = new(_maze, Seed, GetParams(selector));
        return generator.Generate();
    }

    [Benchmark]
    public Maze HeatMapsLinearDecay()
    {
        HeatMapNeighborSelector selector = new(HeatMapDecayFunctions.Linear());
        OriginShiftGeneration generator = new(_maze, Seed, GetParams(selector));
        return generator.Generate();
    }

    [Benchmark]
    public Maze HeatMapsLogarithmicDecay()
    {
        HeatMapNeighborSelector selector = new(HeatMapDecayFunctions.Logarithmic());
        OriginShiftGeneration generator = new(_maze, Seed, GetParams(selector));
        return generator.Generate();
    }

    [Benchmark]
    public Maze HeatMapsExponentialDecay()
    {
        HeatMapNeighborSelector selector = new(HeatMapDecayFunctions.Exponential(0.4));
        OriginShiftGeneration generator = new(_maze, Seed, GetParams(selector));
        return generator.Generate();
    }

    [Benchmark]
    public Maze HeatMapsInverseVisitDecay()
    {
        HeatMapNeighborSelector selector = new(HeatMapDecayFunctions.InverseVisit());
        OriginShiftGeneration generator = new(_maze, Seed, GetParams(selector));
        return generator.Generate();
    }
}
