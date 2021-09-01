using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpsPlayer : MonoBehaviour
{
    public int health = 20;
    public float fireCD = 0.2f;
    public float speed = 5;
    public float mouseSensityvity = 10.0f;
    public float backfireIntensity = 0.2f;
    public float walkAnimMag;
    public float walkAnimSpeed;
    public float jumpForce = 10.0f;
    public AudioSource throat;
    public Transform firePoint;
    public GameObject myCam;
    public GameObject myBullet;
    public generator gameManager;
    public Rigidbody rb;
    public bool shakeGo = false;

    // PRIVATE FIELD
    protected float fireCounter = 0;
    protected float rotLimit = 0;
    protected float fireOffset = 0;
    protected int walkCamShakeSwitch = 0;
    protected float walkAnimCounter;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        rb = this.gameObject.GetComponent<Rigidbody>();
        throat = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            while (gameManager && gameManager.scoreCount < 13) gameManager.addScore(35);
        }
        if (gameManager && gameManager.scoreCount >= 13) return;
        // shake
        if (shakeGo)
        {
            StartCoroutine(camShake(1.0F, 0.02F, 0.3F));
            shakeGo = false;
        }
        // 计算时间
        fireCounter -= Time.deltaTime * (1 + fireOffset * 0.1f);
        // 计算开火
        float speedMultiplier = 1.0f;
        if (Input.GetButton("Fire1") && fireCounter <= 0)
        {
            fire();
            fireCounter = fireCD;
        }
        // 移动
        if (Input.GetButton("Fire1")) speedMultiplier = 0.35f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 3, ~(1 << 8))
            && Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        Vector3 moveVec = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical"));
        rb.MovePosition(transform.position +
            (this.transform.rotation * moveVec).normalized * speed * Time.deltaTime * speedMultiplier);
        // 后坐力
        if (Input.GetButton("Fire1"))
        {
            fireOffset += Time.deltaTime * backfireIntensity;
            myCam.transform.localPosition = new Vector3(
                Random.Range(-fireOffset, fireOffset),
                Random.Range(-fireOffset, fireOffset),
                Random.Range(-fireOffset, fireOffset)
                ) * 0.01F;
        }
        else if (moveVec.sqrMagnitude > 0.1f)
        {
            //throat.PlayOneShot(Resources.Load<AudioClip>("step2"));
            if (walkCamShakeSwitch == 0)
            {
                myCam.transform.localPosition += new Vector3(+walkAnimSpeed, -walkAnimSpeed, 0)
                    * Time.deltaTime;
            }
            else if (walkCamShakeSwitch == 1)
            {
                myCam.transform.localPosition += new Vector3(-walkAnimSpeed, +walkAnimSpeed, 0)
                    * Time.deltaTime;
            }
            else if (walkCamShakeSwitch == 2)
            {
                myCam.transform.localPosition += new Vector3(-walkAnimSpeed, -walkAnimSpeed, 0)
                    * Time.deltaTime;
            }
            else
            {
                myCam.transform.localPosition += new Vector3(+walkAnimSpeed, +walkAnimSpeed, 0)
                    * Time.deltaTime;
            }
            walkAnimCounter += walkAnimSpeed * Time.deltaTime;
            if (walkAnimCounter >= walkAnimMag)
            {
                walkAnimCounter = 0;
                walkCamShakeSwitch = (1 + walkCamShakeSwitch) % 4;
            }
        }
        else
        {
            myCam.transform.localPosition = new Vector3(0, 0, 0);
            fireOffset = 0;
        }
        // 旋转
        this.transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensityvity, 0);
        rotLimit += -Input.GetAxis("Mouse Y") * mouseSensityvity;
        if (rotLimit > -60 && rotLimit < 60)
            myCam.transform.Rotate(-Input.GetAxis("Mouse Y") * mouseSensityvity, 0, 0);
        else
            rotLimit -= -Input.GetAxis("Mouse Y") * mouseSensityvity;
    }

    void fire()
    {
        GameObject bullet = Instantiate(myBullet);
        bullet.transform.position = this.firePoint.position;
        bullet.transform.eulerAngles = new Vector3(
            myCam.transform.eulerAngles.x + Random.Range(-fireOffset, fireOffset),
            myCam.transform.eulerAngles.y + Random.Range(-fireOffset, fireOffset),
            myCam.transform.eulerAngles.z + Random.Range(-fireOffset, fireOffset)
            );
    }

    public IEnumerator camShake(float shakeForSec = 0.5f, float frequency = 0.02f, float offset = 0.1f)
    {
        while (shakeForSec >= 0)
        {
            myCam.transform.localPosition = new Vector3(
                Random.Range(-offset, offset),
                Random.Range(-offset, offset),
                Random.Range(-offset, offset)
                );
            yield return new WaitForSecondsRealtime(frequency);
            shakeForSec -= frequency;
        }
        myCam.transform.localPosition = new Vector3(0, 0, 0);
    }
}
