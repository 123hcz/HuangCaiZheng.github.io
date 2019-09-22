using UnityEngine;
using System.Collections;

public class CRGTObjectDestroyer : MonoBehaviour {

	public bool removeTarget = false;
	public float removeTargetTime = 0.0f;
	public bool removeSelf = true;
	public float removeSelfTime = 0.0f; 


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Car") {	
			if (removeTarget == true) 
				Destroy(other.gameObject, removeTargetTime);

			if (removeSelf == true)
				Destroy (gameObject, removeSelfTime);
		}
	}	
}
