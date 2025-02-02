using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // timer to distroy bubbles after a set amount of time
        Destroy(gameObject, 10f);
    }


}
