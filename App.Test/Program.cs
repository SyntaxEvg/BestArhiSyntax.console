using AutoMapper;
using Xunit;

//internal class Program
//{
//    private static void Main(string[] args)
//    {
//        Console.WriteLine("Hello, World!");
//    }
//}
public class TestAutoMapperConfig
{
    [Fact]
    public void TestSummaryMapping()
    {
        // MapperConfiguration config = CartToSummaryMapper.GetMapperConfiguration();
        MapperConfiguration config = new MapperConfiguration(new MapperConfigurationExpression());
        config.AssertConfigurationIsValid();

    }
}