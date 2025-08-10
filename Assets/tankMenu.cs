using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tankMenu : MonoBehaviour
{
    
    //private GameObject[] aUnits;
    //private List<tank> aUnits;// = new List<tank>();
    [SerializeField] private selectUnit selUnit;
        
    [SerializeField] Button CaptureBasesBtn;
    [SerializeField] Button CaptureFacilitiesBtn;
    [SerializeField] Button DestroyEnemiesBtn;
    
    // Start is called before the first frame update
    void Start()
    {
        //aUnits=new List<tank>();
        //gameObject.SetActive(false);

        //CaptureBasesBtn.onClick += SetCaptureBases;
    }


    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void SetCaptureBases()
    {
        Debug.Log("SetCaptureBases");
        
        foreach (tank unit in selUnit.aUnits)
        {
            unit.GetComponent<tank>().SetTask(G.CAPTURE_BASES);
        }
    }

    public void SetCaptureFacilities()
    {
        
        Debug.Log("SetCaptureFacilities");
        foreach (tank unit in selUnit.aUnits)
        {
            unit.GetComponent<tank>().SetTask(G.CAPTURE_FACTORIES);
        }
    }
    
    public void SetDestroyEnemies()
    {
        Debug.Log("DestroyEnemies");
        foreach (tank unit in selUnit.aUnits)
        {
            unit.GetComponent<tank>().SetTask(G.DESTROY_ENEMIES);
        }
    }
    /*
    public void AddUnit(tank unit)
    {
        
        //aUnits.Add(unit);
        //Debug.Log("Add "+aUnits.Count);
    }
    
    public void RemoveUnit(tank unit)
    {
        //aUnits.Remove(unit);
        //Debug.Log("Remove "+aUnits.Count);
    }   */
}
