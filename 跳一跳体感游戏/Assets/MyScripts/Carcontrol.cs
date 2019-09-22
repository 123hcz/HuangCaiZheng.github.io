using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Carcontrol : MonoBehaviour
{
    public float control = 0;
    public int posup = 0;
    public int posmido = 0;
    public int posdown = 0;
    public bool touch = false;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool isInit = KinectManager.Instance.IsInitialized();
     
           if (!isInit)
          {

           // if (image.texture == null)
             //  {
               // Texture kinectPic = KinectManager.Instance.GetUsersLblTex();
                //Debug.Log("stream");
                //Texture kinectPic = KinectManager.Instance.GetUsersClrTex();
               //       image.texture = kinectPic;
            }
            if (KinectManager.Instance.IsUserDetected())
            {
                    long UserId = KinectManager.Instance.GetPrimaryUserID();
                    int jointTypel = (int)KinectInterop.JointType.HandLeft;
                    int jointTyper = (int)KinectInterop.JointType.HandRight;
                    if (KinectManager.Instance.IsJointTracked(UserId, jointTypel))
                {

                    KinectInterop.HandState rightHandStay = KinectManager.Instance.GetRightHandState(UserId);
                    if (rightHandStay == KinectInterop.HandState.Closed)
                    {
                       //print("右手closed");
                       //Debug.Log("leftclosed");

                    }
                     else if (rightHandStay == KinectInterop.HandState.Open)
                    {
                     // print("rightHandOpened");

                    }
                        Vector3 leftHandPos = KinectManager.Instance.GetJointKinectPosition(UserId, jointTypel);
                        Vector3 rightHandPos = KinectManager.Instance.GetJointKinectPosition(UserId, jointTyper);
                        float absx = -(leftHandPos.x - rightHandPos.x);
                        float absy = (rightHandPos.y - leftHandPos.y);
                        float rightoldY = rightHandPos.y;
                        float leftoldY = leftHandPos.y;
                // Debug.Log("absx:"+absx);
                //Debug.Log("absy:" + absy);

                if (absx > 0.25f && absy > 0.1f) {
                    control = 0.5f;
                    //Debug.Log("右");
                }
                         if (absx > 0.25f && absy < -0.1f) {
                    control = -0.5f;
                    //Debug.Log("左");
                }
                         if ((absy > -0.1f && absy < 0.1f)||absx< 0.25f)
                {
                    //Debug.Log("中");
                    control = 0 ;
                }
                //print(leftHandPos);
                //Vector3 leftHandPos = KinectManager.Instance.GetJointPosition(UserId,jointTypel);
                //print("lx:" + leftHandPos.x + "ly:" + leftHandPos.y + "lz : " + leftHandPos.z);
                //print("rx:" + leftHandPos.x + "ry:" + leftHandPos.y + "rz : " + leftHandPos.z);
                if (absy < 0) { absy = -absy; }

                if (absx > 0.25f && absy > 0.2f)
                {
                    posup = 1;
                    posmido = 0;
                    posdown = 0;
                    // Debug.Log("Posup");
                }
                if (absx > 0.25f && (absy > 0.1f && absy < 0.2f))
                {
                    posup = 0;
                    posmido = 1;
                    posdown = 0;
                    // Debug.Log("Pos2");
                }
                if ((absy > 0.0f && absy < 0.1f) && absx > 0.25f)
                {
                    //Debug.Log("Pos3");
                    posup = 0;
                    posmido = 0;
                    posdown = 1;
                }
                /*
                if ((absy > 0.0f && absy < 0.05f) && absx > 0.25f) {
                       float i = rightoldY - rightHandPos.y;
                       float j = leftoldY - leftHandPos.y;
                       float z = i - j;
                       if (z < 0) { z = -z; }
                    if (i > 0.01 && j > 0.01 & z < 0.05) {
                        touch = true;
                    }
                    }
                 */
           
                
                    }

                }

            }
        }
 