using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButton : MonoBehaviour
{
    public GameObject Credits;

    public void OpenCredits()
    {
        if(Credits != null)
        {
            Credits.SetActive(true);
        }
    }
}
