using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDTimer : MonoBehaviour
{
    /*--- Components ---*/
    public Text scoreText;
    public Text counterText;
    public RectTransform counterRect;

    /* --- Internal Variables --- */
    private float elapsedTime = 0f;
    private Vector3 floatDirection = new Vector3(0, 1f, 0);
    private float floatSpeed = 100f;
    private Vector3 counterInitPosition;
    private Vector3 counterInitScale;
    private int score;
    private float scoreInterval = 5f;
    private float scalar = -0.5f;

    /*--- Unity Methods ---*/
    void Start()
    {
        counterInitPosition = counterRect.localPosition;
        counterInitScale = counterRect.localScale;
    }

    void Update()
    {
        if (counterText.enabled == true)
        {
            counterRect.localPosition = counterRect.localPosition + floatDirection * floatSpeed * Time.deltaTime;
            counterRect.localScale = counterRect.localScale * (1 + Time.deltaTime * scalar);
        }
    }

    void FixedUpdate()
    {
        elapsedTime = elapsedTime + Time.fixedDeltaTime;
        if (elapsedTime >= scoreInterval)
        {
            AddPoints(1);
            elapsedTime = elapsedTime - scoreInterval;
        }
    }

    public void AddPoints(int points)
    {
        score = score + points;
        Count(points);
        if (score < 10)
        {
            scoreText.text = "00" + score.ToString();
        }
        else if (score < 100)
        {
            scoreText.text = "0" + score.ToString();
        }
        else if (score < 1000)
        {
            scoreText.text = score.ToString();
        }
        else if (score >= 1000)
        {
            scoreText.text = "999";
        }
    }

    void Count(int count)
    {
        counterText.text = count.ToString();
        counterText.enabled = true;
        counterRect.localPosition = counterInitPosition;
        counterRect.localScale = counterInitScale;

        StartCoroutine(IECounterOff(0.5f));

    }

    private IEnumerator IECounterOff(float delay)
    {
        yield return new WaitForSeconds(delay);

        counterText.enabled = false;

        yield return null;
    }

}
