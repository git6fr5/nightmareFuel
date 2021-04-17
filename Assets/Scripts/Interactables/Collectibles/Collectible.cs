using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    /* --- Debug --- */
    protected string DebugTag = "[DungeonCrawler Gem]: ";
    protected bool DEBUG_init = false;


    /* --- Components --- */
    public Collider2D hitBox;
    public Sprite portrait;


    /* --- Internal Variables ---*/
    [HideInInspector] public bool isCollectible = true;

    /* --- Unity Methods --- */
    public virtual void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }


    /* --- Methods --- */
    public virtual void Activate()
    {

    }
}
