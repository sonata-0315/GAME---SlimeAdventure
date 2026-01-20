namespace PrecisionPlatformer.Interfaces
{
    /// <summary>
    /// Interface for objects that can be pooled and reused.
    /// Implement this interface for any objects that will be frequently spawned/despawned
    /// (e.g., particles, projectiles, collectibles).
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Called when object is retrieved from the pool and activated.
        /// Use this to reset the object to its initial state.
        /// </summary>
        void OnSpawnFromPool();

        /// <summary>
        /// Called when object is returned to the pool and deactivated.
        /// Use this to clean up state before returning to pool.
        /// </summary>
        void OnReturnToPool();
    }
}
