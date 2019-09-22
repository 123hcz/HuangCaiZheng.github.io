using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teans : MonoBehaviour {
    Vector3 diect = new Vector3(0, -0.2f, 0);
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
       // this.transform.Translate(diect);
    }

    void FixedUpdate()
    {
        this.transform.Translate(diect);
    }

}
