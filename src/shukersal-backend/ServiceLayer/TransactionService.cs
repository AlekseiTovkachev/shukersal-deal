﻿using HotelBackend.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.DomainLayer.notifications;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Data;
using System.Net;

namespace shukersal_backend.ServiceLayer
{
    [Route("api/transactions")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class TransactionService : ControllerBase
    {
        private readonly TransactionController TransactionController;
        private readonly ILogger<ControllerBase> logger;
        private readonly MarketDbContext context;
        private readonly NotificationController _notificationController;

        public TransactionService(MarketDbContext context, ILogger<ControllerBase> logger, NotificationController notificationController)
        {
            TransactionController = new TransactionController(context, notificationController);
            this.logger = logger;
            this.context = context;
            
        }

        // GET: api/Transactions
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.AdministratorGroup)]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            logger.LogInformation("GetTransactions method called.");
            var currentMember= ServiceUtilities.GetCurrentMember(context, HttpContext);
            if ( currentMember== null)
            {
                return Unauthorized();
            }
            var response = await TransactionController.GetTransactions();
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // GET: api/Transactions/5
        [HttpGet("{TransactionId}")]
        public async Task<ActionResult<Transaction>> GetTransaction(long TransactionId)
        {
            logger.LogInformation("GetTransaction method called with id = {TransactionId}", TransactionId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            var response = await TransactionController.GetTransaction(TransactionId,currentMember.Id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }



        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PurchaseAShoppingCart(TransactionPost TransactionPost)
        {
            logger.LogInformation("PurchaseAShoppingCart method called");
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);

            if(currentMember == null)
            {
                TransactionPost.IsMember = false;
            }
            else
            {
                TransactionPost.IsMember= true;
                TransactionPost.MemberId=currentMember.Id;
            }
            if (ModelState.IsValid)
            {
                var response = await TransactionController.PurchaseAShoppingCart(TransactionPost);
                if (!response.IsSuccess || response.Result == null)
                {
                    return BadRequest(response.ErrorMessage);
                }
                var Transaction = response.Result;
                return CreatedAtAction(nameof(GetTransaction), new { TransactionId = Transaction.Id }, Transaction);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{TransactionId}")]
        public async Task<IActionResult> DeleteTransaction(long TransactionId)
        {
            logger.LogInformation("DeleteTransaction method called with id = {TransactionId}", TransactionId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            var response = await TransactionController.DeleteTransaction(TransactionId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return NoContent();
        }


        // PUT: api/Transaction/5
        [HttpPut("{Transactionid}")]
        public async Task<IActionResult> UpdateTransaction(long Transactionid, TransactionPost post)
        {
            logger.LogInformation("UpdateTransaction method called with id = {Transactionid}", Transactionid);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
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
        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<Transaction>> BrowseTransactionHistory(long memberId)
        {
            logger.LogInformation("BrowseTransactionHistory method called with memberId = {memberId}", memberId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await TransactionController.BrowseTransactionHistory(memberId,currentMember.Id);
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
        [HttpGet("store/{storeId}")]
        public async Task<ActionResult<Transaction>> BrowseShopTransactionHistory(long storeId)
        {
            logger.LogInformation("BrowseTransactionHistory method called with storeId = {storeId}", storeId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await TransactionController.BrowseShopTransactionHistory(storeId,currentMember.Id);
                if (!response.IsSuccess)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound(ModelState);
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return Unauthorized();
                    }
                }
                return Ok(response.Result);

            }
            return BadRequest();
        }

    }
}
