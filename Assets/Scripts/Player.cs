using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject {
    public AudioClip moveClip1;
    public AudioClip moveClip2;
    public AudioClip foodClip1;
    public AudioClip foodClip2;
    public AudioClip sodaClip1;
    public AudioClip sodaClip2;
    private Animator animator;
    protected override void Start () {
        animator = GetComponent<Animator> ();
        base.Start ();
    }

    void Update () {

        if (!GameManager.Instance.playersTurn)
            return;

        int h = (int) Input.GetAxisRaw ("Horizontal");
        int v = (int) Input.GetAxisRaw ("Vertical");
        if (h > 0) v = 0;
        if (h != 0 || v != 0) {
            GameManager.Instance.ReduceFood (1);
            AttemptMove<Wall> (h, v);
        }
    }
    protected override void AttemptMove<T> (int xDir, int yDir) {
        base.AttemptMove<T> (xDir, yDir);

        RaycastHit2D hit;
        if (Move (xDir, yDir, out hit)) {
            AudioManager.Instance.RandomPlay (moveClip1, moveClip2);
        }

        GameManager.Instance.playersTurn = false;
    }

    void OnTriggerEnter2D (Collider2D other) {
        switch (other.tag) {
            case "Food":
                GameManager.Instance.AddFood (10);
                other.gameObject.SetActive (false);
                AudioManager.Instance.RandomPlay (foodClip1, foodClip2);
                break;
            case "Soda":
                GameManager.Instance.AddFood (20);
                other.gameObject.SetActive (false);
                AudioManager.Instance.RandomPlay (sodaClip1, sodaClip2);
                break;
            case "Exit":
                GameManager.Instance.Restart ();
                break;
            default:
                break;
        }
    }

    public void TakeDamage (int damage) {
        animator.SetTrigger ("Damage");
        GameManager.Instance.ReduceFood (damage);
    }

    protected override void OnCantMove<T> (T component) {
        Wall hitWall = component as Wall;
        hitWall.TakeDamage ();
        animator.SetTrigger ("Attack");
    }
}