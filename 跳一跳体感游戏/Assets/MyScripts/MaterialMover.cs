using UnityEngine;
using System.Collections;

public class MaterialMover : MonoBehaviour {

	public float materialSpeedX = 0.0f;
	public float materialSpeedY = 1.0f;
	private Vector2 materialOffset;
	private Renderer materialRenderer;

	void Start () {
		materialRenderer = GetComponent<Renderer>();
	}
	
	void Update ()
    {
       materialOffset = new Vector2(materialSpeedX * GameManager.instance.gameSpeed * Time.time, materialSpeedY * GameManager.instance.gameSpeed * Time.time);
       materialRenderer.material.mainTextureOffset = materialOffset;
    }
}
