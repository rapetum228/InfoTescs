using InfoTecs.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InfoTecs.DAL.Tests;

public class ResultRepositoryTests
{
    private InfotecsDataContext _context;
    private ResultRepository _repository;
    private ResultRepositoryTestData _testData;
    private const string DatabaseName = "InfotecsTestDb";

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<InfotecsDataContext>()
                          .UseInMemoryDatabase(databaseName: DatabaseName)
                          .Options;

        _context = new InfotecsDataContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _repository = new ResultRepository(_context);
        _testData = new ResultRepositoryTestData();
    }

    [Test]
    public async Task AddResultAsync_TestAddResultWithNewName()
    {
        //given
        var result = _testData.GetResultForTest();
        var countResults = _context.Results.Count();
        var oldId = result.Id;

        //when
        await _repository.AddResultAsync(result);
        var actualId = result.Id;
        var expected = _context.Results.FirstOrDefault(x => x.Id == actualId);

        //than
        Assert.That(actualId, Is.GreaterThan(oldId));
        Assert.That(expected, Is.EqualTo(result));
        Assert.IsTrue(result.Values.Count > 0);
        Assert.IsTrue(countResults + 1 == _context.Results.Count());

    }

    [Test]
    public async Task AddResultAsync_TestAddResultWithOldName()
    {
        //given
        var result = _testData.GetResultForTest();
        await _context.Results.AddAsync(result);
        await _context.SaveChangesAsync();
        var countResults = _context.Results.Count();
        var oldName = String.Copy(result.FileName);

        //when
        await _repository.AddResultAsync(result);
        var actualId = result.Id;
        var expected = await _context.Results.Include(x => x.Values).Include(x => x.DateTimePeriod).FirstOrDefaultAsync(x => x.Id == actualId);

        //than
        Assert.That(expected, Is.EqualTo(result));
        Assert.That(oldName, Is.EqualTo(result.FileName));
        Assert.IsTrue(countResults == _context.Results.Count());
    }

    [Test]
    public async Task GetValuesByFileNameAsync_Test()
    {
        //given
        var fileName = "file";
        var results = _testData.GetListResultsForFillDb();
        await _context.Results.AddRangeAsync(results);
        await _context.SaveChangesAsync();
        var countValues = _context.Values.Count();


        //when
        var actual = await _repository.GetValuesByFileNameAsync(fileName);
        var result = await _context.Results.Include(x => x.Values).AsNoTracking().FirstOrDefaultAsync(x => x.FileName == fileName);
        var expected = result?.Values;

        //than
        Assert.That(expected, Is.EqualTo(actual));
        Assert.True(countValues == _context.Values.Count());
        Assert.That(result?.FileName, Is.EqualTo(fileName));
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task GetResultsByRequestAsync_Test(int numberCase)
    {
        //given
        var results = _testData.GetListResultsForFillDb();
        await _context.Results.AddRangeAsync(results);
        await _context.SaveChangesAsync();
        var request = _testData.GetRequest(numberCase);

        //when
        var actual = await _repository.GetResultsByRequestAsync(request);
        var count = actual.Count;

        //than
        Assert.IsTrue(request.StartAverageTime is null
            || actual.Any(x => x.AverageDiscretTime >= request.StartAverageTime));
        Assert.IsTrue(request.EndAverageTime is null
            || actual.Any(x => x.AverageDiscretTime <= request.EndAverageTime));
        Assert.IsTrue(request.StartAverageParameter is null
            || actual.Any(x => x.AverageParameters >= request.StartAverageParameter));
        Assert.IsTrue(request.EndAverageParameter is null
            || actual.Any(x => x.AverageParameters <= request.EndAverageParameter));
        Assert.IsTrue(request.StartPeriod is null
            || actual.Any(x => x.StartDateTime >= request.StartPeriod));
        Assert.IsTrue(request.EndPeriod is null
            || actual.Any(x => x.StartDateTime <= request.EndPeriod));
        Assert.IsTrue(string.IsNullOrWhiteSpace(request.FileName)
            || actual.Any(x => x.FileName.Contains(request.FileName!)));
    }

    [Test]
    public async Task RemoveResult_TestAsync()
    {
        //given
        var results = _testData.GetListResultsForFillDb();
        await _context.Results.AddRangeAsync(results);
        await _context.SaveChangesAsync();
        var id = results[0].Id;
        var periodId = results[0].DateTimePeriod.Id;
        var countResultValues = results[0].Values.Count;
        var countAllValues = _context.Values.Count();

        //when
        _repository.RemoveResult(results[0]);
        var result = _context.Results.FirstOrDefault(x => x.Id == id);
        var period = _context.Periods.FirstOrDefault(x => x.Id == periodId);

        //than
        Assert.IsNull(result);
        Assert.IsNull(period);
        Assert.IsTrue(countAllValues - countResultValues == _context.Values.Count());
    }
}