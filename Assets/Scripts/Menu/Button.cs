using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public enum ButtonState { none, pressed, depressed };

    /*--- Components ---*/
    public bool isLevelActivateButton;
    public bool isNextSelectButton;
    public bool isPrevSelectButton;
    public LevelSelect levelSelector;
    public string activationString = "";
    ButtonState buttonState = ButtonState.none;

    public Color fade;
    public float fadeTime;
    public float fadeIntensity;

    /* --- Internal Variables --- */

    /* --- Unity Methods --- */
    void Start()
    {
        ButtonSetAddColor(new Color(0, 0, 0, 0));
        GetComponent<Image>().material.SetFloat("_OutlineWidth", 0f);
    }

    void OnEnable()
    {

    }

    void Update()
    {
        if (buttonState == ButtonState.pressed)
        {
            ButtonAddColor(fade * fadeIntensity * Time.deltaTime / fadeTime);
        }
        else if (buttonState == ButtonState.depressed)
        {
            ButtonAddColor(-(Vector4)fade * fadeIntensity * Time.deltaTime / fadeTime);
        }
    }

    /* --- Methods --- */
    void OnMouseDown()
    {
        if (isLevelActivateButton)
        {
            LevelActivate();
        }
        if (isNextSelectButton)
        {
            SelectNextLevel(1);
        }

        if (isPrevSelectButton)
        {
            SelectNextLevel(-1);
        }

        buttonState = ButtonState.pressed;
        StartCoroutine(IEButtonDepress(fadeTime));
    }

    private IEnumerator IEButtonDepress(float delay)
    {
        yield return new WaitForSeconds(delay);

        buttonState = ButtonState.depressed;
        StartCoroutine(IEButtonIdle(fadeTime));

        yield return null;
    }

    private IEnumerator IEButtonIdle(float delay)
    {
        yield return new WaitForSeconds(delay);

        buttonState = ButtonState.none;
        ButtonSetAddColor(new Color(0, 0, 0, 0));

        yield return null;
    }

    void LevelActivate()
    {
        SceneManager.LoadScene(levelSelector.selectedLevel.levelName, LoadSceneMode.Single);
    }

    void SelectNextLevel(int level)
    {
        levelSelector.SelectLevel(levelSelector.levelIndex + level);
    }

    void ButtonAddColor(Color color)
    {
        GetComponent<Image>().material.SetColor("_AddColor", GetComponent<Image>().material.GetColor("_AddColor") + color);
    }

    void ButtonSetAddColor(Color color)
    {
        GetComponent<Image>().material.SetColor("_AddColor", color);
    }

    void OnMouseOver()
    {
        print("hello");
        GetComponent<Image>().material.SetFloat("_OutlineWidth", 1f);
        
    }

    void OnMouseExit()
    {
        print("hello");
        GetComponent<Image>().material.SetFloat("_OutlineWidth", 0f);

    }
}
