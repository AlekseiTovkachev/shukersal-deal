using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace shukersal_backend.Controllers.StoreControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly StoreContext _context;
        private StoreManager _managerDummy;

        public StoreController(StoreContext context)
        {
            _context = context;
            _managerDummy = new StoreManager();
        }

        // GET: api/Store
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
            if (_context.Stores == null)
            {
                return NotFound();
            }
            var stores = await _context.Stores.ToListAsync();
            return stores;
        }

        // GET: api/Store/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStore(long id)
        {
            var store = await _context.Stores.FindAsync(id);

            if (store == null)
            {
                return NotFound();
            }

            return store;
        }

        // POST: api/Store
        [HttpPost]
        public async Task<ActionResult<Store>> CreateStore(StorePost storeData)
        {
            //TODO: check if the manager already exists in the system
            //TODO: retrieve the manager from manager context in manager controller
            if (_context.Stores == null)
            {
                return Problem("Entity set 'MemberContext.Members'  is null.");
            }
            if (ModelState.IsValid)
            {
                var store = new Store(
                        storeData.Name,
                        storeData.Description,
                        storeData.RootManagerId
                        );

                _context.Stores.Add(store);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetStore", new { id = store.Id }, store);
            }
            else
            {
                return BadRequest(ModelState);
            }           
        }

        // PUT: api/Store/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStore(long id, Store store)
        {
            if (id != store.Id)
            {
                return BadRequest();
            }

            _context.Entry(store).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Store/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(long id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StoreExists(long id)
        {
            return _context.Stores.Any(e => e.Id == id);
        }

        // Action method for adding a product to a store
        [HttpPost("stores/{storeId}/products")]
        public async Task<IActionResult> AddProduct(long storeId, Product product)
        {
            var store = await _context.Stores.FindAsync(storeId);

            if (store == null)
            {
                return NotFound("Store not found.");
            }

            // Associate the product with the store
            product.StoreId = storeId;
            product.Store = store;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok("Product added successfully.");
        }

        // Action method for updating a product in a store
        [HttpPut("stores/{storeId}/products")]
        public async Task<IActionResult> UpdateProduct(long storeId, long productId, Product product)
        {
            var store = await _context.Stores.FindAsync(storeId);

            if (store == null)
            {
                return NotFound("Store not found.");
            }

            var existingProduct = await _context.Products.FindAsync(productId);

            if (existingProduct == null || existingProduct.StoreId != storeId)
            {
                return NotFound("Product not found in the specified store.");
            }

            // Update the existing product with the new data
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;

            await _context.SaveChangesAsync();

            return Ok("Product updated successfully.");
        }

        // Action method for deleting a product from a store
        [HttpDelete("stores/{storeId}/products")]
        public async Task<IActionResult> DeleteProduct(long storeId, long productId)
        {
            var store = await _context.Stores.FindAsync(storeId);

            if (store == null)
            {
                return NotFound("Store not found.");
            }

            var product = await _context.Products.FindAsync(productId);

            if (product == null || product.StoreId != storeId)
            {
                return NotFound("Product not found in the specified store.");
            }

            // Remove the product from the store
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product deleted successfully.");
        }
    }
}
