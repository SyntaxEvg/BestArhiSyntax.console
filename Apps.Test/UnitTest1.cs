using AutoMapper;

namespace Apps.Test
{
    public class UnitTest1
    {
        [Fact]
        public void TestMapping()
        {
            // MapperConfiguration config = CartToSummaryMapper.GetMapperConfiguration();
            MapperConfiguration config = new MapperConfiguration(new MapperConfigurationExpression());
            config.AssertConfigurationIsValid();

        }
    }
}