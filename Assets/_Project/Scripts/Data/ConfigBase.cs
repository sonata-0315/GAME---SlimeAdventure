using UnityEngine;

namespace PrecisionPlatformer.Data
{
    /// <summary>
    /// Base class for all ScriptableObject configurations.
    /// Demonstrates the pattern students should follow when creating config files.
    /// </summary>
    public abstract class ConfigBase : ScriptableObject
    {
        [Header("Config Info")]
        [Tooltip("Description of this configuration")]
        [TextArea(2, 4)]
        public string description = "Enter config description here";

        /// <summary>
        /// Virtual method for validation. Override in derived classes to add custom validation.
        /// </summary>
        public virtual bool Validate()
        {
            return true;
        }

        /// <summary>
        /// Called when values are changed in the Inspector (Editor only).
        /// Override this to add custom validation or clamping logic.
        /// </summary>
        protected virtual void OnValidate()
        {
            if (!Validate())
            {
                Debug.LogWarning($"{GetType().Name}: Configuration validation failed! Check values.", this);
            }
        }
    }
}
