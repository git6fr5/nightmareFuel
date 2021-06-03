using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emote : MonoBehaviour
{
    public enum Emoticon { none, exclamation, heartbreak }
    public Dictionary<Emoticon, Sprite> emoteDict = new Dictionary<Emoticon, Sprite>();
    public Image displayedEmote;
    public Sprite exclamationSprite;
    public Sprite heartbreakSprite;
    public RectTransform rect;
    private Vector3 initScale;
    public float scalar = 1f;
    private float _scalar = 1f;

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

        print("Setting emote");

        displayedEmote.sprite = emoteDict[emoticon];
        displayedEmote.enabled = true;

        rect.localScale = initScale;
        _scalar = scalar;
        StartCoroutine(IEEmoteBob(0.2f));

        StartCoroutine(IEEmoticonOff(duration));

    }

    private IEnumerator IEEmoteBob(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Bob in the opposite direction
        _scalar = -_scalar;
        if (displayedEmote.enabled == true) { StartCoroutine(IEEmoteBob(0.2f)); }

        yield return null;
    }

    private IEnumerator IEEmoticonOff(float delay)
    {
        yield return new WaitForSeconds(delay);

        displayedEmote.enabled = false;

        yield return null;
    }
}
