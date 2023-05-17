using shukersal_backend.Models;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    internal class T2_4_09_CloseStore : AcceptanceTest
    {
        private long StoreId;
        private bool isInitFinished;
        public T2_4_09_CloseStore(ITestOutputHelper output) : base(output)
        {
            isInitFinished = false;
            Init();
            while (!isInitFinished) { }
        }
        private async void Init()
        {

            await bridge.Register(new RegisterPost { Username = "testUsername2409", Password = "testPassword" });
            await bridge.Login(new LoginPost { Username = "testUsername2409", Password = "testPassword" });
            var user = (await bridge.GetLoggedUser()).Value;
            var res = await bridge.CreateStore(new StorePost { Name = "store2409", Description = "" });
            if (res.Value != null)
                StoreId = res.Value.Id;
            isInitFinished = true;
        }
    }
}