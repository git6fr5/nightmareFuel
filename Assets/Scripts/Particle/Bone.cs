using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Bone}: ";
    private bool DEBUG_init = false;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

    /*--- Methods ---*/
    public void Attach(Bone _bone)
    {
        /*
        // Get the top-most parent of the bone
        List<Transform> boneChain = new List<Transform>();
        boneChain.Add(_bone.transform);
        while (boneChain[boneChain.Count-1].parent != null)
        {
            boneChain.Add(boneChain[boneChain.Count - 1].parent);
        }

        // Attach the top-most parent to the bone
        boneChain[boneChain.Count - 1].SetParent(transform);

        // Adjust the local position of the top-most parent to align the bones
        for (int i = boneChain.Count - 2; i >= 0; i--)
        {
            boneChain[boneChain.Count - 1].localPosition = -boneChain[i].localPosition;
        }*/

        // Get the top-most parent of the bone
        List<Transform> boneChain = new List<Transform>();
        boneChain.Add(_bone.transform);
        while (!boneChain[boneChain.Count - 1].GetComponent<Skeleton>())
        {
            boneChain.Add(boneChain[boneChain.Count - 1].parent);
        }
        boneChain.Add(boneChain[boneChain.Count - 1].parent);
        // Attach the top-most parent to the bone
        boneChain[boneChain.Count - 1].SetParent(transform);

        // Adjust the local position of the top-most parent to align the bones
        for (int i = boneChain.Count - 2; i >= 0; i--)
        {
            boneChain[boneChain.Count - 1].localPosition = -boneChain[i].localPosition;
        }
        
    }
}
