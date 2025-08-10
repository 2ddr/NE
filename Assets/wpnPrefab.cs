using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class wpnPrefab : MonoBehaviour
{
    public Transform end1;
    public Transform end2;
    
    private Animator animator;

    public SpriteRenderer spriteForColoring;

    [SerializeField] GameObject bullet;
    private GameObject bulletLayer;

    public tank parent;

    public float bulletRange;
    public float bulletDamage;
    public float bulletVelocity;

    public List<itemCost> myCost= new List<itemCost>();

    //public ItemCost Price;

    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponentInChildren<Animator>() != null)
        {
            animator = this.GetComponentInChildren<Animator> ();
            StopFire();
        }
        
        bulletLayer = GameObject.FindGameObjectWithTag("Bullet Layer");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void End1Fire()
    {
        if (!animator.GetBool("firing")) return;

        Rigidbody2D ShellInstance = Instantiate(bullet,bulletLayer.transform).GetComponent<Rigidbody2D>();
        ShellInstance.GetComponent<bullet>().SetMySide(parent.mySide);
        ShellInstance.GetComponent<bullet>().effectsGroup = parent.effectsGroup;
        ShellInstance.GetComponent<bullet>().bulletDamage = bulletDamage;
        //ShellInstance.GetComponent<bullet>().bulletRange = bulletRange*G.SQ;
        ShellInstance.gameObject.transform.rotation    = end1.rotation;
        ShellInstance.gameObject.transform.position    = end1.position;
        ShellInstance.linearVelocity                         = end1.transform.right * bulletVelocity;

        ShellInstance.gameObject.GetComponent<bullet>().Init(bulletRange);
    }
    public void End2Fire()
    {
        if (!animator.GetBool("firing")) return;
        Rigidbody2D ShellInstance = Instantiate(bullet, bulletLayer.transform).GetComponent<Rigidbody2D>();
        ShellInstance.GetComponent<bullet>().SetMySide(parent.mySide);
        ShellInstance.GetComponent<bullet>().effectsGroup = parent.effectsGroup;
        ShellInstance.GetComponent<bullet>().bulletDamage = bulletDamage;
        //ShellInstance.GetComponent<bullet>().bulletRange = bulletRange * G.SQ;
        ShellInstance.transform.rotation                = end2.rotation;
        ShellInstance.transform.position                = end2.position;
        ShellInstance.linearVelocity                          = end2.transform.right * bulletVelocity;
        ShellInstance.gameObject.GetComponent<bullet>().Init(bulletRange);
    }
    
    public void StartFire()
    {
        animator.SetBool("firing", true);
    }
   
    public void StopFire()
    {
        animator.SetBool("firing", false);
    }
}
