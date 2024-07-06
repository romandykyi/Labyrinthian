using BenchmarkDotNet.Running;
using Benchmarks;
using Labyrinthian;

BenchmarkRunner.Run(typeof(OriginShiftBenchmarks));
return;

const int seed = 3403;
const int tests = 500;
const int deltaFactorsNumber = 21;

Random rnd = new(seed);

Func<Maze>[] mazeTypes =
[
	() => new OrthogonalMaze(5, 5),
	() => new OrthogonalMaze(10, 10),
	() => new OrthogonalMaze(20, 20),
	() => new OrthogonalMaze(30, 30),
	() => new OrthogonalMaze(50, 50),
];

float[] deltaFactors = Enumerable.Range(0, deltaFactorsNumber)
	.Select(n => (float)n / (deltaFactorsNumber - 1))
	.ToArray();

foreach (var mazeFunc in mazeTypes)
{
	var testMaze = mazeFunc();
	int edgesCount = testMaze.Cells.Sum(c => c.Neighbors.Length);
	using StreamWriter sw = new($"{testMaze}.csv");

	sw.WriteLine("deltaFactor; iterations; dead ends ratio; path length to cells ratio");
	string[,] results = new string[tests, deltaFactors.Length];
	int[] localSeeds = new int[tests];
	for (int i = 0; i < tests; i++)
	{
		localSeeds[i] = rnd.Next();
	}
	Parallel.For(0, tests, i =>
	{
		int localSeed = localSeeds[i];

		for (int j = 0; j < deltaFactors.Length; j++)
		{
			var maze = mazeFunc();
			MazePath path = new(maze, maze.GetOuterWalls().First(), maze.GetOuterWalls().Last());
			maze.Paths.Add(path);

			var function = HeatMapDecayFunctions.Multiplicative(deltaFactors[j]);
			OriginShiftParams @params = new()
			{
				MaxIterations = -1,
				GenerateUntilAllCellsAreVisited = true,
				NeighborSelector = new HeatMapNeighborSelector(function)
			};
			OriginShiftGeneration generator = new(maze, localSeed, @params);

			int iterations = generator.GenerateStepByStep().Count();
			float deadEndsRatio = (float)maze.FindDeadEnds().Count() / maze.Cells.Length;
			float pathLengthToCells = (float)path.GetSegments(false).Count() / maze.Cells.Length;
			results[i, j] = $"{deltaFactors[j]}; {iterations}; {deadEndsRatio}; {pathLengthToCells}";
		}
	});
	for (int i = 0; i < deltaFactors.Length; i++)
	{
		for (int j = 0; j < tests; j++)
		{
			sw.WriteLine(results[j, i]);
		}
	}
}