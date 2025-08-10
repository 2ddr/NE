using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking.Match;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = System.Random;
using DG.Tweening;
//[Serializable]
public class tank : MonoBehaviour
{
  //  Vector2 startPos = Vector2.zero;
  //  Vector2 endPos = Vector2.zero;
  //  Rect selectRect = new Rect();
    //public Transform camRig;
    private float SQ = G.SQ;
    
    private Collider2D C2D;
    [SerializeField] Collider2D menuC2D;

    public GameObject fogMask;

    public Transform effectsGroup;

    private const int F_NOCTRL	 = 0;
    private const int F_NOTASK	 = 5;
    private const int F_IDLE	 = 10;
    private const int F_ROTATE= 20;
    private const int F_MOVE	 = 30;
    private const int F_MEETTARGET	 = 40;
    private const int F_CAPTURE	 = 50;

    private const int F_ACCEL	 = 60;
    private const int F_MOVE2	 = 70;
    private const int F_BRAKE	 = 80;
    public int mySide = 0;
    
    public int fase	= -1;
    public int faseNext	= F_NOTASK;
    public float fcounter = 0;
    
    
    public GameObject chassis;
    //public SpriteRenderer basis;   
    public GameObject gun1;
    //public SpriteRenderer gun2;
    //public SpriteRenderer addon;

    [SerializeField] GameObject myBase;
    [SerializeField] GameObject myHead;

    private Animator gun1Anim;
        
    /*
    public Image chassisSrc;
    public Image basisSrc;   
    public Image gun1Src;
    public Image gun2Src;
    public Image addonSrc;
*/
    public List<Vector2> wayPoints = new List<Vector2>();
    private Vector2 prevPoint;
    private Vector2 nextPoint;
    private Vector2 currentPoint;
    
    private Vector2 targetPoint;
    private building target;
    
    private GameObject headTarget;  // Head target
    private bool headHaveTarget=false;
    private float headTargetSearchCounter=0.2f;
        
    private Vector2 dir;
    private int dirN;
    private int tryingDirN=99;
    private float currAngle;
    private float newAngle;

    private float headNewAngle;
    

    private float accelDist = 1;
    private float speed = 30;
    public float speedMax = 30;

    private Vector2 maxVelocity;
    
    
    Vector2[] aDir = new Vector2[4];
    public Vector2[] tryDir = new Vector2[4];
    private bool moveX;
   
    Camera cam;
    private bool selected;
    public GameObject selectorObj;
    public GameObject selectorObjGroup;
    public tankMenu tankmenu;
    public GameObject explo;

    int task = 0; // capture bases etc  
    int nextTask = 0; 
    
   // [SerializeField] database db;
   private float health=100;
    private float fireRange = 0;

    private bool destroyed = false;
    public tankMoveHelper moveHelper;

    private Tween moveTween;

    private List<Vector2> aBlocked = new List<Vector2>();

    private float sightRange=7.0f;// here in squares

    private CanvasGroup canvasGroup;

        // Start is called before the first frame update
    void Start()
    {
        //EffectsGroup = GameObject.FindGameObjectWithTag("Effects Layer").GetComponent<Transform>();

        dir = new Vector2(0,-1);
        
        aDir[0]=new Vector2(1,0);
        aDir[1]=new Vector2(0,1); 
        aDir[2]=new Vector2(-1,0);
        aDir[3]=new Vector2(0,-1);
        
        cam = Camera.main;
        
        selectorObj = Instantiate(selectorObj, selectorObjGroup.transform);    
        selectorObj.SetActive(false);
        
        selected = false;
        
        SetToSqCenter();
        prevPoint = new Vector2(transform.position.x,transform.position.y);
        nextPoint = new Vector2(transform.position.x,transform.position.y);
        
        accelDist = 0.9f * (SQ * 0.5f);
        
        C2D = GetComponent<Collider2D>();

        nextTask = G.CAPTURE_FACTORIES;
        currAngle = transform.eulerAngles.z;

        dirN = 99;
        tryingDirN = 99;

        sightRange*=SQ;
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetEquipment(chassis chassis_,gun gun1_)
    {
        
        //Debug.Log("* * * chassis name: "+chassis_.name+" / gun1 name: "+gun1_.name);
        chassis = Instantiate(chassis_.prefab,myBase.transform);
        
        gun1 = Instantiate(gun1_.prefab,myHead.transform);
        //gun1.GetComponent<wpnPrefab>().bullet = gun1_.bullet;
        //gun1.GetComponent<wpnPrefab>().bulletLayer=GameObject.FindGameObjectWithTag("Bullet Layer");
        gun1.GetComponent<wpnPrefab>().parent = GetComponent<tank>();  
        gun1Anim = gun1.GetComponent<Animator> ();
        gun1Anim.SetBool("firing", false);
        
        //SetMySide((Mathf.Round(UnityEngine.Random.Range(0.0f,1.0f))>0.5f?G.WarSides.ENEMY:G.WarSides.PLAYER));
        
       // Debug.Log("gunAnim "+gun1Anim);

        speedMax = chassis_.speed[0];
        fireRange = gun1_.range*G.SQ;

        if (fogMask!=null)
        {
            float sc = 2.0f * fogMask.transform.localScale.x * (gun1.GetComponent<wpnPrefab>().bulletRange + 1);
           // fogMask.transform.localScale = new Vector3(sc, sc, 1.0f);
            fogMask.transform.DOScale(sc, 1);
        }
    }
    
    private void Awake()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyed) return;

