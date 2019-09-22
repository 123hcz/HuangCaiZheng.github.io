using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flage : MonoBehaviour {
    public bool agane;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        agane = true;
        print(agane);
    }
}
