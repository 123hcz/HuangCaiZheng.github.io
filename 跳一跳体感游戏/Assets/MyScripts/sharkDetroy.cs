using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sharkDetroy : MonoBehaviour {

    public string TargetTag = "shark";
    public bool removeTarget = false;
    public float removeTargetTime = 0.0f;
    public bool removeSelf = true;
    public float removeSelfTime = 0.0f;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == TargetTag)
        {
            //if (removeTarget == true)
              //  Destroy(other.gameObject, removeTargetTime);

            if (removeSelf == true)
                Destroy(gameObject, removeSelfTime);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == TargetTag)
        {
            if (removeSelf == true)
            {
                Destroy(gameObject, removeSelfTime);
            }

        }
           
             
        
    }
}