        if (mySide == G.PLAYER_SIDE)
        {
            fogMask.transform.position = transform.position;
            selectorObj.transform.position = cam.WorldToScreenPoint(transform.position);
        }

        if (fase != faseNext || fcounter<0) 
        {
            fcounter = 0.0f;
            fase = faseNext;

            //Debug.Log(fase + " - ");
        }
        else 
        {
            fcounter += Time.deltaTime;				
        }
		
        switch (fase) {
            //case F_NOCTRL: 	FNoCtrl();
            case F_NOTASK: 	    FNotask(); break;
            case F_IDLE: 	    FIdle(); break;

            case F_ACCEL: 	    FAcceleration(); break;
            case F_MOVE2: 	    FMove2(); break;
            case F_BRAKE: 	    FBrake(); break;

            case F_ROTATE: 	    FRotate(); break;
            case F_MOVE: 	    FMove(); break;
            case F_MEETTARGET: 	FMeetTarget(); break;
            case F_CAPTURE:     FCapture();
                break;
            
			
        }	

        Vector3 v1 = transform.position;
        v1.z = -1;
        Vector3 v2 = nextPoint;
        v2.z = -1;
       // Debug.DrawLine(v1,v2,Color.green);

        if (target != null)
        {
            Debug.DrawLine(transform.position, targetPoint, Color.gray);
            Debug.DrawLine(transform.position, V3V2(transform.position)+SQ*Dir90ToPoint(targetPoint), Color.cyan);
            Debug.DrawLine(currentPoint, nextPoint, Color.magenta);
            // Debug.DrawRay(myHead.transform.position,V2V3(Vector2.Angle(Vector2.right,target.transform.position-myHead.transform.position)),Color.red);
        }
        
        
    }
    
    private void FixedUpdate()
    {
        for (int i = 0; i < wayPoints.Count-1; i++)
        {
            Debug.DrawLine(wayPoints[i],wayPoints[i+1], Color.white);
        }

        HeadControl(); 

        //CheckMeInFog();
    }

    void FNotask()
    {
        if (fcounter == 0)
        {
            if (ChooseTarget())
            {
                task = nextTask;
                faseNext = F_IDLE;
                //nextFase = ChooseTarget();
            }
        }
        

        if (fcounter > 1.0f)
        {
            fcounter = -1;
        }
    }
    
    
    void FIdle()
    {
        if (fcounter == 0) 
        {

            SetToSqCenter();



            if (TargetIsActual())
            {
                nextPoint = ChooseNextPoint(transform.position);

                //Debug.Log("NP: " + nextPoint + " / TP: " + transform.position);
                {
                    faseNext = F_ROTATE;
                }
            }
            else
                faseNext = F_NOTASK;

        }

        if (fcounter > 1.0f)
        {
            fcounter = -1;
        }
        //Debug.Log("NP: "+nextPoint+" / TP: "+transform.position);

    }

    bool TargetIsActual()
    {
        if (target != null)
        {
            if (target.GetComponent<building>().mySide == mySide)
            {
                target = null;
                return false;
            }

            return true;
        }
        /*
        if (target == null)
            if (ChooseTarget())
            {
                //Debug.Log("nextPoint " + nextPoint);
                if (Vector2.Distance(transform.position, nextPoint) > 1.0f)
                    return true;

                return false;
            }
            else return false;
        */
        return false;
    }

    void FRotate()
    {
        if (fcounter == 0) 
        {
            //Debug.Log("FRotate nextPoint " + nextPoint);
            moveTween=myBase.transform.DORotate(new Vector3(0f,0f,DirToAngle(tryDir[dirN])),1.0f).SetEase(Ease.InOutCubic);
            moveTween.OnComplete(()=>
                {
                    faseNext = F_ACCEL;
                });
            //Debug.Log("FRotate moveTween " + moveTween);
        }

       // newAngle =Mathf.MoveTowardsAngle(myBase.transform.eulerAngles.z, DirToAngle(tryDir[dirN]),3);
       // myBase.transform.eulerAngles = new Vector3(0,0,newAngle);
        /*
        if (moveTween.IsComplete())//if (Mathf.Abs(myBase.transform.eulerAngles.z - DirToAngle(tryDir[dirN])) < 5)
        {
           // faseNext = F_MOVE;
            faseNext = F_ACCEL;
            //myBase.transform.eulerAngles = new Vector3(0,0,DirToAngle(tryDir[dirN]));
        }
        */
    }
    
    void PrepareToMove()
    {

    }
    void FAcceleration()
    {
        if (fcounter == 0)
        {
            prevPoint = GetClosestSqCenter(transform.position);
            currentPoint=prevPoint;
            //Debug.Log("PP: "+prevPoint+" / NP: "+nextPoint);
            if (moveHelper.SqIsEmpty(nextPoint))
            {
                SetBlocked(nextPoint);
                //Debug.Log(" Empty NP: "+nextPoint);
            }
                
            else {
                //Debug.Log(" Blocked NP: "+nextPoint);
                faseNext=F_IDLE;
                return;
            }
        }

        Vector2 lastXY= new Vector2();
        lastXY = transform.position;

        transform.position = Vector2.MoveTowards(transform.position, nextPoint, Time.deltaTime * CalcSpeed(prevPoint, transform.position));

        if (Vector2.Distance(prevPoint,transform.position)>=SQ*0.5)
        faseNext = F_MOVE2;
/*
        if (moveX)
        {
            
            if (CheckCrossSQ(lastXY.x, transform.position.x))
            {
                faseNext = F_MOVE2;

                
            }
        }
        else
        {
            if (CheckCrossSQ(lastXY.y, transform.position.y))
            {
                faseNext = F_MOVE2;
            }
        }*/

/*
        if (Vector2.Distance(transform.position, nextPoint) < speed * Time.deltaTime)
        {
            faseNext = F_MOVE2;
        }
        */
    }

    void FMove2()
    {
        if (fcounter == 0)
        {
            //moveHelper.SetEmpty(prevPoint);

            //AddWayPointsTo(GetBetterNextPointFrom(GetClosestSqCenter(transform.position)));
            prevPoint=currentPoint;
            currentPoint = nextPoint;

            SetUnblockedAll();
            
/*
            if (currentPoint == targetPoint)
            {
                faseNext=F_BRAKE;
                nextPoint=currentPoint;
                return;
            }
*/
           
            
            if (HereLineToTarget(currentPoint))
            {
                if (moveHelper.SqIsEmpty(currentPoint+SQ*(targetPoint-currentPoint).normalized))
                    FillDirectionsArray(currentPoint);
                //nextPoint = GetBestNextPointFrom(currentPoint);
                //goto beginMove2;
            
            }
            /*
            else

            if (dirN>0)
            if (moveHelper.SqIsEmpty(currentPoint+tryDir[dirN-1]*SQ))
            {
                //FillDirectionsArray(currentPoint);
                dirN=dirN-1;
            }
               // if (dirN<4 && dirN>=0)
                while (!moveHelper.SqIsEmpty(currentPoint+tryDir[dirN]*SQ) && dirN<3)
                {
                    dirN+=1;
                }
                
                if (dirN>3)
                {
                    faseNext=F_BRAKE;
                    nextPoint=currentPoint;
                    return;
                }
                else
                {


                    nextPoint=currentPoint+tryDir[dirN]*SQ;
                }
                /*
                nextPoint=ChooseNextPoint(nextPoint,true);//nextPoint=currentPoint+ Dir90FromToPoint(currentPoint,targetPoint)*SQ;

                while (!moveHelper.SqIsEmpty(nextPoint))
                {

                    nextPoint=ChooseNextPoint(nextPoint);
                }
*/

                nextPoint = GetBestNextPointFrom(currentPoint);

                if (currentPoint==targetPoint) 
                {
                    nextPoint = currentPoint;
                    faseNext=F_BRAKE;                
                    return;
                }
            /*}
            else
            if (moveHelper.SqIsEmpty(currentPoint+dir*SQ))
            {
                nextPoint=GetClosestSqCenter(currentPoint+dir*SQ);
                
            }
            else
            {
                nextPoint=ChooseNextPoint(nextPoint);
            }
*/
            SetBlocked(currentPoint);
            SetBlocked(nextPoint);
            
            Debug.Log(" FMove2 NP: "+nextPoint+" / CP: "+currentPoint);



            if (currentPoint!=nextPoint) 
            {
                AddWayPointsTo(nextPoint);

                if (wayPoints.Count==0)
                {
                    Debug.Log(" FMove2 NO WAYPOINTS???");
                    nextPoint = currentPoint;
                    faseNext=F_BRAKE;
                    return;
                }
            }
            else
            {
                    nextPoint = currentPoint;
                    faseNext=F_BRAKE;
                    return;
            }

               
            Debug.Log(" FMove2 wayPoints Count: "+wayPoints.Count);
            for (int i = 0; i < wayPoints.Count; i++)
            {
                Debug.Log(wayPoints[i]);
            }

        }


        if (wayPoints.Count==0)
        {
            fcounter=-1.0f;
            return;
        } 

        Vector2 lastXY= new Vector2();
        lastXY = transform.position;



            transform.position = Vector2.MoveTowards(transform.position, wayPoints[0], Time.deltaTime * speedMax);
/*
        newAngle =Mathf.MoveTowardsAngle(
            myBase.transform.eulerAngles.z, 
            Vector2.SignedAngle(Vector2.right, V3V2(transform.position) - lastXY),
            100*Time.deltaTime
            );
        myBase.transform.eulerAngles = new Vector3(0,0,newAngle);

            */
        myBase.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, V3V2(transform.position) - lastXY));



        if (Vector2.Distance(transform.position, wayPoints[0]) < speedMax * Time.deltaTime)
        {
            wayPoints.RemoveAt(0);
            if (wayPoints.Count==0)
                fcounter=-1.0f;

            return;
        }

    }
    
    void FBrake()
    {
        if (fcounter == 0)
        {
            Debug.Log("FBrake / NP: "+nextPoint);

            SetUnblockedAll();
            SetBlocked(nextPoint);
            //myBase.transform.Rotate(new Vector3(0, 0,90+Vector2.Angle(currentPoint,nextPoint)));
            myBase.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, nextPoint-V3V2(transform.position)));
             //myBase.transform.rotation= new Vector3(0, 0,90+Vector2.Angle(currentPoint,nextPoint));
           // myBase.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(V3V2(transform.position), nextPoint));
        }

        transform.position = Vector2.MoveTowards(transform.position, nextPoint, Time.deltaTime * CalcSpeed(transform.position,nextPoint));

        if (Vector2.Distance(transform.position, nextPoint) < speed * Time.deltaTime)
        {
            dirN=99;
            
            transform.position=GetClosestSqCenter();

            if (targetPoint==V3V2(transform.position))
            {
                faseNext = F_CAPTURE;
            }
            else
            {
                faseNext = F_IDLE;
            }

        }
    }

    void FMove()
    {
        if (fcounter == 0) 
        {
            prevPoint = transform.position;
        }

        Vector2 lastXY= new Vector2();
        lastXY = transform.position;

        speed = CalcSpeed();
        
        transform.position = Vector2.MoveTowards(transform.position, nextPoint, Time.deltaTime * speed);

        if (Vector2.Distance(transform.position, nextPoint) < speed * Time.deltaTime)
        {
            transform.position = nextPoint;
            speed = 0.0f;
            faseNext = (Vector2.Distance(nextPoint, targetPoint) < 1.0f ? F_MEETTARGET : F_IDLE);

            //Debug.Log("FMove nextPoint " + nextPoint);

            if (Vector2.Distance(nextPoint,prevPoint)>SQ*2)
            {
                dirN = 99;
                //tryingDirN = 99;
            }
            else
            {
                //tryingDirN = dirN;
            }

            return;
        }
        /*
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, SQ*0.95f, LayerMask.GetMask("solid"));

        if (hit.collider != null || moveHelper.SqIsEmpty(transform.position.x+dir.x*SQ,transform.position.y+dir.y*SQ))
        {
            nextPoint = GetClosestSqCenter();
            //transform.position = nextPoint;
            return;
        }*/
        if (moveHelper.SqIsEmpty(transform.position.x+dir.x*SQ,transform.position.y+dir.y*SQ))
        {
            nextPoint = GetClosestSqCenter();
            
            return;
        }

        if (dirN > 0)
            if (moveX)
            {
                if (CheckCrossSQ(lastXY.x, transform.position.x))
                {
                    //Debug.Log("FMove Check dir " + tryDir[dirN - 1]);
                    if (Physics2D.Raycast(transform.position, tryDir[dirN - 1], SQ * 0.95f, LayerMask.GetMask("solid")).collider == null)
                    {
                        nextPoint = GetClosestSqCenter();
                        tryingDirN = dirN - 1;
                        //Debug.Log("FOUND!");
                    }
                }
            }
            else
            {
                if (CheckCrossSQ(lastXY.y, transform.position.y))
                {
                    //Debug.Log("FMove Check dir " + tryDir[dirN - 1]);
                    if (Physics2D.Raycast(transform.position, tryDir[dirN - 1], SQ * 0.95f, LayerMask.GetMask("solid")).collider == null)
                    {
                        nextPoint = GetClosestSqCenter();
                        tryingDirN = dirN - 1;
                        //Debug.Log("FOUND!");
                    }

                }
            }
        
        

            if (target!=null)
            if (moveX)
            {
                if (CheckCrossSQ(lastXY.x, transform.position.x))
                {
                    if (!CheckCrossSQ(targetPoint.x, transform.position.x))
                    {
                        if (Physics2D.Raycast(transform.position, Dir90ToPoint(targetPoint), SQ * 0.95f, LayerMask.GetMask("solid")).collider == null)
                            nextPoint = GetClosestSqCenter();
                    }
                }
            }
            else
            {
                if (CheckCrossSQ(lastXY.y, transform.position.y))
                {
                    if (!CheckCrossSQ(targetPoint.y, transform.position.y))
                        if (Physics2D.Raycast(transform.position, Dir90ToPoint(targetPoint), SQ*0.95f, LayerMask.GetMask("solid")).collider==null)
                            nextPoint = GetClosestSqCenter();

                }
            }

    }

    void FMeetTarget()
    {
        if (fcounter == 0)
        {
            //Debug.Log("MEET TARGET");
            //target = null;
            //targetPoint.Set(0.0f,0.0f);
            faseNext = F_CAPTURE;
            dirN = 99;
        }
        
        

       
    }

    void FCapture()
    {
        if (fcounter == 0)
        {
            if (target.mySide == mySide)
            {
                faseNext = F_NOTASK;               
                target = null;
                targetPoint.Set(0.0f,0.0f);
                
                return;
            }
            
        }

        if (fcounter > 3.0f)
        {
            if (target != null)
            {

                target.CaptureMe(mySide);
               
                target = null;
            }
            
            targetPoint.Set(0.0f,0.0f);
            faseNext = F_NOTASK;
        }
            
        
    }
    
    bool CheckPointInDirection(Vector2 checkDir)
    {
        return moveHelper.SqIsEmpty(transform.position.x+checkDir.x*SQ,transform.position.y+checkDir.y*SQ);
    }
    bool CheckPointInDirectionFrom(Vector2 pos, Vector2 checkDir)
    {
        return moveHelper.SqIsEmpty(pos.x+checkDir.x*SQ,pos.y+checkDir.y*SQ);
    }


    Vector2 ChooseNextPoint(Vector2 fromPnt, bool reset=false)
    {
        Debug.Log("1 ChooseNextPoint nextPoint=" + nextPoint + " / tryingDirN = " + tryingDirN + " / dirN = " + dirN + " / dir = " + dir);
        
        if (reset) 
            FillDirectionsArray(fromPnt);
        else
            if (tryingDirN < 4)
                dirN = tryingDirN;
            else
            {
                dirN += 1;

                if (dirN > 3)
                {
                    dirN = 0;
                    FillDirectionsArray(fromPnt);
                }
            }



        tryingDirN = 99;

        Vector2 pnt = new Vector2();
        
        dir = tryDir[dirN];
        pnt.x = fromPnt.x + dir.x * SQ;// * UnityEngine.Random.Range(3, 10);
        pnt.y = fromPnt.y + dir.y * SQ;// * UnityEngine.Random.Range(3, 10);
        
        moveX = Mathf.Abs(dir.x) > 0.5f;

        Debug.Log("2 ChooseNextPoint nextPoint=" + nextPoint + " / tryingDirN = " + tryingDirN + " / dirN = " + dirN + " / dir = " + dir);

        return pnt;
    }

    void FillDirectionsArray(Vector2 fromPnt)
    {      

        dirN = 0;

        Vector2 vec = new Vector2();
        vec = targetPoint - (new Vector2(fromPnt.x,fromPnt.y));

        if (target == null || Mathf.Abs(vec.x + vec.y) < SQ)
        {
            //Debug.Log(" !!! FillDirectionsArray NOTARGET !!!");
        }

        if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
        {
            if (vec.x > 0)
            {
                tryDir[0] = aDir[0];
                tryDir[2] = aDir[2];
            }
            else
            {
                tryDir[0] = aDir[2];
                tryDir[2] = aDir[0];
            }

            if (vec.y > 0)
            {
                tryDir[1] = aDir[1];
                tryDir[3] = aDir[3];
            }

            else
            {
                tryDir[1] = aDir[3];
                tryDir[3] = aDir[1];
            }
               
        }
        else
        {
            if (vec.y > 0)
            {
                tryDir[0] = aDir[1];
                tryDir[2] = aDir[3];
            }
            else
            {
                tryDir[0] = aDir[3];
                tryDir[2] = aDir[1];
            }

            if (vec.x > 0)
            {
                tryDir[1] = aDir[0];
                tryDir[3] = aDir[2];
            }

            else
            {
                tryDir[1] = aDir[2];
                tryDir[3] = aDir[0];
            }
        }

         //Debug.Log("NEW tryDir0 " + tryDir[0]);

        //dir = tryDir[0];
        //target = obj;
        //nextPoint.x += dir.x * 24*UnityEngine.Random.Range(1,5);
        //nextPoint.y += dir.y * 24*UnityEngine.Random.Range(1,5);

        //moveX = Mathf.Abs(dir.x) > 0;

        //faseNext = F_MOVE;
    }


    void chooseNewPoint()
    {
        SetToSqCenter();
        
        Vector2[] scr = new Vector2[4];
        
        scr[0]=new Vector2(1,0);
        scr[1]=new Vector2(0,1); 
        scr[2]=new Vector2(-1,0);
        scr[3]=new Vector2(0,-1);

        int i = 0;
        
        do
        {
            i = Mathf.FloorToInt(UnityEngine.Random.Range(0.0f, 3.9f));

        } while (dir==scr[i]);

        dir = scr[i];

        nextPoint.x += dir.x * 24*UnityEngine.Random.Range(1,5);
        nextPoint.y += dir.y * 24*UnityEngine.Random.Range(1,5);
               
    }

    void SetToSqCenter()
    {
        //RB.velocity = new Vector2();
        
        transform.position = new Vector2(Mathf.Floor(transform.position.x / 24.0f) * 24.0f + 12.0f,Mathf.Floor(transform.position.y / 24.0f) * 24.0f + 12.0f);
        
    }

    bool InSqCenter()
    {
        return (Vector2.Distance(transform.position, GetClosestSqCenter()) < 1.0f);

    }

    Vector2 GetClosestSqCenter()
    {
        return (new Vector2(Mathf.Floor(transform.position.x / 24.0f) * 24.0f + 12.0f,Mathf.Floor(transform.position.y / 24.0f) * 24.0f + 12.0f));
    }

    Vector2 GetClosestSqCenter(Vector2 pos)
    {
        return (new Vector2(Mathf.Floor(pos.x / 24.0f) * 24.0f + 12.0f, Mathf.Floor(pos.y / 24.0f) * 24.0f + 12.0f));
    }

    bool CheckCrossSQ(Vector2 pos1 , Vector2 pos2)
    {
        if (Mathf.FloorToInt(pos1.x / SQ) != Mathf.FloorToInt(pos2.x / SQ) ||
            Mathf.FloorToInt(pos1.y / SQ) != Mathf.FloorToInt(pos2.y / SQ))
            return true;

        return false;
    }
    
    bool CheckCrossSQ(float pos1 , float pos2)
    {
        if (Mathf.FloorToInt(pos1 / SQ) != Mathf.FloorToInt(pos2 / SQ))
            return true;

        return false;
    }

    /*void CheckMouseClick()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            //Debug.Log("sel");
            
               
            if (menuC2D==Physics2D.OverlapPoint(Input.mousePosition, LayerMask.GetMask("UI")))
            {
                //Debug.Log("Clicked to menu "+Input.mousePosition);
            }
            else
            if (C2D==Physics2D.OverlapPoint(cam.ScreenToWorldPoint(Input.mousePosition), LayerMask.GetMask("units")))  
            {
                //selectorObj();
            }
            else
            {
                UnSelect();
            }
                

        }
    }*/

    public void Select()
    {
        if (!selected)
        {
            selected = true;
            selectorObj.SetActive(selected);
        }
            
    }

    public void UnSelect()
    {
        selected = false;
        selectorObj.SetActive(selected);
    }

    public void SetTask(int _task)
    {
        nextTask = _task;
       // //Debug.Log("my task is "+task);
    }

    bool ChooseTarget()
    {
        bool ret = false;

        switch (nextTask)
        {
            case G.CAPTURE_BASES:
                ret=ChooseBaseForCapture();
                break;
            case G.DESTROY_ENEMIES:
                ret = ChooseEnemyForDestroy();
                break;           
            case G.CAPTURE_FACTORIES:
                ret =ChooseFactoryForCapture();
                break;          
            
        }

        return ret;
    }

    bool ChooseBaseForCapture()
    {
        return false;
    }
    bool ChooseEnemyForDestroy()
    {
        return false;
    }
    
    bool ChooseFactoryForCapture()
    {
        float dist = 99999;
        building nearestObj=null;
        target = null;
        
        GameObject[] aObj = GameObject.FindGameObjectsWithTag("Factory");
        
        foreach (GameObject obj in aObj)
        {
            if (obj.GetComponent<building>().mySide != mySide)
            if (Vector2.Distance(transform.position, obj.transform.position)* obj.GetComponent<building>().GetAimerCount(mySide) < dist)
            {
                    dist = Vector2.Distance(transform.position, obj.transform.position);
                    nearestObj = obj.GetComponent<building>();     
            }
        }
       
        //Debug.Log("ChooseFactoryForCapture: "+nearestObj);
        if (nearestObj!=null)
        {
            target = nearestObj;
            targetPoint = target.transform.position;

            target.AddAimer(mySide);

            return true;
        }
        else
        {
           StopNow();
        }

        return false;
    }

    void StopNow()
    {
        nextPoint = GetClosestSqCenter();
        faseNext = F_IDLE;
    }

    float CalcSpeed()
    {
        if (Vector2.Distance(transform.position, prevPoint)<Vector2.Distance(prevPoint,nextPoint)*0.4)
        {
            if (Vector2.Distance(transform.position, prevPoint) < accelDist)
            {
                return speedMax*(Vector2.Distance(transform.position, prevPoint)) / accelDist+1.0f;
            } 
        }
        else
        {
            if (Vector2.Distance(transform.position, nextPoint) < accelDist)
            {
                return speedMax*(Vector2.Distance(transform.position, nextPoint)) / accelDist+1.0f;
            }           
            
        }

        return speedMax;
    }
    float CalcSpeed(Vector2 from, Vector2 to)
    {
        if (Vector2.Distance(from, to) < accelDist)
            return speedMax*(Vector2.Distance(from, to)) / accelDist+1.0f;

        return speedMax;
    }
    Vector2 Dir90ToPoint(Vector2 vec)
    {
        Vector2 ret = new Vector2();
        
        if (Mathf.Abs(vec.x - transform.position.x) > Mathf.Abs(vec.y - transform.position.y))
        {
            if (vec.x > transform.position.x)
            {
                ret.x = 1;
            }
            else
            {
                ret.x = -1;
            }
        }
        else
        {
            if (vec.y > transform.position.y)
            {
                ret.y = 1;
            }
            else
            {
                ret.y = -1;
            }
        }

        return ret;
    }
    Vector2 Dir90FromToPoint(Vector2 from, Vector2 to)
    {
        Vector2 ret = new Vector2();
        
        if (Mathf.Abs(to.x - from.x) > Mathf.Abs(to.y - from.y))
        {
            if (to.x > from.x)
            {
                ret.x = 1;
            }
            else
            {
                ret.x = -1;
            }
        }
        else
        {
            if (to.y > from.y)
            {
                ret.y = 1;
            }
            else
            {
                ret.y = -1;
            }
        }

        return ret;
    }
    Vector2 V3V2(Vector3 vec3)
    {
        return new Vector2(vec3.x,vec3.y);
    }
    
    Vector3 V2V3(Vector2 vec3)
    {
        return new Vector3(vec3.x,vec3.y,0.0f);
    }
    
    float DirToAngle(Vector2 dir)
    {
        if (dir.y == 0)
        {
            if (dir.x > 0.5)
                return 0;
            if (dir.x < 0.5)
                return 180;
        }
        else
        if (dir.x == 0)
        {
            if (dir.y > 0.5)
                return 90;
            if (dir.y < 0.5)
                return 270;
        }

        return 0;
    }

    public void SetMySide(int side)
    {
        mySide = side;
        
        RecolorAll(G.GetSideColor(mySide));
        /*
        if (side == G.PLAYER_SIDE)
        {
            RecolorAll(Color.red);
        }
        else
        if (side == G.WarSides.ENEMY)
        {
            RecolorAll(Color.blue);
        }        */
    }

    void RecolorAll(Color col)
    {
        //chassis.color = col;
        chassis.GetComponent<passiveEquipPrefab>().spriteForColoring.color = col;
        gun1.GetComponent<wpnPrefab>().spriteForColoring.color = col;
        
    }

    void HeadControl()
    {
        if (headTargetSearchCounter > 0)
        {
            headTargetSearchCounter -= Time.fixedDeltaTime;            
        }        
        else
        {        
            headTarget=HeadFindTarget();
            headHaveTarget = (headTarget != null);
            
            if (!headHaveTarget)
            {
                headTargetSearchCounter = 0.25f;
            }
            else
            {
                headTargetSearchCounter = 1.0f;

                myHead.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, headTarget.transform.position - myHead.transform.position));
                gun1Anim.SetBool("firing", true);
            }
        }

        if (headTarget != null)
        {
            myHead.transform.eulerAngles =
                new Vector3(0, 0,
                    Vector2.SignedAngle(Vector2.right, headTarget.transform.position - myHead.transform.position));
            gun1Anim.SetBool("firing", true);
        }
        else        
        {
            myHead.transform.eulerAngles = myBase.transform.eulerAngles;
                //new Vector3(0, 0,Vector2.SignedAngle(Vector2.right,myBase.transform.eulerAngles));
            headHaveTarget = false;
            headTargetSearchCounter = -1;
            gun1Anim.SetBool("firing", false);

           // if (gun1Anim.sto)
        }
        
    }

    GameObject HeadFindTarget()
    {
        float dist = fireRange;
        GameObject nearestObj=null;
        
        
        GameObject[] aObj = GameObject.FindGameObjectsWithTag("Tank");
        
        foreach (GameObject obj in aObj)
        {
            if (obj.GetComponent<tank>().mySide != mySide)
                if (Vector2.Distance(transform.position, obj.transform.position) < dist)
                    if (!Physics2D.Linecast(transform.position, obj.transform.position, 1 << 10))
                {
                    
                    //Debug
                        dist = Vector2.Distance(transform.position, obj.transform.position);
                    nearestObj = obj;//.GetComponent<tank>();                   
                

                }
        }

        return nearestObj;
    }
   
    public bool Selected()
    {
        return selected;
    }

    public void SetDamage(float damage)
    {      

        health -= damage;
        //Debug.Log("Damage " + damage + " / Health " + health);

        if (health>0) return;

        health = 0.0f;
       
        GameObject o=Instantiate(explo, effectsGroup);
        o.transform.position = transform.position;

        if (target != null)
            target.GetComponent<building>().RemoveAimer(mySide);

       Destroy(gameObject, 1 * Time.deltaTime);
        destroyed = true;


        Destroy(selectorObj.gameObject);

        if (fogMask != null)
        {
            fogMask.transform.DOScale(0.001f, 3);
            Destroy(fogMask.gameObject, 3);
        }

        SetUnblockedAll();

    }

    public bool GetDestroyed()
    {
        return destroyed;
    }
