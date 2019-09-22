using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beckGruond : MonoBehaviour
{
    public Transform spawPos;
    public GameObject sparwGeound;
    private GameObject sparwGeounded;
    public GameObject ground;

    public GameObject sparwGround2;
    private GameObject sparwGeounded2;
    public GameObject ground2;

    public ChangtoScenedScens changflag;

    public GameObject sea1;
    public GameObject sea2;


    //private bool destroy1 = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("trigger11111111111111111111111111111111111111111111111111111");
        if (changflag.scensFlage == false)
        {
            sparwGeounded = GameObject.Instantiate(sparwGeound);
            sparwGeound.gameObject.transform.position = spawPos.position;
        }
        if (changflag.scensFlage == true)
        {
            sea1.SetActive(true);
            sea2.SetActive(true);
            sparwGeounded2 = GameObject.Instantiate(sparwGround2);
            sparwGround2.gameObject.transform.position = spawPos.position;
            print("true");

        }
        Destroy(collision.gameObject);
        //Destroy(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (changflag.scensFlage == false)
        {
            sparwGeounded = GameObject.Instantiate(sparwGeound);
            sparwGeound.gameObject.transform.position = spawPos.position;
        }
        if (changflag.scensFlage == true)
        {
            if (sea1 != null && sea2 != null)
            {
             sea1.SetActive(true);
             sea2.SetActive(true);
            }
           
            sparwGeounded2 = GameObject.Instantiate(sparwGround2);
            sparwGround2.gameObject.transform.position = spawPos.position;
            print("true");

        }
        Destroy(collision.gameObject);
        //Destroy(collision.gameObject);
    }

}
