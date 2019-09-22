using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flage2 : MonoBehaviour {
    public flage flage;
    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void Awake()
    {
        flage.agane = false;
        print(flage.agane);
    }
}
