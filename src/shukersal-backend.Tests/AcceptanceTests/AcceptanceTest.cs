using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    internal class AcceptanceTest
    {
        public IBridge bridge;
        public ITestOutputHelper output;
        public AcceptanceTest(ITestOutputHelper output) {
            this.output = output;

            //uncomment the following line when using the proxy bridge
            //bridge = new ProxyBridge();

            //uncomment the following line when using the real bridge
            bridge = new Bridge();
        }
    }
}
