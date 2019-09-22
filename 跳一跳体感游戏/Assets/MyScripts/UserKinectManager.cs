using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserKinectManager : MonoBehaviour {
    public RawImage kinectImg;
    //public Canvas canvas;
   // public Image leftHand;
    //public Camera camera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       bool isInit = KinectManager.Instance.IsInitialized();
        if (isInit) {
           // Debug.Log("stream");
            if (kinectImg.texture == null)
            {
                Texture kinectPic = KinectManager.Instance.GetUsersLblTex();
                //Debug.Log("stream");
                //Texture kinectPic = KinectManager.Instance.GetUsersClrTex();
                kinectImg.texture = kinectPic;
             }
            if (KinectManager.Instance.IsUserDetected())
            {
                long UserId = KinectManager.Instance.GetPrimaryUserID();
                int jointTypel = (int)KinectInterop.JointType.HandLeft;
               // int jointTyper = (int)KinectInterop.JointType.HandRight;
                if (KinectManager.Instance.IsJointTracked(UserId,jointTypel))
                {
                    //print("left");
                    Vector3 leftHandPos = KinectManager.Instance.GetJointKinectPosition(UserId, jointTypel);
                    //print(leftHandPos);
                    //Vector3 leftHandPos = KinectManager.Instance.GetJointPosition(UserId,jointTypel);
                    // print("x:" + leftHandPos.x + ",,,,y:" + leftHandPos.y + ",,,,,z : " + leftHandPos.z);
                   // Vector3 leftHandscreenPostion = Camera.main.WorldToScreenPoint(leftHandPos);
                   // print("xxxxxxxx");
                   // Vector2 leftHandSenPos = new Vector2(leftHandscreenPostion.x, leftHandscreenPostion.y);
                   // Vector2 leftHandUguiPos;


                   // if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, leftHandSenPos, null, out leftHandUguiPos))
                   // {
                      //  print("left");
                       // RectTransform liftRectTf = leftHand.transform as RectTransform;
                       // liftRectTf.anchoredPosition = leftHandUguiPos;
                      //  print("left" + leftHandUguiPos.x+"y"+leftHandUguiPos.y);
                   // }




                    KinectInterop.HandState rightHandStay = KinectManager.Instance.GetRightHandState(UserId);



                    if (rightHandStay == KinectInterop.HandState.Closed) {
                        print("右手closed");
                        Debug.Log("leftclosed");

                    } else if (rightHandStay == KinectInterop.HandState.Open) {
                        print("rightHandOpened");
                        
                    } 
                }
               
            }
           

            

        }
	}
}
