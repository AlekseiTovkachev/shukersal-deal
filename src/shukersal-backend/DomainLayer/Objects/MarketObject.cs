﻿using Microsoft.EntityFrameworkCore;
using shukersal_backend.ExternalServices.ExternalDeliveryService;
using shukersal_backend.ExternalServices.ExternalPaymentService;
using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.DomainLayer.Objects
{
    public class MarketObject
    {
        private MarketDbContext _context;
        private readonly PaymentProxy _paymentProvider;
        private readonly DeliveryProxy _deliveryProvider;
        private PurchaseRuleObject _purchaseRuleObject;
        private DiscountObject _discountObject;

        public MarketObject(MarketDbContext context)
        {
            _context = context;
            _paymentProvider = new PaymentProxy();
            _deliveryProvider = new DeliveryProxy();
            _purchaseRuleObject = new PurchaseRuleObject(_context);
            _discountObject = new DiscountObject(_context);
        }
        public async Task<Response<IEnumerable<Store>>> GetStores()
        {
            var stores = await _context.Stores
                //.Include(s => s.Products)
                .Include(s => s.DiscountRules).ToListAsync();
            return Response<IEnumerable<Store>>.Success(HttpStatusCode.OK, stores);
        }


        public async Task<Response<Store>> GetStore(long id)
        {
            try
            {
                if (_context.Stores == null)
                {
                    return Response<Store>.Error(HttpStatusCode.NotFound, "Entity set 'StoreContext.Stores' is null.");
                }
                var store = await _context.Stores
                    //.Include(s => s.Products)
                    .Include(s => s.DiscountRules)
                    .Include(s => s.PurchaseRules)
                    .FirstOrDefaultAsync(s => s.Id == id);
                if (store == null)
                {
                    return Response<Store>.Error(HttpStatusCode.NotFound, "Not found");
                }
                return Response<Store>.Success(HttpStatusCode.OK, store);
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error message
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Response<Store>.Error(HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
            }
        }

        //public async Task<Response<Store>> GetStore(long id)
        //{
        //    if (_context.Stores == null)
        //    {
        //        return Response<Store>.Error(HttpStatusCode.NotFound, "Entity set 'StoreContext.Stores'  is null.");
        //    }
        //    var store = await _context.Stores
        //        //.Include(s => s.Products)
        //        .Include(s => s.DiscountRules)
        //        .Include(s => s.PurchaseRules)
        //        .FirstOrDefaultAsync(s => s.Id == id);
        //    if (store == null)
        //    {
        //        return Response<Store>.Error(HttpStatusCode.NotFound, "Not found");
        //    }
        //    return Response<Store>.Success(HttpStatusCode.OK, store);
        //}

        public async Task<Response<Store>> GetStore(string name)
        {
            if (_context.Stores == null)
            {
                return Response<Store>.Error(HttpStatusCode.NotFound, "Entity set 'StoreContext.Stores'  is null.");
            }
            var store = await _context.Stores
                //.Include(s => s.Products)
                .Include(s => s.DiscountRules)
                .Include(s => s.PurchaseRules)
                .FirstOrDefaultAsync(s => s.Name == name);
            if (store == null)
            {
                return Response<Store>.Error(HttpStatusCode.NotFound, "Not found");
            }
            return Response<Store>.Success(HttpStatusCode.OK, store);
        }

        public async Task<Response<Store>> CreateStore(StorePost storeData, Member member)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var store = new Store
                    {
                        Name = storeData.Name,
                        Description = storeData.Description,
                        Products = new List<Product>(),
                        DiscountRules = new List<DiscountRule>(),
                        PurchaseRules = new List<PurchaseRule>()
                    };

                    _context.Stores.Add(store);
                    await _context.SaveChangesAsync(); // Save changes to generate the store ID

                    var storeManager = new StoreManager
                    {
                        MemberId = member.Id,
                        StoreId = store.Id,
                        Store = store,
                        StorePermissions = new List<StorePermission>(),
                        Username = member.Username
                    };
                    _context.StoreManagers.Add(storeManager);
                    await _context.SaveChangesAsync(); // Save changes to generate the manager ID

                    var permission = new StorePermission
                    {
                        StoreManager = storeManager,
                        StoreManagerId = storeManager.Id,
                        PermissionType = PermissionType.Manager_permission
                    };

                    storeManager.StorePermissions.Add(permission);

                    store.RootManager = storeManager;
                    store.RootManagerId = storeManager.Id;

                    await _context.SaveChangesAsync(); // Save changes to update the RootManagerId

                    transaction.Commit(); // Commit the transaction

                    return Response<Store>.Success(HttpStatusCode.Created, store);
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Rollback the transaction if an exception occurs
                                            // Handle the exception here
                }
            }

            return Response<Store>.Error(HttpStatusCode.BadRequest, "database transaction error");
        }

        public async Task<Response<bool>> UpdateStore(long id, StorePatch patch, Member member)
        {

            var manager = await _context.StoreManagers
                .Include(m => m.StorePermissions)
                .FirstOrDefaultAsync(m => m.MemberId == member.Id && m.StoreId == id);

            if (manager == null)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "The user is not authorized to update store");
            }

            //bool hasPermission = manager.StorePermissions.Any(p => p.PermissionType == PermissionType.Manager_permission);

            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Store not found");
            }
            if (store.RootManagerId != manager.Id)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "Only the root manager can change the store name");
            }


            if (patch.Description != null) store.Description = patch.Description;
            if (patch.Name != null) store.Name = patch.Name;

            //_context.Entry(store).State = EntityState.Modified;
            _context.MarkAsModified(store);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
                {
                    return Response<bool>.Error(HttpStatusCode.NotFound, "not found");
                }
                else
                {
                    throw;
                }
            }

            return Response<bool>.Success(HttpStatusCode.NoContent, true);
        }

        public async Task<Response<bool>> DeleteStore(long id, Member member)
        {
            if (_context.Stores == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Entity set 'StoreContext.Stores' is null.");
            }
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Store not found");
            }

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();

            return Response<bool>.Success(HttpStatusCode.NoContent, true);
        }

        public async Task<Response<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Response<IEnumerable<Category>>.Success(HttpStatusCode.OK, categories);
        }

        private bool StoreExists(long id)
        {
            return _context.Stores.Any(e => e.Id == id);
        }


        #region Transaction Functionality
        // *------------------------------------------------- Transaction Functionality --------------------------------------------------*

        public DeliveryProxy getDeliveryProxy() { return _deliveryProvider; }

        public PaymentProxy getPaymentProxy() { return _paymentProvider; }

        public void SetPaymentProvider(string url)
        {
            _paymentProvider.SetPaymentProvider(url);
        }

        public void SetDeliveryProvider(string url)
        {

            _deliveryProvider.SetDeliveryProvider(url);
        }

        public bool TransactionExists(long id)
        {
            return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        #endregion

        #region ShoppingCart Functionality
        // *------------------------------------------------- ShoppingCart Functionality --------------------------------------------------*

        public async Task<Utility.Response<ShoppingCart>> GetShoppingCartByUserId(long memberId)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ShoppingBaskets)
                .ThenInclude(b => b.ShoppingItems)
                .FirstOrDefaultAsync(c => c.MemberId == memberId);
            if (shoppingCart == null)
            {
                return Utility.Response<ShoppingCart>.Error(HttpStatusCode.NotFound, "User's shopping cart not found.");
            }
            return Utility.Response<ShoppingCart>.Success(HttpStatusCode.OK, shoppingCart);

        }

        public async Task<Utility.Response<ShoppingCart>> GetShoppingCartById(long cartId)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ShoppingBaskets)
                .ThenInclude(b => b.ShoppingItems)
                .FirstOrDefaultAsync(c => c.Id == cartId);
            if (shoppingCart == null)
            {
                return Utility.Response<ShoppingCart>.Error(HttpStatusCode.NotFound, "Shopping cart not found.");
            }
            return Utility.Response<ShoppingCart>.Success(HttpStatusCode.OK, shoppingCart);
        }

        public async Task<Response<IEnumerable<ShoppingBasketObject>>> EmptyCart(long memberId)
        {
            var cartResp = await GetShoppingCartByUserId(memberId);
            if (cartResp == null || cartResp.Result == null)
            {
                return Response<IEnumerable<ShoppingBasketObject>>.Error(HttpStatusCode.NotFound, "Shopping cart not found.");
            }
            var cart = new ShoppingCartObject(_context, cartResp.Result);
            var emptied = await cart.EmptyCart();

            return emptied;
        }


        public async Task<Response<bool>> CheckPurchasePolicy(long storeId, List<TransactionItem> TransactionItems)
        {
            try
            {
                var shop = await GetStore(storeId);
                if (shop.Result == null)
                {
                    return Response<bool>.Success(HttpStatusCode.BadRequest, false);
                }
                var pr = new PurchaseRuleObject(_context);
                if (!pr.Evaluate((await pr.GetAppliedPurchaseRule(storeId)).Result, TransactionItems))
                {
                    return Response<bool>.Success(HttpStatusCode.BadRequest, false);
                }

                return Response<bool>.Success(HttpStatusCode.NoContent, true);
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error message
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Response<bool>.Error(HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
            }
        }


        //public async Task<Response<bool>> CheckPurchasePolicy(long storeId, List<TransactionItem> TransactionItems)
        //{
        //    var shop = await GetStore(storeId);
        //    if (shop.Result == null)
        //    {
        //        return Response<bool>.Success(HttpStatusCode.BadRequest, false);
        //    }
        //    var pr = new PurchaseRuleObject(_context);
        //    if (!pr.Evaluate((await pr.GetAppliedPurchaseRule(storeId)).Result, TransactionItems))
        //    {
        //        return Response<bool>.Success(HttpStatusCode.BadRequest, false);
        //    }

        //    return Response<bool>.Success(HttpStatusCode.NoContent, true);

        //}

        public async Task<Response<bool>> confirmDeliveryAndPayment(DeliveryDetails deliveryDetails, List<TransactionItem> transactionItems, PaymentDetails paymentDetails)
        {
            //connction with external payment service
            bool paymentConfirmed = _paymentProvider.ConfirmPayment(paymentDetails);

            if (!paymentConfirmed)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, "Payment declined");

            }

            //connction with external delivery service
            bool deliveryConfirmed = _deliveryProvider.ConfirmDelivery(deliveryDetails, transactionItems);
            if (!deliveryConfirmed)
            {
                _paymentProvider.CancelPayment(transactionItems.ElementAt(0).TransactionId);
                return Response<bool>.Error(HttpStatusCode.BadRequest, "Delivey declined");

            }

            return Response<bool>.Success(HttpStatusCode.BadRequest, true);
        }

        public async Task<Response<bool>> UpdateStock(long storeId, List<TransactionItem> TransactionItems)
        {
            Dictionary<long, int> updated = new Dictionary<long, int>();
            var shop = await GetStore(storeId);
            if (shop.Result == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, shop.ErrorMessage);
            }

            foreach (var TransactionItem in TransactionItems)
            {
                var product = await _context.Products.FindAsync(TransactionItem.ProductId);
                if (product == null) { return Response<bool>.Error(HttpStatusCode.NotFound, "Product does not exist"); }
                if (product.UnitsInStock >= TransactionItem.Quantity)
                {
                    product.UnitsInStock = product.UnitsInStock - TransactionItem.Quantity;
                    await _context.SaveChangesAsync();
                    updated.Add(product.Id, TransactionItem.Quantity);
                }
                else
                {
                    //rollback
                    foreach (KeyValuePair<long, int> itemQuantityPair in updated)
                    {
                        var productToRollBack = await _context.Products.FindAsync(itemQuantityPair.Key);
                        if (productToRollBack != null)
                        {
                            productToRollBack.UnitsInStock = productToRollBack.UnitsInStock + itemQuantityPair.Value;
                            await _context.SaveChangesAsync();
                        }

                    }
                    return Response<bool>.Error(HttpStatusCode.BadRequest, "Product's qunatity is unavailable in store");

                }

            }
            return Response<bool>.Success(HttpStatusCode.NoContent, true);
        }

        public async Task<Response<bool>> CheckAvailabilityInStock(long storeId, List<TransactionItem> TransactionItems)
        {
            var shop = await GetStore(storeId);
            if (shop.Result == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, shop.ErrorMessage);
            }

            foreach (var TransactionItem in TransactionItems)
            {
                var product = await _context.Products.FindAsync(TransactionItem.ProductId);
                if (product == null) { return Response<bool>.Error(HttpStatusCode.NotFound, "Product does not exist"); }
                if (product.UnitsInStock < TransactionItem.Quantity) { return Response<bool>.Error(HttpStatusCode.BadRequest, "Product's qunatity is unavailable in store"); }
            }

            return Response<bool>.Success(HttpStatusCode.NoContent, true);

        }

        #endregion
    }
}
