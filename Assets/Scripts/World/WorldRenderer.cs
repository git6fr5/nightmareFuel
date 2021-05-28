using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {WorldRenderer}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/
    public GameObject plusPrefab;

    /* --- Internal Variables --- */
    [HideInInspector] public float plusTime = 0f;
    private float plusSpawnTime = 3f;
    private float worldBound = 10f;
    private Plus plus;

    /* --- Stats --- */

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

    void Update()
    {
        MinimumSort();
    }

    void FixedUpdate()
    {
        SpawnPlus();
    }

    void OnMouseDown()
    {
    }

    /* --- Methods --- */
    public static void InsertSort()
    {
        // Declare the object array and the array of sorted characters
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        List<CharacterRenderer> characterRenderers = new List<CharacterRenderer>();

        // There is no need to sort if there is less than two characters
        if (characters.Length < 2) { return; }

        // Add the first input into the characters array
        characterRenderers.Add(characters[0].GetComponent<CharacterRenderer>());

        // Itterate through the rest of the list
        for (int i = 1; i < characters.Length; i++)
        {
            CharacterRenderer characterRenderer  = characters[i].GetComponent<CharacterRenderer>();
            bool inserted = false;
            for (int j = 0; j < characterRenderers.Count; j++)
            {
                // The larger the depth, the further behind the character is
                if (characterRenderer.depth > characters[j].GetComponent<CharacterRenderer>().depth)
                {
                    characterRenderers.Insert(j, characterRenderer);
                    inserted = true;
                    break;
                }
            }
            if (!inserted)
            {
                characterRenderers.Add(characterRenderer);
            }
        }

        for (int i = 0; i < characterRenderers.Count; i++)
        {
            print(characterRenderers[i].name + ", " + i.ToString());
            characterRenderers[i].spriteRenderer.sortingOrder = i;
        }
    }

    public static void MinimumSort()
    {
        // Declare the object array and the array of sorted characters
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Mob");
        List<CharacterRenderer> characterRenderers = new List<CharacterRenderer>();
        List<float> characterDepths = new List<float>();

        // There is no need to sort if there is less than two characters
        if (characters.Length < 2) { return; }

        // Itterate through the rest of the list
        for (int i = 0; i < characters.Length; i++)
        {
            CharacterRenderer characterRenderer = characters[i].GetComponent<CharacterRenderer>();
            characterDepths.Add(characterRenderer.depth);
        }
        
        while (characterDepths.Count > 0)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                CharacterRenderer characterRenderer = characters[i].GetComponent<CharacterRenderer>();
                if (characterRenderer.depth == characterDepths.Max())
                {
                    characterRenderers.Add(characterRenderer);
                    characterDepths.Remove(characterDepths.Max());
                    if (characterDepths.Count == 0) { break; }
                }
            }
        }

        for (int i = 0; i < characterRenderers.Count; i++)
        {
            //print(characterRenderers[i].name + ", " + i.ToString());
            characterRenderers[i].spriteRenderer.sortingOrder = i;
        }
    }

    void SpawnPlus()
    {
        if (plus == null) { plusTime = plusTime + Time.fixedDeltaTime; }
        if (plusTime >= plusSpawnTime)
        {
            float magnitude = Random.Range(worldBound / 4, worldBound / 2);
            Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector3 randomVector = direction * magnitude;
            Vector3 nonRandomVector = new Vector2(1, 0) * magnitude;
            plus = Instantiate(plusPrefab, randomVector, Quaternion.identity, transform).GetComponent<Plus>();
            //plusObject.SetActive(true);
            plus.plusValue = plusTime;
            plusTime = 0;
        }
    }

}
