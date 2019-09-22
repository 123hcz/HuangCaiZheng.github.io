using UnityEngine;
using System.Collections;

public class CRGTRemoveAfterTime : MonoBehaviour {

	public float removeAfterTime = 0f;

	void Start () {
		Destroy(gameObject, removeAfterTime);
	}
	

}
