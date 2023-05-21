using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animateonlook : MonoBehaviour
{

    public bool IsLookedAt = false;
    Animator anim;
    Vector3 Movedirection;
    public float Movespeed = 0.01f;
    float speedrandomizer;
    public bool Stationary = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        speedrandomizer = Random.Range(-0.2f, 0.2f);
    }

    // Update is called once per frame
    

    void Update()
    {
        Movedirection = (TAUXRPlayer.Instance.transform.position - transform.position).normalized;
        Movedirection.y = 0.0f;

        //while(Vector3.Distance(transform.position, TAUXRPlayer.Instance.transform.position)> 0.5f)
        //{
            
        //}
        if (IsLookedAt)
        {
            anim.SetFloat("Speed", 0);
            anim.SetBool("LookedAt", true);
        }
        else
        {
            anim.SetFloat("Speed", 1+ speedrandomizer);
            anim.SetBool("LookedAt", false);

            if (!Stationary)
            {
                transform.position += Movedirection * Movespeed * (1 + speedrandomizer);
            }
            
        }
        //anim.SetFloat("Speed", 0);

    }
}
