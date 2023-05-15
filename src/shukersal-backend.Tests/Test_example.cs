using Xunit.Abstractions;

namespace shukersal_backend.Tests
{
    public class Test_example
    {
        private readonly ITestOutputHelper output;
        public Test_example(ITestOutputHelper output)
        {
            this.output = output;
        }
        //[Fact]
        public void Test1()
        {
            output.WriteLine("hello world");
            output.WriteLine("hello world");

            output.WriteLine("hello world");

        }
    }
}