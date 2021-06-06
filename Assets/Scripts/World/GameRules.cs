using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRules : MonoBehaviour
{
    /*--- Components ---*/

    /* --- Internal Variables --- */
    [HideInInspector] public static float gameDuration = 90f; // in seconds
    [HideInInspector] public static float gameTime = 0f;
    /* --- Unity Methods --- */
    void Start()
    {
        GameRules.gameTime = 0f;
    }

    void FixedUpdate()
    {
        GameRules.gameTime = GameRules.gameTime + Time.fixedDeltaTime;
    }
}
