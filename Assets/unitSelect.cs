using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitSelect : MonoBehaviour
{
   // private float SQ = 24.0f;
    
    Camera cam;
    private bool selected;
    public GameObject select;
    public GameObject menu;
    private Sprite spr;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        selected = false;
        select.SetActive(selected);
        spr = select.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("sel");
            
            //if ((Mathf.Abs(cam.ScreenToWorldPoint(Input.mousePosition).x - select.transform.position.x) < select.GetComponent<Sprite>().rect.Contains()) &&
            //    (Mathf.Abs(cam.ScreenToWorldPoint(Input.mousePosition).y - select.transform.position.y) < SQ * 0.5))
            if (Mathf.Abs(cam.ScreenToWorldPoint(Input.mousePosition).x - spr.rect.x) <spr.rect.width*0.5 &&
                Mathf.Abs(cam.ScreenToWorldPoint(Input.mousePosition).y - spr.rect.y) <spr.rect.height*0.5)
            {
                selected = !selected;
                select.SetActive(selected);
               // menu.SetActive(selected);
                Debug.Log("selected "+ selected);
            }
            else
            {
                selected = false;
                select.SetActive(selected);
               // menu.SetActive(selected);
            }
                

        }*/
    }
}
