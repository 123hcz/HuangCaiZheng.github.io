using UnityEngine;
using System.Collections;

public class CRGTObjectMover : MonoBehaviour {

	public float objectSpeed = -3.0f;
    public float objectMoveX = 0;
    public float objectMoveY = 1;
    public float objectMoveZ = 0;
    

    void Update ()
    {
        transform.Translate(new Vector3(objectMoveX, objectMoveY, objectMoveZ) * objectSpeed * CRGTGameManager.instance.gameSpeed * Time.deltaTime);
        
        
    }
}
