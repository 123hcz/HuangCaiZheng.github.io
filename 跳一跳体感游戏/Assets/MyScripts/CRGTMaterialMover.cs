using UnityEngine;
using System.Collections;

public class CRGTMaterialMover : MonoBehaviour {

	public float materialSpeedX = 0.0f;
	public float materialSpeedY = 1.0f;
	private Vector2 materialOffset;
	private Renderer materialRenderer;

	void Start () {
		materialRenderer = GetComponent<Renderer>();
	}
	
	void Update ()
    {
       materialOffset = new Vector2(materialSpeedX * CRGTGameManager.instance.gameSpeed * Time.time, materialSpeedY * CRGTGameManager.instance.gameSpeed * Time.time);
       materialRenderer.material.mainTextureOffset = materialOffset;
    }
}
