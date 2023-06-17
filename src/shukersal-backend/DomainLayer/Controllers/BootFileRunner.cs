using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using shukersal_backend.DomainLayer.notifications;
using shukersal_backend.Models;

namespace shukersal_backend.DomainLayer.Controllers
{

    public class BootFileRunner
    {
        public static async Task<bool> Run(MarketDbContext context)
        {
            MarketDbContext _context = context;
            MemberController memberController = new MemberController(context);
            StoreManagerController managerController = new StoreManagerController(context, null);
            StoreController storeController = new StoreController(context, null);

            string projectFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            string bootFilesFolderPath = Path.Combine(projectFolderPath, "BootFiles");

            var members = await AddMembers(memberController, bootFilesFolderPath);

            var stores = await AddStores(storeController, bootFilesFolderPath, members);

            var managers = await AddManagers(managerController, bootFilesFolderPath, members, stores);

            var products = await AddProducts(storeController, managerController, bootFilesFolderPath, members, managers);

            return true;

        }


        private static async Task<List<Member>> AddMembers(MemberController controller, string jsonPath)
        {
            string jsonFilePath = Path.Combine(jsonPath, "members_bootfile.json");

            string jsonMembersContent = File.ReadAllText(jsonFilePath);
            List<MemberPost>? memberPosts;

            memberPosts = JsonConvert.DeserializeObject<List<MemberPost>>(jsonMembersContent);
            var members = new List<Member>();

            if (memberPosts == null)
                return members;

            foreach (var member in memberPosts)
            {
                var existingMemberRes = await controller.GetMember(member.Username);
                var existingMember = existingMemberRes.Result;
                if (existingMember != null)
                    members.Add(existingMember);
                else
                {
                    var res = await controller.AddMember(member);
                    if (res.IsSuccess && res.Result != null)
                    {
                        members.Add(res.Result);
                    }
                }

            }
            return members;
        }

        private static async Task<List<Store>> AddStores(StoreController controller, string jsonPath, List<Member> members)
        {
            List<Store> stores = new List<Store>();
            jsonPath = Path.Combine(jsonPath, "stores_bootfile.json");

            string jsonStoresContent = File.ReadAllText(jsonPath);

            JArray jsonArray = JArray.Parse(jsonStoresContent);
            if (jsonArray.Count == 0)
                return stores;
            List<string>? memberUsernames = jsonArray[0].ToObject<List<string>>();
            JArray storePostsJsonArray = (JArray)jsonArray[1];
            List<StorePost>? storePosts = storePostsJsonArray.ToObject<List<StorePost>>();



            if (storePosts == null || memberUsernames == null)
            {
                return stores;
            }

            List<(string Username, StorePost Store)> pairedList = memberUsernames
                .Zip(storePosts, (username, storePost) => (Username: username, Store: storePost)).ToList();

            foreach (var item in pairedList)
            {
                var member = members.Where(m => m.Username == item.Username).FirstOrDefault();

                if (member != null)
                {
                    var res2 = await controller.GetStore(item.Store.Name);
                    if (res2.IsSuccess && res2.Result != null)
                    {
                        stores.Add(res2.Result);
                        continue;
                    }
                    var res = await controller.CreateStore(item.Store, member);
                    if (res.IsSuccess && res.Result != null)
                    {
                        stores.Add(res.Result);
                    }
                }
            }
            return stores;
        }

        private static async Task<List<StoreManager>> AddManagers
            (StoreManagerController manager_controller, string jsonPath, List<Member> members, List<Store> stores)
        {
            List<StoreManager> managers = new List<StoreManager>();


            string jsonFilePath = Path.Combine(jsonPath, "managers_bootfile.json");

            string jsonMembersContent = File.ReadAllText(jsonFilePath);

            List<ManagerBoot>? managerBoots = JsonConvert.DeserializeObject<List<ManagerBoot>>(jsonMembersContent);
            if (managerBoots == null)
            {
                return managers;
            }
            foreach (var managerBoot in managerBoots)
            {
                var member_boss = members.Where(m => m.Username == managerBoot.BossMemberName).FirstOrDefault();
                var member_subordinate = members.Where(m => m.Username == managerBoot.NewManagerName).FirstOrDefault();
                var store = stores.Where(s => s.Name == managerBoot.StoreName).FirstOrDefault();
                if (member_boss != null && member_subordinate != null && store != null)
                {

                    var res = await manager_controller.GetStoreManagersByMemberId(member_boss.Id);
                    if (res.Result == null)
                    {
                        continue;
                    }
                    var boss = res.Result.Where(m => m.StoreId == store.Id).FirstOrDefault();
                    if (boss == null)
                    {
                        continue;
                    }
                    var manager_post = new OwnerManagerPost
                    {
                        BossId = boss.Id,
                        MemberId = member_subordinate.Id,
                        StoreId = store.Id,
                        Owner = managerBoot.Owner
                    };

                    Utility.Response<StoreManager> res3;
                    if (manager_post.Owner)
                        res3 = await manager_controller.PostStoreOwner(manager_post, member_boss);
                    else
                        res3 = await manager_controller.PostStoreManager(manager_post, member_boss);

                    if (res3.Result != null)
                        managers.Add(res3.Result);
                    else
                    {
                        var res4 = await manager_controller.GetStoreManagersByMemberId(member_subordinate.Id);
                        if (res4.Result == null)
                            continue;
                        var manager = res4.Result.Where(m => m.StoreId == store.Id).FirstOrDefault();
                        if (manager != null) managers.Add(manager);
                    }

                }

            }

            return managers;
        }

        private static async Task<List<Product>> AddProducts
            (StoreController storeController, StoreManagerController managerController, string jsonPath, List<Member> members, List<StoreManager> managers)
        {
            List<Product> products = new List<Product>();

            string jsonFilePath = Path.Combine(jsonPath, "products_bootfile.json");

            string jsonContent = File.ReadAllText(jsonFilePath);

            List<ProductBoot>? productBoots = JsonConvert.DeserializeObject<List<ProductBoot>>(jsonContent);
            if (productBoots == null)
            {
                return products;
            }
            foreach (var productBoot in productBoots)
            {
                var res1 = await storeController.GetStore(productBoot.StoreName);
                var store = res1.Result;
                if (store == null) continue;
                var productPost = new ProductPost
                {
                    Name = productBoot.Name,
                    Description = productBoot.Description,
                    Price = productBoot.Price,
                    UnitsInStock = productBoot.UnitsInStock,
                    CategoryId = productBoot.CategoryId,
                    IsListed = true
                };
                var manager = managers
                    .Where(m => m.Id == store.RootManagerId)
                    .FirstOrDefault();

                if (manager == null) continue;

                var member = members.Where(m => m.Id == manager.MemberId).FirstOrDefault();

                if (member == null) continue;

                var res3 = await storeController.AddProduct(store.Id, productPost, member);
                if (res3.Result != null) products.Add(res3.Result);

            }

            return products;
        }

    }




}
