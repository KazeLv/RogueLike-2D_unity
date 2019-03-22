using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private BoxCollider2D collider;
    private Rigidbody2D rigidbody;
    private float restTime;
    private Animator animator;

    public float resetRest=0.5f;
    public float smoothing=1;
    public Vector2 targetPosition = new Vector2(1, 1);
    public AudioClip chop1Audio;
    public AudioClip chop2Audio;
    public AudioClip step1Audio;
    public AudioClip step2Audio;
    public AudioClip fruit1Audio;
    public AudioClip fruit2Audio;
    public AudioClip soda1Audio;
    public AudioClip soda2Audio;

    // Use this for initialization
    void Start () {
        rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        restTime = resetRest;
	}
	
	// Update is called once per frame
	void Update ()
    {
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime));
        restTime -= Time.deltaTime;
        if (restTime > 0) return;

        if (GameManager.Instance.energy <= 0) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h > 0) v = 0;
        if (h != 0 || v != 0)
        {
            GameManager.Instance.LoseEnergy(1);
            collider.enabled = false;
            RaycastHit2D hit= Physics2D.Linecast(targetPosition, targetPosition + new Vector2(h, v));
            collider.enabled = true;
            if (hit.transform == null) {
                targetPosition += new Vector2(h, v);
                AudioManager.Instance.RandomPlay(0.2f,step1Audio, step2Audio);
            }
            else
            {
                switch (hit.collider.tag)
                {
                    case "OutWalls":
                        break;
                    case "Walls":
                        animator.SetTrigger("PlayerAttack");
                        AudioManager.Instance.RandomPlay(chop1Audio,chop2Audio);
                        hit.collider.SendMessage("GetAttacked");
                        break;
                    case "Apple":
                        GameManager.Instance.AddEnergy(20);
                        Destroy(hit.transform.gameObject);
                        AudioManager.Instance.RandomPlay(fruit1Audio, fruit2Audio);
                        targetPosition += new Vector2(h, v);
                        break;
                    case "Soda":
                        GameManager.Instance.AddEnergy(10);
                        Destroy(hit.transform.gameObject);
                        AudioManager.Instance.RandomPlay(0.4f,soda1Audio, soda2Audio);
                        targetPosition += new Vector2(h, v);
                        break;
                    case "Enemy":
                        targetPosition += new Vector2(h, v);
                        break;
                }
            }
            GameManager.Instance.OnPlayerMove();
            restTime = resetRest;
        }
	}

    public void GetHit(int power)
    {
        GameManager.Instance.LoseEnergy(power);
        animator.SetTrigger("PlayerHit");
    }
}
