using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowAttributionText : MonoBehaviour
{
    //creates textvalue in inspector
    public string textValue;
    //holds a UI text element in inspector
    public Text textElement;
    // Start is called before the first frame update
    void Start()
    {
        //sets text field in inspector
        textElement.text = textValue; 
    }

    // Update is called once per frame
    void Update()
    {
        //value can be changed while game is running - not necessary
        //textElement.text = textValue;
    }
}
