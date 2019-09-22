using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sharkTransferm : MonoBehaviour {
    public float objectSpeed = -3.0f;
    public float objectMoveX = 0;
    public float objectMoveY = 0.1f;
    public float objectMoveZ = 0;
    // Update is called once per frame
    void Update () {
        //transform.Translate(new Vector3(objectMoveX, objectMoveY, objectMoveZ) * objectSpeed * CRGTGameManager.instance.gameSpeed * Time.deltaTime);
        this.transform.Translate(new Vector3(objectMoveX, objectMoveY, objectMoveZ));
    }
}
