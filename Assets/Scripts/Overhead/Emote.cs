using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emote : MonoBehaviour
{
    public enum Emoticon { none, exclamation, heartbreak, skull }
    public Dictionary<Emoticon, Sprite> emoteDict = new Dictionary<Emoticon, Sprite>();
    public Image displayedEmote;
    public Sprite exclamationSprite;
    public Sprite heartbreakSprite;
    public Sprite skullSprite;

    public RectTransform rect;
    private Vector3 initScale;
    public float scalar = 1f;
    private float _scalar = 1f;

    Coroutine emoteBob;
    Coroutine emoteOff;

    void Update()
    {
        if (displayedEmote.enabled == true)
        {
            rect.localScale = rect.localScale * (1 + Time.deltaTime * _scalar);
        }
    }

    public void SetEmoteDict()
    {
        emoteDict.Add(Emoticon.exclamation, exclamationSprite);
        emoteDict.Add(Emoticon.heartbreak, heartbreakSprite);
        emoteDict.Add(Emoticon.skull, skullSprite);
        initScale = rect.localScale;
    }

    public void SetEmote(Emoticon emoticon, float duration)
    {
        if (emoticon == Emoticon.none)
        {
            displayedEmote.enabled = false;
            return;
        }

        if (displayedEmote.enabled == true)
        {
            return;
        }

        displayedEmote.sprite = emoteDict[emoticon];
        displayedEmote.enabled = true;

        rect.localScale = initScale;
        _scalar = scalar;
        emoteBob = StartCoroutine(IEEmoteBob(0.2f));

        emoteOff = StartCoroutine(IEEmoticonOff(duration));

    }

    public void OverrideEmote(Emoticon emoticon, float duration)
    {
        if (emoticon == Emoticon.none)
        {
            displayedEmote.enabled = false;
            return;
        }
        if (displayedEmote.sprite == emoteDict[emoticon])
        {
            return;
        }

        displayedEmote.sprite = emoteDict[emoticon];
        displayedEmote.enabled = true;

        rect.localScale = initScale;
        _scalar = scalar;

        StopCoroutine(emoteBob);
        StopCoroutine(emoteOff);
    }

    private IEnumerator IEEmoteBob(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Bob in the opposite direction
        if (displayedEmote.enabled == true) 
        {
            _scalar = -_scalar;
            emoteBob = StartCoroutine(IEEmoteBob(0.2f)); 
        }

        yield return null;
    }

    private IEnumerator IEEmoticonOff(float delay)
    {
        yield return new WaitForSeconds(delay);

        displayedEmote.enabled = false;

        yield return null;
    }

}
