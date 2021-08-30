using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpsPlayer : MonoBehaviour
{
    public int health = 100;
    public float fireCD = 0.2f;
    public float speed = 5;
    public float mouse_sensityvity = 10.0f;
    public float jumpHeight = 2.0f;
    public float backfireIntensity = 0.2f;
    public Transform firePoint;
    public GameObject myCam;
    public GameObject myBullet;

    // PRIVATE FIELD
    Rigidbody rb;
    float fireCounter = 0;
    float jumpCounter = 0;
    float rotLimit = 0;
    float fireOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 计算时间
        fireCounter -= Time.deltaTime;
        jumpCounter -= Time.deltaTime * 9.8f;
        // 计算开火
        float speedMultiplier = 1.0f;
        if (jumpCounter < 0) jumpCounter = 0;
        if (Input.GetButton("Fire1") && fireCounter <= 0)
        {
            fire();
            fireCounter = fireCD;
        }
        // 后坐力
        if (Input.GetButton("Fire1"))
        {
            fireOffset += Time.deltaTime * backfireIntensity;
            speedMultiplier = 0.6f;
        }
        else fireOffset = 0;
        // 移动
        if (Input.GetKey(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.1f)
            jumpCounter = jumpHeight;
        Vector3 moveVec = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical"));
        Vector3 jumpVec = new Vector3(
            0,
            jumpCounter,
            0);
        rb.MovePosition(transform.position + jumpVec +
            (this.transform.rotation * moveVec).normalized * speed * Time.deltaTime * speedMultiplier);
        // 旋转
        this.transform.Rotate(0, Input.GetAxis("Mouse X") * mouse_sensityvity, 0);
        rotLimit += -Input.GetAxis("Mouse Y") * mouse_sensityvity;
        if (rotLimit > -30 && rotLimit < 30)
            myCam.transform.Rotate(-Input.GetAxis("Mouse Y") * mouse_sensityvity, 0, 0);
        else
            rotLimit -= -Input.GetAxis("Mouse Y") * mouse_sensityvity;
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
}
