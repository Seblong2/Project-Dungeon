using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knightskin : MonoBehaviour
{
    public Material skin;
    public Material iskin;

    public void Fade(bool transparent)
    {
        if (transparent)
        {
            gameObject.GetComponent<SkinnedMeshRenderer>().material = iskin;
        }
        else
        {
            gameObject.GetComponent<SkinnedMeshRenderer>().material = skin;
        }
    }
}
