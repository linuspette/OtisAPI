using OtisAPI.Infrastructure;

namespace OtisAPI.Test.Infrastructure;

public class ErrandNumberGenerator_Tests
{
    public ErrandNumberGenerator_Tests()
    {

    }

    [Fact]
    public void Test_GenerateErrandNumber()
    {
        var errandNumber = ErrandNumberGenerator.GenerateErrandNumber();

        Assert.InRange(errandNumber.Length, 18, 18);
    }
}