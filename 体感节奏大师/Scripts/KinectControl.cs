using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KinectControl : MonoBehaviour
{
    public RawImage KinectImg;
    public bool RightHandOpen;
    public bool LeftHandOpen;
    public bool flagtrak1;
    public bool flagtrak2;
    public bool flagtrak3;
    public bool flagtrak4;
    public bool flagtrak5;
    public bool flagtrak6;
    public Canvas canvas;
    public GameObject RHand;
    public GameObject LHand;
    public RectTransform track1;
    public RectTransform track2;
    public RectTransform track3;
    public RectTransform track4;
    public RectTransform track5;
    public RectTransform track6;
    // Use this for initialization
    void Start()
    {
        RightHandOpen = false;
        LeftHandOpen = false;
        //Vector3 screenPostion = Camera.main.WorldToScreenPoint(Cube.position);//屏幕坐标系
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 mousePosition = Input.mousePosition;//鼠标坐标系
        bool isInit = KinectManager.Instance.IsInitialized();
        //Debug.Log("isInit"+isInit);
        if (isInit)
        {
            if (KinectImg.texture == null)
            {
                Texture kinectPic = KinectManager.Instance.GetUsersLblTex();
                //Texture kinectPic = KinectManager.Instance.GetUsersClrTex();
                KinectImg.texture = kinectPic;
            }
        }

        if (KinectManager.Instance.IsUserDetected())
        {
            long UserId = KinectManager.Instance.GetPrimaryUserID();
            int jointTypel = (int)KinectInterop.JointType.HandLeft;
            int jointTyper = (int)KinectInterop.JointType.HandRight;
            if (KinectManager.Instance.IsJointTracked(UserId, jointTypel))
            {
                KinectInterop.HandState rightHandStay = KinectManager.Instance.GetRightHandState(UserId);
                KinectInterop.HandState LeftHandStay = KinectManager.Instance.GetLeftHandState(UserId);
                if (rightHandStay == KinectInterop.HandState.Closed)
                {
                    //print("右手closed");
                    RightHandOpen = false;
                    //Debug.Log("leftclosed");

                }
                else if (rightHandStay == KinectInterop.HandState.Open)
                {
                    // print("rightHandOpened");
                    RightHandOpen = true;
                }

                if (LeftHandStay == KinectInterop.HandState.Closed)
                {
                   // print("LeftHandclosed");
                    LeftHandOpen = false;
                    //Debug.Log("leftclosed");

                }
                else if (LeftHandStay == KinectInterop.HandState.Open)
                {
                    LeftHandOpen = true;
                    //print("LeftHandOpened");
                }
                Vector3 leftHandPos = KinectManager.Instance.GetJointKinectPosition(UserId, jointTypel);
                Vector3 rightHandPos = KinectManager.Instance.GetJointKinectPosition(UserId, jointTyper);
                Vector3 rightHandScrenPos = Camera.main.WorldToScreenPoint(rightHandPos);
                Vector3 leftHandScrenPos = Camera.main.WorldToScreenPoint(leftHandPos);
                Vector2 rightHandScren2Pos = new Vector2(rightHandScrenPos.x,rightHandScrenPos.y);
                Vector2 LeftHandScren2Pos = new Vector2(leftHandScrenPos.x, leftHandScrenPos.y);
                Vector2 rightHandUguiPos;
                Vector2 LeftHandUguiPos;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform,rightHandScren2Pos,null,out rightHandUguiPos))
                 {

                    rightHandUguiPos.x = (rightHandUguiPos.x*8 -100);
                    rightHandUguiPos.y = (rightHandUguiPos.y - 300) * 2;
                    RectTransform rightRectTf = RHand.transform as RectTransform;
                    rightRectTf.anchoredPosition = rightHandUguiPos;
                    if (rightRectTf.anchoredPosition.x < track4.anchoredPosition.x + 60 && rightRectTf.anchoredPosition.x > track4.anchoredPosition.x - 60
                        && rightRectTf.anchoredPosition.y < track4.anchoredPosition.y + 60 && rightRectTf.anchoredPosition.y > track4.anchoredPosition.y - 60)
                    {
                        flagtrak4 = true;
                        //Debug.Log("111111111111");
                    }
                    else
                    {
                        flagtrak4 = false;

                    }
                    if (rightRectTf.anchoredPosition.x < track5.anchoredPosition.x + 60 && rightRectTf.anchoredPosition.x > track5.anchoredPosition.x - 60
                         && rightRectTf.anchoredPosition.y < track5.anchoredPosition.y + 60 && rightRectTf.anchoredPosition.y > track5.anchoredPosition.y - 60)
                    {
                        flagtrak5 = true;
                        //Debug.Log("222222222222255555");
                    }
                    else
                    {
                        flagtrak5 = false;

                    }
                    if (rightRectTf.anchoredPosition.x < track6.anchoredPosition.x + 60 && rightRectTf.anchoredPosition.x > track6.anchoredPosition.x - 60
                        && rightRectTf.anchoredPosition.y < track6.anchoredPosition.y + 60 && rightRectTf.anchoredPosition.y > track6.anchoredPosition.y - 60)
                    {
                        flagtrak6 = true;
                        //Debug.Log("3333333333333336666666666666666666");
                    }
                    else
                    {
                        flagtrak6 = false;
                    }
                }
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, LeftHandScren2Pos, null, out LeftHandUguiPos))
                {

                    LeftHandUguiPos.x = (LeftHandUguiPos.x * 8 + 100);
                    LeftHandUguiPos.y = (LeftHandUguiPos.y - 310)*2;
                    RectTransform leftRectTf = LHand.transform as RectTransform;
                    leftRectTf.anchoredPosition = LeftHandUguiPos;
                    if (leftRectTf.anchoredPosition.x < track1.anchoredPosition.x + 60 && leftRectTf.anchoredPosition.x > track1.anchoredPosition.x - 60
                        &&leftRectTf.anchoredPosition.y < track1.anchoredPosition.y + 60 && leftRectTf.anchoredPosition.y > track1.anchoredPosition.y - 60)
                    {
                        flagtrak1 = true;
                        //Debug.Log("11");
                    }
                    else {
                        flagtrak1 = false;
                        
                    }
                    if (leftRectTf.anchoredPosition.x < track2.anchoredPosition.x + 60 && leftRectTf.anchoredPosition.x > track2.anchoredPosition.x - 60
                         && leftRectTf.anchoredPosition.y < track1.anchoredPosition.y + 60 && leftRectTf.anchoredPosition.y > track1.anchoredPosition.y - 60)
                    {
                        flagtrak2 = true;
                        //Debug.Log("2222");
                    }
                    else
                    {
                        flagtrak2 = false;
                      
                    }
                    if (leftRectTf.anchoredPosition.x < track3.anchoredPosition.x + 60 && leftRectTf.anchoredPosition.x > track3.anchoredPosition.x - 60
                        && leftRectTf.anchoredPosition.y < track1.anchoredPosition.y + 60 && leftRectTf.anchoredPosition.y > track1.anchoredPosition.y - 60)
                    {
                        flagtrak3 = true;
                        //Debug.Log("33333333");
                    }
                    else
                    {
                        flagtrak3 = false;
                    }
                    //RHand.transform.position = rightHandPos;
                }
                //print(rightHandScrenPos);


            }

        }

    }
}