// See https://aka.ms/new-console-template for more information

await RunTest(testSuitesCount: 10, testCasesInSuiteCount: 5, stepsInTestCaseCount: 5);
await RunTest(testSuitesCount: 100, testCasesInSuiteCount: 5, stepsInTestCaseCount: 5);
await RunTest(testSuitesCount: 500, testCasesInSuiteCount: 5, stepsInTestCaseCount: 5);
await RunTest(testSuitesCount: 1000, testCasesInSuiteCount: 5, stepsInTestCaseCount: 5);

Console.ReadLine();

#region Test Helpers

async Task RunTest(int testSuitesCount, int testCasesInSuiteCount, int stepsInTestCaseCount)
{
    AppArguments arguments = new TestEnvConfigurations().GetAppArguments();

    List<TestCase> testCases = new TestCasesHelper(arguments.RootTestSuite).CreateTestCases(testSuitesCount, testCasesInSuiteCount, stepsInTestCaseCount);

    AppBootstrap app = SetupApp(arguments, testCases);

    Console.WriteLine("Run clean command.");
    await app.RunAsync<CleanAppCommand>();
    
    Console.WriteLine("Run sync command");
    Console.WriteLine($"testSuitesCount: {testSuitesCount}, testCasesInSuiteCount: {testCasesInSuiteCount}, stepsInTestCaseCount: {stepsInTestCaseCount}");
    var ts = await RunSyncCommand(app);
    Console.WriteLine($"Synchronization time {ts.TotalSeconds} seconds");
}

async Task<TimeSpan> RunSyncCommand(AppBootstrap app)
{
    var stopWatch = new Stopwatch();
    stopWatch.Start();
    await app.RunAsync<PushAppCommand>();
    stopWatch.Stop();
    var timeSpan = TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds);
    return timeSpan;
}

AppBootstrap SetupApp(AppArguments arguments, IReadOnlyList<TestCase> testCases)
{
    var testCasesSource = new Mock<ITestCasesSource>();
    testCasesSource.Setup(m => m.Get()).Returns(testCases);

    var appBootstrap = new AppBootstrap(arguments)
        .WithStartup(new Startup())
        .WithOverrides(collection => { collection.Replace(ServiceDescriptor.Scoped<ITestCasesSource>(_ => testCasesSource.Object)); });
    return appBootstrap;
}

#endregion