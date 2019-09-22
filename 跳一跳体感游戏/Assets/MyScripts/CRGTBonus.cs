using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bonus : MonoBehaviour {

    public Transform scoreEffect;
    public GameObject bonusParticles;
    public int scoreValue = 1;
    public AudioClip soundBonus;

    void OnTriggerEnter2D(Collider2D other)
    {
       if (other.tag == "Player") {
            GameManager.instance.UpdateScore(scoreValue);
            if (soundBonus)
                SoundManager.instance.PlaySound(soundBonus);
            Vector3 particleBPos = new Vector3 (this.transform.position.x, this.transform.position.y, 0.0f);  
            GameObject bonusParticle = Instantiate (bonusParticles, particleBPos, Quaternion.identity) as GameObject;  
            Destroy(bonusParticle, 1.0f);
            if (scoreEffect)
            {
                Transform newScoreTextEffect = Instantiate(scoreEffect, transform.position, Quaternion.identity) as Transform;
                newScoreTextEffect.Find("Text").GetComponent<Text>().text = "+" + scoreValue.ToString();
            }
            Destroy(gameObject);
       }  
    }
}
