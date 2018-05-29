using UnityEngine;

public interface IMechanicsData
{
    /// <summary>
    /// Gets or sets spell prefab.
    /// </summary>
    GameObject spellPrefab { get; set; }

    /// <summary>
    /// Gets or sets instantieted object.
    /// </summary>
    GameObject spawnedObject { get; set; }

    /// <summary>
    /// Gets or sets action key 1.
    /// </summary>
    KeyCode actionKey1 { get; set; }

    /// <summary>
    /// Gets or sets action key 2.
    /// </summary>
    KeyCode actionKey2 { get; set; }

    /// <summary>
    /// Gets or sets action key 3.
    /// </summary>
    KeyCode actionKey3 { get; set; }
}
