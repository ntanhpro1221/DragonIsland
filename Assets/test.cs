using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.touches.Length > 0)
            if (Input.touches[0].phase == TouchPhase.Moved)
                Camera.main.transform.Translate(Vector3.one);
    }
}
