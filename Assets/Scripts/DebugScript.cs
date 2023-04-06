using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
   public Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
      Debug.Log(rb.velocity); 
    }
}
