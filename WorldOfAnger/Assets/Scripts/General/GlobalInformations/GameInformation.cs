using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains global informations inside of the game.
/// </summary>
public class GameInformation : MonoBehaviour {

    /// <summary>
    /// Gets or sets flag for stopping the time.
    /// </summary>
	public static bool TimeStoped { get; set; }

    private void Awake()
    {
        TimeStoped = false;
    }
}