/*
    private Vector2 GetBetterNextPointFrom(Vector2 point)
    {
        //Vector2 retPnt = new Vector2();

        return ChooseNextPoint(point);


        //return retPnt;
    }
*/
    private void AddWayPointsTo(Vector2 point)
    {
        Debug.Log("AddWayPointsTo  PP: "+prevPoint+" / CP: "+currentPoint+" / NP: "+nextPoint);
        Debug.Log("AddWayPointsTo   Ang1=" + Vector2.Angle(currentPoint-prevPoint,nextPoint-currentPoint)+" Ang2="+Vector2.Angle(nextPoint-currentPoint,new Vector2()));

        if ((currentPoint-prevPoint).normalized==(nextPoint-currentPoint).normalized)
        {
            wayPoints.Add(new Vector2((nextPoint.x+currentPoint.x)*0.5f,(nextPoint.y+currentPoint.y)*0.5f));

        }
        else
        if (Vector2.Angle(currentPoint-prevPoint,nextPoint-currentPoint)==90)// turn left
        {
            MakePathTurn90Deg((currentPoint+prevPoint)*0.5f,(nextPoint-currentPoint).normalized);
        }
        else
        if (Vector2.Angle(currentPoint-prevPoint,nextPoint-currentPoint)==-90)// turn right
        {
            MakePathTurn90Deg((currentPoint+prevPoint)*0.5f,(nextPoint-currentPoint).normalized);
        }
/*
        if (Mathf.Abs(transform.position.x-point.x)<SQ*0.1 || Mathf.Abs(transform.position.y-point.y)<SQ*0.1)
        {
            wayPoints.Add(point);
        }
        else
        if (Mathf.Abs(transform.position.x-point.x)<SQ*0.1 || Mathf.Abs(transform.position.y-point.y)<SQ*0.1)
        {
            wayPoints.Add(point);
        }        
*/
    }

    void CrossPointAction()
    {
        //choose new direction
        Vector2 tryPoint = ChooseNextPoint(nextPoint);

        //build way to this dir
    }

    void MakePathTurn90Deg(Vector2 startPnt, Vector2 dir)
    {
        Debug.Log("MakePathTurn90Deg  from: "+startPnt+" / dir: "+dir);

        float sx=GetSign(nextPoint.x,currentPoint.x);
        float sy=GetSign(nextPoint.y,currentPoint.y);

        float x0=Mathf.RoundToInt(startPnt.x/SQ/2)*SQ*2;
        float y0=Mathf.RoundToInt(startPnt.y/SQ/2)*SQ*2;

        sx*=0.5f*SQ;
        sy*=0.5f*SQ;

        Vector2 a= (currentPoint-prevPoint).normalized*SQ*0.5f;
        Vector2 b= (nextPoint-currentPoint).normalized*SQ*0.5f;
        Vector2 o= (currentPoint+prevPoint)*0.5f;

        wayPoints.Add(o+0.4f*a+0.1f*b);
        wayPoints.Add(o+0.7f*a+0.3f*b);
        wayPoints.Add(o+0.9f*a+0.6f*b);
        wayPoints.Add(o+1.0f*a+1.01f*b);

/*
        if (moveX)
        {
            wayPoints.Add(new Vector2(0.4f*a+x0, 0.1f*b+y0));
            wayPoints.Add(new Vector2(0.7f*sx+x0, 0.3f*sy+y0));
            wayPoints.Add(new Vector2(0.9f*sx+x0, 0.6f*sy+y0));
            wayPoints.Add(new Vector2(1f*sx+x0, 1f*sy+y0));
        }
        else
        {
            wayPoints.Add(new Vector2(0.1f*sx+x0, 0.4f*sy+y0));
            wayPoints.Add(new Vector2(0.3f*sx+x0, 0.7f*sy+y0));
            wayPoints.Add(new Vector2(0.6f*sx+x0, 0.9f*sy+y0));
            wayPoints.Add(new Vector2(1f*sx+x0, 1f*sy+y0));
            
        }   */   

        

    }

    int GetSign(float b, float a)
    {
        if (Mathf.Abs(b-a)<0.01f)
            return 0;
        if (b-a>0) return 1;

        return -1;
    }

    bool HereLineToTarget(Vector2 pnt)
    {
        if (Mathf.Abs(targetPoint.x-pnt.x)<1f || Mathf.Abs(targetPoint.y-pnt.y)<1f)
        return true;//new Vector2(GetSign(targetPoint.x,pnt.x), GetSign(targetPoint.x,pnt.x));

        return false;
    }

    void SetBlocked (Vector2 pos)
    {
        moveHelper.SetBlocked(pos);
        aBlocked.Add(pos);                
    }
    void SetUnblocked (Vector2 pos)
    {
        moveHelper.SetEmpty(pos);
        aBlocked.Remove(pos);                        
    }
    void SetUnblockedAll ()
    {
        for (int i = 0; i < aBlocked.Count; i++)
        {
            moveHelper.SetEmpty(aBlocked[i]);

        }
        aBlocked.Clear();  
    }

    Vector2 GetBestNextPointFrom(Vector2 point)
    {
        
        if (dirN>0)  
        {
            if (moveHelper.SqIsEmpty(point+tryDir[dirN-1]*SQ)) 
            {
                dirN=dirN-1;
                return point+tryDir[dirN]*SQ; 
            }
                 
            //return point+tryDir[dirN]*SQ; 
        }
        
        for (int i = dirN; i < tryDir.Length; i++)
        {
            if (moveHelper.SqIsEmpty(point+tryDir[i]*SQ)) 
            {
                dirN=i;
                return point+tryDir[i]*SQ;  
            }          
        }

        return currentPoint;
    }

    void CheckMeInFog()
    {
        /*
        if (mySide==G.PLAYER_SIDE) return;

        float dist = fireRange;
        GameObject[] aObj = GameObject.FindGameObjectsWithTag("Tank");
*/

        /*
        foreach (GameObject obj in aObj)
        {
           canvasGroup.alpha=0.0f;

            if (obj.GetComponent<tank>().mySide != mySide)
                if (Vector2.Distance(transform.position, obj.transform.position) < sightRange)
                canvasGroup.alpha=1.0f;
        }*/
    }
}
