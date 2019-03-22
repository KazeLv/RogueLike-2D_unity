using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    private Vector2 targetPos;
    private Transform playerTransform;
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private Animator animator;

    public float smoothing=8;
    public int power = 20;
    public AudioClip attackAudio;

	// Use this for initialization
	void Start () {
        targetPos = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        GameManager.Instance.enemyList.Add(this);
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPos, smoothing*Time.deltaTime));
	}

    public void Move()
    {
        Vector2 offset = playerTransform.position - transform.position;
        if (offset.magnitude < 1.1f)
        {
            //Attack
            animator.SetTrigger("Attack");
            AudioManager.Instance.RandomPlay(0.5f,attackAudio);
            playerTransform.SendMessage("GetHit", power);
        }else
        {
            float x = 0, y = 0;
            if (Mathf.Abs(offset.y) >= Mathf.Abs(offset.x))
            {
                if (offset.y > 0)
                {
                    y = 1;
                }else {
                    y = -1;
                }
            }else
            {
                if (offset.x > 0)
                {
                    x = 1;
                }else
                {
                    x = -1;
                }
            }
            collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPos, targetPos + new Vector2(x, y));
            collider.enabled = true;
            if (hit.transform == null || hit.collider.tag == "Soda" || hit.collider.tag == "Apple"||hit.collider.tag=="enemy")
            {
                targetPos += new Vector2(x, y);
            }
        }
    }
}
