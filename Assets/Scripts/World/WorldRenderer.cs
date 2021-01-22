using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {WorldRenderer}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/

    /* --- Info --- */

    /* --- Stats --- */

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

    void Update()
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        for (int i = 0; i < characters.Length; i++)
        {
            CharacterState characterState = characters[i].GetComponent<CharacterState>();
            if (spriteRenderers.Count > 0)
            {
                bool inserted = false;
                for (int j = 0; j < spriteRenderers.Count; j++)
                {
                    if (characterState.depth < characters[j].GetComponent<CharacterState>().depth)
                    {
                        spriteRenderers.Insert(j, characterState.spriteRenderer);
                        inserted = true;
                        break;
                    }
                }
                if (!inserted)
                {
                    spriteRenderers.Add(characterState.spriteRenderer);
                }

            }
            else { spriteRenderers.Add(characterState.spriteRenderer); }
        }

        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            print(spriteRenderers[i].name + ", " + i.ToString());
            spriteRenderers[i].sortingOrder = i;
        }
    }

    void OnMouseDown()
    {
    }
}
