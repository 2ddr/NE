using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class passiveEquipPrefab : MonoBehaviour
{
    
    private Animator animator;
    public SpriteRenderer spriteForColoring;
    public tank parent;

    public List<itemCost> myCost = new List<itemCost>();

    private bool working=false;
    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponentInChildren<Animator>() != null)
        {
            animator = this.GetComponentInChildren<Animator> ();
           // StopFire();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartWork()
    {
        animator.SetBool("fireing", true);
        working = true;
    }
   
    public void StopWork()
    {
        animator.SetBool("fireing", false);
        working = false;
    }
}
