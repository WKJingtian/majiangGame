using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class majiang : MonoBehaviour
{
    public SpriteRenderer sp1;
    public SpriteRenderer sp2;
    public float jumpCD;
    public float hitJumpHeight;
    public float jumpHeight;
    public float jumpLeapSpeed;
    public int health;
    public int typeID;

    float jumpCDcounter;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        jumpCDcounter = jumpCD;
        setImg(-1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health < 0) Destroy(this.gameObject);
        if (this.transform.position.y < -100)
        {
            Debug.LogError("majiang paole");
            Destroy(this.gameObject);
        }

        jumpCDcounter -= Time.deltaTime;
        if (jumpCDcounter <= 0)
        {
            this.transform.Rotate(0, Random.Range(-90.0f, 90.0f), 0);
            jumpCDcounter = jumpCD;
            rb.AddForce(this.transform.rotation * new Vector3(jumpLeapSpeed, 0, 0)
                + new Vector3(0, jumpHeight, 0),
                ForceMode.Impulse);
        }
    }

    public void setImg(int type)
    {
        typeID = type;
        // load image according to type
    }

    public void getHit(int dmg)
    {
        rb.AddForce(new Vector3(0, hitJumpHeight, 0), ForceMode.Impulse);
        health -= dmg;
    }

    void OnCollisionEnter(Collider other)
    {
        if (other.gameObject.tag == "qiangbi")
        {
            Debug.LogWarning("zhuangqiang");
            rb.AddForce(new Vector3(rb.velocity.x * -2, 0, rb.velocity.z * -2), ForceMode.Impulse);
        }
        if (other.gameObject.tag == "player")
        {
            Debug.LogWarning("zhuangren");
            rb.AddForce(10*(other.transform.position - this.transform.position), ForceMode.Impulse);
        }
    }
}
