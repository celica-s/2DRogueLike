using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
    public int damage;
    public AudioClip enemyClip;
    private Transform player;
    private Animator animator;
    private bool skipMove;

    protected override void Start () {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        animator = GetComponent<Animator> ();
        GameManager.Instance.enemys.Add (this);
        base.Start ();
    }

    protected override void AttemptMove<T> (int xDir, int yDir) {
        if (skipMove) {
            skipMove = false;
            return;
        }
        base.AttemptMove<T> (xDir, yDir);
        skipMove = true;
    }

    public void Move () {
        int x = 0;
        int y = 0;
        Vector3 offset = player.position - transform.position;
        if (Mathf.Abs (offset.y) > Mathf.Abs (offset.x)) {
            y = offset.y < 0 ? -1 : 1;
        } else {
            x = offset.x < 0 ? -1 : 1;
        }

        AttemptMove<Player> (x, y);
    }

    protected override void OnCantMove<T> (T component) {
        Player hitPlayer = component as Player;
        animator.SetTrigger ("Attack");
        hitPlayer.TakeDamage (damage);
        animator.SetTrigger ("Attack");
        AudioManager.Instance.RandomPlay (enemyClip);
    }
}