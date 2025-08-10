using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bullet : MonoBehaviour
{

    public int mySide = 0;
    //public float damage;
    private float bulletRange;
    public float bulletDamage;
    //public float bulletSpeed;
    public ParticleSystem PS;
    public GameObject effect;
    public Transform effectsGroup;

    private Vector2 startPos;
    private bool disabled = false;

    // Start is called before the first frame update
    void Awake()
    {
        //Init();
    }
  
    public void Init(float bulletRange_)
    {
        startPos = transform.position;
        bulletRange = bulletRange_* G.SQ;

        // Debug.Log("startPos " + startPos);
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // transform.position=new Vector3(transform.position.x,transform.position.y,0);
        if (disabled) return;
        if (Vector2.Distance(transform.position, startPos) > bulletRange) MyDestroy();

        //Debug.Log("Vector2.Distance(transform.position, startPos) " + Vector2.Distance(transform.position, startPos));

    }
    
    public void SetMySide(int side)
    {
        mySide = side;
        /*
        if (side == G.WarSides.PLAYER)
        {
            RecolorAll(Color.red);
        }
        
        if (side == G.WarSides.ENEMY)
        {
            RecolorAll(Color.blue);
        } */       
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (disabled) return;

        if (col.gameObject.tag == "Tank")
            if (col.gameObject.GetComponent<tank>().mySide == mySide)
            {
                
            }
            else
            {
                MyDestroy(col);
            }
            
        if (col.gameObject.tag == "Solid")   
            Destroy(gameObject,1*Time.deltaTime);
        //Debug.Log("GameObject1 collided with " + col.name);
        
        
    }
    

    void RecolorAll(Color col)
    {
        //chassis.color = col;
        GetComponent<SpriteRenderer>().color = col;
        
    }

    void MyDestroy(Collider2D col)
    {
        
        col.GetComponent<tank>().SetDamage(bulletDamage);

        MyDestroy();

    }

    void MyDestroy()
    {
       PS.enableEmission = false;

        Destroy(gameObject, 2);
        
        GameObject o = Instantiate(effect, effectsGroup);
        o.transform.position = transform.position;
        GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        disabled = true;
    }
}
