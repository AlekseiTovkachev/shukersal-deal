using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;

namespace shukersal_backend.ServiceLayer
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class TransactionService : ControllerBase
    {
        private readonly TransactionController TransactionController;


        public TransactionService(MarketDbContext context)
        {
            TransactionController = new TransactionController(context);
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            var response = await TransactionController.GetTransactions();
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // GET: api/Transactions/5
        [HttpGet("Transactionid")]
        public async Task<ActionResult<Transaction>> GetTransaction(long TransactionId)
        {
            var response = await TransactionController.GetTransaction(TransactionId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }



        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> TransactionAShoppingCart(TransactionPost TransactionPost)
        {
            if (ModelState.IsValid)
            {
                var response = await TransactionController.TransactionAShoppingCart(TransactionPost);
                if (!response.IsSuccess || response.Result == null)
                {
                    return BadRequest(response.ErrorMessage);
                }
                var Transaction = response.Result;
                return CreatedAtAction(nameof(GetTransaction), new { id = Transaction.Id }, Transaction);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Transactions/5
        [HttpDelete("Transactionid/{TransactionId}")]
        public async Task<IActionResult> DeleteTransaction(long TransactionId)
        {
            var response = await TransactionController.DeleteTransaction(TransactionId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return NoContent();
        }


        // PUT: api/Transaction/5
        [HttpPut("Transactionid/{Transactionid}")]
        public async Task<IActionResult> UpdateTransaction(long Transactionid, TransactionPost post)
        {
            if (ModelState.IsValid)
            {
                var response = await TransactionController.UpdateTransaction(Transactionid, post);
                if (!response.IsSuccess)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound(ModelState);
                    }
                }
                return Ok(response.Result);

            }
            return BadRequest();
        }

        // GET: api/Transactions/memberId/5
        [HttpGet("memberId/{memberId}")]
        public async Task<ActionResult<Transaction>> BroweseTransactionHistory(long memberId)
        {
            if (ModelState.IsValid)
            {
                var response = await TransactionController.BroweseTransactionHistory(memberId);
                if (!response.IsSuccess)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound(ModelState);
                    }
                }
                return Ok(response.Result);

            }
            return BadRequest();
        }

        // GET: api/Transactions/storeId/5
        [HttpGet("storeId/{storeId}")]
        public async Task<ActionResult<Transaction>> BroweseShopTransactionHistory(long storeId)
        {
            if (ModelState.IsValid)
            {
                var response = await TransactionController.BroweseShopTransactionHistory(storeId);
                if (!response.IsSuccess)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound(ModelState);
                    }
                }
                return Ok(response.Result);

            }
            return BadRequest();
        }

    }
}
