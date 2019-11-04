using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
    public AudioClip attackClip1;
    public AudioClip attackClip2;
    public int hp = 2;
    public Sprite damageSprite;
    public void TakeDamage () {
        hp--;
        AudioManager.Instance.RandomPlay (attackClip1, attackClip2);
        GetComponent<SpriteRenderer> ().sprite = damageSprite;
        if (hp <= 0) {
            Destroy (gameObject);
        }
    }
}