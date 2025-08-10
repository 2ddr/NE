using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class timer : MonoBehaviour
{
    public Transform FactoryGroup;
    public Transform PlayersList;

    [SerializeField] private TextMeshProUGUI tmPro;
    //public Text label;
    
    private float sec=0.0f;
    private int min=0;
    private int hour=0;
    private int day=0;

    private int minLast = 0;
    
    //public Time time;
    // Start is called before the first frame update
    void Start()
    {
        tmPro.SetText("haha");
        //StartCoroutine(Count());
       // tmPro = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (100 - sec <=0)
        {
            TimerUpdate();
            minLast = min;
        }
        //cicleUpdate += Time.deltaTime;



        sec += 1*Time.deltaTime;


        if (sec > 60.0f)
        {
            sec -= 60.0f;
            min += 1;
            //TimerUpdate();
            
        }

        if (min > 59)
        {
            min -=60;
            hour += 1;
           // TimerUpdate();
        }

        if (hour > 23)
        {
            hour -= 24;
            day += 1;
           // TimerUpdate();

            
        }
        DayEndAction();

    }

    void TimerUpdate()
    {
        tmPro.SetText("Day " + day + "\n" + hour + ":" + min + ":" + sec.ToString());
    }


    IEnumerator Count()
    {

        for (int d = 1; d < 3650; d += 1)
        {
            for (int h = 0; h < 24; h += 1)
                for (int m = 0; m < 1; m += 1)
                    for (int s = 0; s < 1; s += 1)
                    {
                        
                        yield return new WaitForSeconds(0.01f);
                    }
            DayEndAction();
            yield return new WaitForSeconds(0.001f); ;
        }


        

    }

    void DayEndAction()
    {

        foreach (Transform child in FactoryGroup)
        {
          //  child.GetComponent<building>().TimeToAdd();
        }

        foreach (Transform child in PlayersList)
        {
            child.GetComponent<playerController>().RenewResourcesText();
        }
    }

}
