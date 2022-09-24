using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsExit : MonoBehaviour
{
    public GameObject Credits;

    public void CloseCredits()
    {
        if (Credits != null)
        {
            Credits.SetActive(false);
        }
    }
}
