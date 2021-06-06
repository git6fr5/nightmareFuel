using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour
{
    /* --- Components --- */

    // Signalled Object
    public Transform signalObject;

    // Signal Ring
    public Transform signalRing;
    public Vector3 maxSignalRingScale;
    public Vector3 minSignalRingScale;

    /* --- Internal Variables --- */
    private Vector3 location = Vector3.zero;
    private float fluxRate;

    void Update()
    {
        transform.position = location;
        SignalSize();
    }

    void FixedUpdate()
    {
        SignalRingFlux();
    }

    public void Activate(Vector3 _location, float _fluxRate)
    {
        fluxRate = _fluxRate;
        location = _location;
        transform.position = location;
        signalRing.localScale = maxSignalRingScale;
        SignalSize();
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {           
        gameObject.SetActive(false);
    }

    void SignalSize()
    {
        float dist = Vector2.Distance(transform.position, signalObject.position);
        float scale = (dist + 1) * Mathf.Pow(2, -dist);
        transform.localScale = new Vector3(scale, scale, 1);
    }

    void SignalRingFlux()
    {
        Vector3 gradient = maxSignalRingScale - minSignalRingScale;
        signalRing.localScale = signalRing.localScale - gradient * fluxRate * Time.fixedDeltaTime;
        if (signalRing.localScale.x < minSignalRingScale.x)
        {
            signalRing.localScale = maxSignalRingScale;
        }
    }
}
