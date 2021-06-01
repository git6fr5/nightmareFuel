using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCircle : MonoBehaviour
{
    private List<CharacterState> characters = new List<CharacterState>();
    private float poisonDamage = 0.1f;
    private float tickDuration = 0.2f;
    private float tickInterval = 1f;

    void Start()
    {
        StartCoroutine(IEPoisonTicker(tickInterval));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<CharacterState>() && characters.Contains(collider.GetComponent<CharacterState>()))
        {
            characters.Remove(collider.GetComponent<CharacterState>());
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<CharacterState>())
        {
            characters.Add(collider.GetComponent<CharacterState>());
        }
    }

    private IEnumerator IEPoisonTicker(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].Damage(tickDuration, poisonDamage);
        }
        StartCoroutine(IEPoisonTicker(tickInterval));

        yield return null;
    }
}
