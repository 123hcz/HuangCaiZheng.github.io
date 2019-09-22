using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getTouch : MonoBehaviour {

    public teamp gameScore;

   // public int gameScoreTeamp = 0;
    // Use this for initialization
    void Start () {
        if (gameScore.teampScros!= 0) {
            gameScore.teampScros = 0;
        }
       
       // print("11111111111111111111111111111111111111111");
    }
	
	// Update is called once per frame
	void Update () {
        //print(gameScoreTeamp);
        //if (agane == true) {
            //this.gameObject.SetActive(true);
       // }
        
    }

    private void OnTriggerStay2D(Collider2D Player)
    {
        
        if (Player.tag == "sec2Plater") {
            gameScore.teampScros++;
           // Debug.Log("Ontrigger");
            this.gameObject.SetActive(false);
            
        }
    }
    private void OnDestroy()
    {
        
    }


}
