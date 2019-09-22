using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changImage : MonoBehaviour {
    public GameObject down;
    public GameObject mido;
    public GameObject up;
    //public CRGTPlayerController getpos;
    public Carcontrol getpos;
    Vector3 diectx = new Vector3(0.2f, 0, 0);
    Vector3 diectx2 = new Vector3(-0.2f, 0, 0);
    // Use this for initialization
    private int count = 0;
    private bool lift = false;

    // Update is called once per frame

    void FixedUpdate() {
        count++;
        if (count < 150 ) {
            this.transform.Translate(diectx);         
        }
        if (count < 300&& count >= 150)
        {
            this.transform.Translate(diectx2);
        }
        if (count >= 300) {
            count = 0;
           
        }

        GameObject PosUp = this.transform.Find("摘苹果姿态4").gameObject;
        GameObject PosMido = this.transform.Find("摘苹果姿态2").gameObject;
        GameObject PosDown = this.transform.Find("摘苹果姿态1").gameObject;
        //this.transform.Translate(diectx);
      

            if (getpos.posup == 1)
            {
                //print(PosUp.transform.position);

                if (PosUp != null && PosMido != null && PosDown != null)
                {
                    print("posup");
                    PosUp.SetActive(true);
                    PosMido.SetActive(false);
                    PosDown.SetActive(false);
                    
                }
            }
            if (getpos.posmido == 1)
            {
                //print("posmido");
                // print(PosUp.transform.position);
                if (PosUp != null && PosMido != null && PosDown != null)
                {
                    print("posmido");
                    PosUp.SetActive(false);
                    PosMido.SetActive(true);
                    PosDown.SetActive(false);
                   
                }

            }
            if (getpos.posdown == 1)
            {
                //
                //print(PosUp.transform.position);
                if (PosUp != null && PosMido != null && PosDown != null)
                {
                    print("posdown");
                    PosUp.SetActive(false);
                    PosMido.SetActive(false);
                    PosDown.SetActive(true);
                    
                }

            }

            
        }




       

}
