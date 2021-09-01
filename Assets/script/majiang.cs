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
    public float explodeRad;
    public float explodeDmg;
    public int health;
    public int typeID;
    public generator gameManager;
    public GameObject explosionParticle;
    public GameObject deathParticle;
    public Rigidbody rb;
    public float animMag;
    public float animSpeed;

    protected float animCounter;
    protected bool animSwitch = true;
    protected float jumpCDcounter;
    protected AudioSource throat;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        jumpCDcounter = jumpCD;
        throat = this.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (health < 0) death();
        if (this.transform.position.y < -100)
        {
            Debug.LogError("majiang paole");
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        jumpCDcounter -= Time.deltaTime;
        if (jumpCDcounter <= 0)
        {
            this.transform.Rotate(0, Random.Range(-90.0f, 90.0f), 0);
            jumpCDcounter = jumpCD;
            //throat.PlayOneShot(Resources.Load<AudioClip>("bounce"));
            rb.AddForce(this.transform.rotation * new Vector3(jumpLeapSpeed, 0, 0) * Random.Range(0.5f, 1.5f)
                + new Vector3(0, jumpHeight, 0) * Random.Range(0.5f, 1.5f),
                ForceMode.Impulse);
        }
        if (animSwitch)
        {
            this.transform.localScale += new Vector3(+animSpeed, -animSpeed, 0)
                * Time.deltaTime;
            animCounter += animSpeed * Time.deltaTime;
            if (animCounter >= animMag) animSwitch = !animSwitch;
        }
        else
        {
            this.transform.localScale -= new Vector3(+animSpeed, -animSpeed, 0)
                * Time.deltaTime;
            animCounter -= animSpeed * Time.deltaTime;
            if (animCounter <= -animMag) animSwitch = !animSwitch;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "qiangbi")
        {
            rb.AddForce(new Vector3(rb.velocity.x * -2, 0, rb.velocity.z * -2), ForceMode.Impulse);
        }
        if (col.gameObject.tag == "Player")
        {
            rb.AddForce(
                20 * (this.transform.position - col.gameObject.transform.position) +
                new Vector3(0, 10, 0),
                ForceMode.Impulse);
            throat.PlayOneShot(Resources.Load<AudioClip>("bounce"));
        }
    }

    public void setImg(int type)
    {
        typeID = type;
        // load image according to type
        // Debug.Log("LOADING... majiang/" + typeID.ToString() + ".jpg");
        if (!(Resources.Load("majiang/" + typeID.ToString())))
        {
            Debug.LogWarning("invalid file " + type.ToString());
            return;
        }
        if (!(Resources.Load<Sprite>("majiang/" + typeID.ToString())))
        {
            Debug.LogWarning("invalid id " + type.ToString());
            return;
        }
        sp1.sprite = Resources.Load<Sprite>("majiang/" + typeID.ToString());
        sp2.sprite = Resources.Load<Sprite>("majiang/" + typeID.ToString());
    }

    public void getHit(int dmg)
    {
        rb.AddForce(new Vector3(0, hitJumpHeight, 0), ForceMode.Impulse);
        health -= dmg;
        throat.PlayOneShot(Resources.Load<AudioClip>("meow"));
    }

    void death()
    {
        if (typeID == 36)
        {
            explode();
            Destroy(this.gameObject);
            return;
        }
        if (typeID == 34) typeID = (int)Random.Range(0, 33.90f);
        if(gameManager) gameManager.addScore(typeID);
        GameObject deathClout = Instantiate(deathParticle);
        deathClout.transform.position = this.transform.position;
        Destroy(deathClout, 3.0f);
        Destroy(this.gameObject);
    }

    void explode()
    {
        GameObject exp = Instantiate(explosionParticle);
        exp.transform.position = this.transform.position;
        Destroy(exp, 1.2f);
        if (gameManager) gameManager.isExploding = true;

        foreach (majiang mj in GameObject.FindObjectsOfType<majiang>())
        {
            if (Vector3.Distance(this.transform.position, mj.transform.position) < explodeRad)
            {
                float multiplier = (explodeRad - Vector3.Distance(this.transform.position, mj.transform.position))
                    / explodeRad;
                mj.health -= (int)(multiplier * explodeDmg);
                mj.rb.AddForce(
                    (multiplier * explodeDmg) * (mj.transform.position - this.transform.position) +
                    new Vector3(0, (multiplier * explodeDmg), 0),
                    ForceMode.Impulse);
            }
        }

        foreach (fpsPlayer player in GameObject.FindObjectsOfType<fpsPlayer>())
        {
            player.shakeGo = true;
            if (Vector3.Distance(this.transform.position, player.transform.position) < explodeRad)
            {
                float multiplier = (explodeRad - Vector3.Distance(this.transform.position, player.transform.position))
                    / explodeRad;
                player.health -= (int)(multiplier * explodeDmg) * 3;
                player.rb.AddForce(
                    (multiplier * explodeDmg) * (player.transform.position - this.transform.position) +
                    new Vector3(0, (multiplier * explodeDmg), 0),
                    ForceMode.Impulse);
            }
        }
    }
}
