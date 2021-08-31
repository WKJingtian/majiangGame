using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletSpeed = 15.0f;
    public int damage;
    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += this.transform.rotation * new Vector3(0, 0, bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "majiang")
        {
            GameObject exp = Instantiate(hitEffect);
            exp.transform.position = this.transform.position;
            Destroy(exp, 0.2f);

            other.gameObject.GetComponent<majiang>().getHit(damage);
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "qiangbi" ||
            other.gameObject.tag == "diban" ||
            other.gameObject.tag == "tianhuaban")
        {
            GameObject exp = Instantiate(hitEffect);
            exp.transform.position = this.transform.position;
            Destroy(exp, 0.2f);

            Destroy(this.gameObject);
        }
    }
}
