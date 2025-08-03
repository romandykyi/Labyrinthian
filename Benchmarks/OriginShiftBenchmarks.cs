using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Labyrinthian;

namespace Benchmarks;

[ShortRunJob(RuntimeMoniker.Net80)]
public class OriginShiftBenchmarks
{
    private const int Seed = 123;

    private Maze _maze = null!;

    [Params(10, 20, 30, 50, 100)]
    public int MazeSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _maze = new OrthogonalMaze(MazeSize, MazeSize);
    }

    [Benchmark]
    public Maze AldousBroder() => new AldousBroderGeneration(_maze, seed: Seed).Generate();
    [Benchmark]
    public Maze HeatMapAldousBroder()
    {
        HeatMapNeighborSelector selector = new();
        AldousBroderGeneration generator = new(_maze, seed: Seed, neighborSelector: selector);
        return generator.Generate();
    }
    [Benchmark]
    public Maze DFSGeneration() => new DFSGeneration(_maze, seed: Seed).Generate();
    [Benchmark]
    public Maze Kruskal() => new KruskalGeneration(_maze, seed: Seed).Generate();
    [Benchmark]
    public Maze OriginShift()
    {
        OriginShiftParams @params = new()
        {
            MaxIterations = MazeSize * MazeSize * 10,
            GenerateUntilAllCellsAreVisited = false,
            NeighborSelector = new UnweightedNeighborSelector()
        };
        OriginShiftGeneration generator = new(_maze, seed: Seed, @params);
        return generator.Generate();
    }
    [Benchmark]
    public Maze HeatMapOriginShift()
    {
        OriginShiftParams @params = new()
        {
            MaxIterations = -1,
            GenerateUntilAllCellsAreVisited = true,
            NeighborSelector = new HeatMapNeighborSelector()
        };
        OriginShiftGeneration generator = new(_maze, seed: Seed, @params);
        return generator.Generate();
    }
    [Benchmark]
    public Maze Prim() => new PrimGeneration(_maze, seed: Seed).Generate();
    [Benchmark]
    public Maze Wilson() => new WilsonGeneration(_maze, seed: Seed).Generate();
}
