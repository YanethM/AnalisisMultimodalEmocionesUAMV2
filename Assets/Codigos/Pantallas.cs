using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pantallas : MonoBehaviour {

    void Start()
    {
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();        
    }
}
