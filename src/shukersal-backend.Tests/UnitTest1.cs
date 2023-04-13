using Xunit.Abstractions;

namespace shukersal_backend.Tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;
        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void Test1()
        {
            output.WriteLine("hello world");
            output.WriteLine("hello world");

            output.WriteLine("hello world");

        }
    }
}