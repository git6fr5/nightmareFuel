using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDArrow : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {HUDPlusArrow}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public Image arrow;
    public Collectible.Type arrowType;
    public RectTransform rect;
    public RectTransform canvasRect;

    /* --- Internal Variables --- */
    private float scaler = 0.999f;
    private float scaleDiff = -0.002f;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        StartCoroutine(IEPlusArrowBob(0.5f));
    }

    void Update()
    {
        Point();
        rect.localScale = rect.localScale * scaler;
    }

    /*--- Methods ---*/
    void Point()
    {
        Collectible collectible = null;
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");

        arrow.enabled = false;

        for (int i = 0; i < collectibles.Length; i++)
        {
            if (collectibles[i].GetComponent<Collectible>().type == arrowType)
            {
                arrow.enabled = true;
                collectible = collectibles[i].GetComponent<Collectible>();
            }
        }
        if (collectible != null)
        {

            Vector3 viewPos = Camera.main.WorldToViewportPoint(collectible.transform.position);
            if ((viewPos.x > 0 && viewPos.x < 1) && (viewPos.y > 0 && viewPos.y < 1))
            {
                arrow.enabled = false;
            }
            else
            {
                viewPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f, viewPos.z); // transform to a more easy to work with space
                // normalize to the greater value -
                if (Mathf.Abs(viewPos.x) > Mathf.Abs(viewPos.y))
                {
                    viewPos = viewPos / (Mathf.Abs(viewPos.x) * 2);
                }
                else
                {
                    viewPos = viewPos / (Mathf.Abs(viewPos.y) * 2);
                }
                viewPos = new Vector3(viewPos.x + 0.5f, viewPos.y + 0.5f, viewPos.z);
                rect.anchoredPosition = new Vector2(((viewPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)), ((viewPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

                float rot_z = Mathf.Atan2(viewPos.y-0.5f, viewPos.x-0.5f) * Mathf.Rad2Deg;
                rect.rotation = Quaternion.Euler(0f, 0f, rot_z -90);
            }
        }
    }

    private IEnumerator IEPlusArrowBob(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Bob in the opposite direction
        scaleDiff = -scaleDiff;
        scaler = scaler + scaleDiff;
        StartCoroutine(IEPlusArrowBob(0.5f));

        yield return null;
    }
}
