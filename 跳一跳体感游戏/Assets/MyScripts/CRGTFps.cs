using UnityEngine;
using System.Collections;


[RequireComponent (typeof(GUIText))]
public class CRGTFps : MonoBehaviour {

	public float updateInterval = 0.5f;
	private float accum;
	private int frames;
	private float timeLeft;

	private GUIText FpsText;

	void Start () {
		timeLeft = updateInterval;
		FpsText = GetComponent<GUIText> ();
	}
	
	void Update () {
		timeLeft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames++;

		if ((double)timeLeft <= 0.0) {
			float numFps = accum / (float)frames;
			string text = string.Format("{0:F2} FPS", numFps);
			FpsText.text = text;
			if (numFps < 30.0f) {
				FpsText.color = Color.yellow;
			} else if (numFps < 10.0f) {
				FpsText.color = Color.red;	
			} else {
				FpsText.color = Color.green;
			}

			timeLeft = updateInterval;
			accum = 0.0f;
			frames = 0;
 		}
	}
}
