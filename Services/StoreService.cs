using Google.Cloud.Firestore;
using StoreAPI.Models;
using System;
using System.Threading.Tasks;

namespace StoreAPI.Services
{
    /// <summary>
    /// Provides methods for interacting with the stores and API keys collections in the Firestore database.
    /// </summary>
    public class StoreService
    {
        private readonly FirestoreDb db;

        /// <summary>
        /// Creates a new instance of the <see cref="StoreService"/> class.
        /// </summary>
        public StoreService()
        {
            db = FirestoreDb.Create("neplode-358223");
        }

        /// <summary>
        /// Gets the store with the specified name from the stores collection in the Firestore database.
        /// </summary>
        /// <param name="query_name">The name of the store to retrieve.</param>
        /// <returns>A <see cref="Store"/> object representing the store, or null if no store with the specified name was found.</returns>
        public async Task<Store?> Get(string query_name)
        {
            try
            {
                DocumentSnapshot snapshot = await db.Collection("stores").Document(query_name).GetSnapshotAsync();
                if (!snapshot.Exists)
                {
                    return null;
                }

                Store store = new Store();
                store.Name = snapshot.GetValue<string>("name") ?? throw new NullReferenceException("The 'name' field in the store document is null.");
                store.Description = snapshot.GetValue<string>("description") ?? throw new NullReferenceException("The 'description' field in the store document is null.");

                return store;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while trying to get the store with name '{query_name}': {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Determines whether the specified API key is valid.
        /// </summary>
        /// <param name="key">The API key to validate.</param>
        /// <returns>True if the API key is valid, false otherwise.</returns>
        public async Task<bool> IsAPIKeyValid(string key)
        {
            try
            {
                DocumentSnapshot snapshot = await db.Collection("api_keys").Document(key).GetSnapshotAsync();
                if (!snapshot.Exists || !snapshot.GetValue<bool>("valid"))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while trying to validate the API key '{key}': {ex.Message}");
                return false;
            }
        }
    }
}
