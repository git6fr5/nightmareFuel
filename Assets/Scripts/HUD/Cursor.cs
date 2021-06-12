using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public SpriteRenderer cursorRenderer;
    //public Sprite idleSprite;
    //public Sprite lockedSprite;
    public Vector2 maxScale;
    public Vector2 minScale;
    public float fluxRate;
    private int fluxFlip = 1;

    // Update is called once per frame
    void Update()
    {
        //print(Cursor.hotspot);
        Position();
        Flux(); 
    }

    void Position()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector2 worldPos = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = (Vector3)worldPos;
    }

    void Flux()
    {
        Vector3 gradient = (fluxFlip) * (maxScale - minScale);
        transform.localScale = transform.localScale - gradient * fluxRate * Time.deltaTime;
        if (transform.localScale.x < minScale.x)
        {
            transform.localScale = minScale * 1.01f;
            fluxFlip = -fluxFlip;
        }
        else if (transform.localScale.x > maxScale.x)
        {
            transform.localScale = maxScale * 0.99f;
            fluxFlip = -fluxFlip;
        }
    }
}
