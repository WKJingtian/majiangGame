using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMajiang : majiang
{
    // Update is called once per frame
    void FixedUpdate()
    {
        fpsPlayer target = FindObjectOfType<fpsPlayer>();

        jumpCDcounter -= Time.deltaTime;
        if (jumpCDcounter <= 0)
        {
            this.transform.Rotate(0, Random.Range(-90.0f, 90.0f), 0);
            jumpCDcounter = jumpCD;
            //throat.PlayOneShot(Resources.Load<AudioClip>("bounce"));
            rb.AddForce((transform.position - target.transform.position).normalized * Random.Range(0.2f, 3.0f)
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
}
