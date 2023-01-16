using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;
using StoreAPI.Services;
using System;
using System.Threading.Tasks;

namespace StoreAPI.Controllers
{
    /// <summary>
    /// Provides a RESTful API for interacting with stores in the database.
    /// </summary>
    [Route("api/store")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly StoreService storeService;

        /// <summary>
        /// Creates a new instance of the <see cref="StoreController"/> class.
        /// </summary>x
        public StoreController()
        {
            this.storeService = new StoreService();
        }

        /// <summary>
        /// Gets the store with the specified name.
        /// </summary>
        /// <param name="api_key">The API key to use for authentication.</param>
        /// <param name="name">The name of the store to retrieve.</param>
        /// <returns>A <see cref="Store"/> object representing the store, or a 404 Not Found response if the store does not exist or the API key is invalid.</returns>
        [HttpGet("{api_key}/{name}")]
        public async Task<ActionResult<Store>> Get(string api_key, string name)
        {
            try
            {
                if (!await storeService.IsAPIKeyValid(api_key))
                {
                    return NotFound();
                }

                Store? store = await storeService.Get(name);
                if (store == null)
                {
                    return NotFound();
                }

                return store;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while trying to get the store with name '{name}': {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
