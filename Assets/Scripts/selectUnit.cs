using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectUnit : MonoBehaviour
{
    private float SQ = G.SQ;
    
    public RectTransform selectionBox;
    private Vector2 startPos;
    private Camera cam;

    public GameObject tankGroup;
    public List<tank> aUnits = new List<tank>();

    public tankMenu tMenu;

    private bool skipSelection=false;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            startPos = (Input.mousePosition);
            skipSelection = (Physics2D.OverlapPoint(Input.mousePosition, LayerMask.GetMask("UI")) != null);
            Debug.Log("skipSelection " + skipSelection);
        }


// mouse held down
        if(Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
            ReleaseSelectionBox();
        }
        
        // mouse up
        if(Input.GetMouseButtonUp(0))
        if (!skipSelection)
        {
            
            ReleaseSelectionBox(true);
            
            if (aUnits.Count>0)
            {
                tMenu.gameObject.SetActive(true);
            }
            else
            {
                tMenu.gameObject.SetActive(false);
            }
            
            skipSelection = false;
            selectionBox.gameObject.SetActive(false);
        }
        
        
    }
    
    
    // called when we are creating a selection box
    void UpdateSelectionBox (Vector2 curMousePos)
    {
        if(!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);
 
        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;
 
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }
    
    // called when we release the selection box
    void ReleaseSelectionBox (bool finish=false)
    {
        //if (finish) selectionBox.gameObject.SetActive(false);
 
        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);
 
        aUnits.Clear();
        
        foreach(tank unit in tankGroup.GetComponentsInChildren(typeof(tank)))
        {
            
            Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);
        
            if((screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y
               || Mathf.Abs(screenPos.x-min.x)<SQ*0.5 && Mathf.Abs(screenPos.y-min.y)<SQ*0.5) && unit.mySide==G.PLAYER_SIDE)
            {
               
               if (!unit.GetDestroyed())
               {
                   unit.Select();
                   aUnits.Add(unit);
               }
               
            }
            else
            {
                if (!unit.GetDestroyed())
                unit.UnSelect();
            }
            
        }
    }
}
