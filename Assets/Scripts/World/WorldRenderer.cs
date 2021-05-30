using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{

    /*--- Components ---*/
    public GameObject plusPrefab;
    public LayerMask opaqueLayer;

    /* --- Internal Variables --- */
    [HideInInspector] public float plusTime = 0f;
    private float plusSpawnTime = 3f;
    private float worldBound = 10f;
    private Plus plus;

    private List<CharacterRenderer> sortedCharacterRenderers = new List<CharacterRenderer>();
    private List<CharacterRenderer> unsortedCharacterRenderers = new List<CharacterRenderer>();
    private List<float> unsortedCharacterDepths = new List<float>();

    /* --- Stats --- */

    /*--- Unity Methods ---*/
    void Update()
    {
        MinimumSort(opaqueLayer);
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

    public static void MinimumSort(LayerMask layerMask)
    {
        // Declare the object array and the array of sorted characters
        float visionRadius = 20f;
        Collider2D[] unsortedColliders = Physics2D.OverlapCircleAll(Camera.main.transform.position, visionRadius, layerMask);
        List<CharacterRenderer> unsortedCharacterRenderers = new List<CharacterRenderer>();
        for (int i = 0; i < unsortedColliders.Length; i++)
        {
            Collider2D collider = unsortedColliders[i];
            if (collider.GetComponent<CharacterRenderer>() && collider.GetType() == typeof(CapsuleCollider2D))
            {
                unsortedCharacterRenderers.Add(collider.GetComponent<CharacterRenderer>());
            }
        }

        List<float> unsortedCharacterDepths = new List<float>();
        for (int i = 0; i < unsortedCharacterRenderers.Count; i++)
        {
            CharacterRenderer characterRenderer = unsortedCharacterRenderers[i];
            unsortedCharacterDepths.Add(characterRenderer.depth);
        }

        List<CharacterRenderer> sortedCharacterRenderers = new List<CharacterRenderer>();
        while (unsortedCharacterDepths.Count > 0)
        {
            int numCharacters = unsortedCharacterRenderers.Count;
            for (int i = 0; i < numCharacters; i++)
            {
                CharacterRenderer characterRenderer = unsortedCharacterRenderers[i];
                if (characterRenderer.depth == unsortedCharacterDepths.Max())
                {
                    sortedCharacterRenderers.Add(characterRenderer);
                    unsortedCharacterRenderers.Remove(characterRenderer);
                    unsortedCharacterDepths.Remove(unsortedCharacterDepths.Max());
                    break;
                }
            }
        }

        for (int i = 0; i < sortedCharacterRenderers.Count; i++)
        {
            //print(characterRenderers[i].name + ", " + i.ToString());
            sortedCharacterRenderers[i].spriteRenderer.sortingOrder = i;
        }
    }

    void AddToList(GameObject[] gameObjects, List<CharacterRenderer> _unsortedCharacterRenderers)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            CharacterRenderer characterRenderer = gameObjects[i].GetComponent<CharacterRenderer>();
            _unsortedCharacterRenderers.Add(characterRenderer);
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
