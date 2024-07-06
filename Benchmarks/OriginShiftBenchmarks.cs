using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Labyrinthian;

namespace Benchmarks;

[ShortRunJob(RuntimeMoniker.Net80)]
public class OriginShiftBenchmarks
{
	private const int Seed = 123;

	private Maze _maze = null!;

	[Params(20)]
	public int MazeSize { get; set; }

	[GlobalSetup]
	public void Setup()
	{
		_maze = new OrthogonalMaze(MazeSize, MazeSize);
	}

	[Benchmark]
	public Maze AldousBroder() => new AldousBroderGeneration(_maze, seed: Seed).Generate();
	[Benchmark]
	public Maze DFSGeneration() => new DFSGeneration(_maze, seed: Seed).Generate();
	[Benchmark]
	public Maze Kruskal() => new KruskalGeneration(_maze, seed: Seed).Generate();
	[Benchmark]
	public Maze OriginShift() => new OriginShiftGeneration(_maze, seed: Seed).Generate();
	[Benchmark]
	public Maze Prim() => new PrimGeneration(_maze, seed: Seed).Generate();
	[Benchmark]
	public Maze Wilson() => new WilsonGeneration(_maze, seed: Seed).Generate();
}
