using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemsManager : MonoBehaviour
{
    public enum SwitchSystem { DOOR, PLATFORM, TUBE, OTHER }

    [System.Serializable]
    public class switchProperties
    {
        public SwitchSystem type;
        public GameObject Switch;
        public GameObject CurrentSystem;
        public bool Open;
        public bool OpenForever;
        public List<GameObject> Points;

    }
    public List<switchProperties> Action;


    private Vector3 PositionChange;
    // private List<GameObject> barriers = new List<GameObject>();
    private GameObject inWay;

    private int currentPoint = 0;
    bool goP1;

   // Start is called before the first frame update
   void Start()
    {

        //for (int i = 0; i < Action.Count; i++)
        //{
        //    GameObject _barrier = Action[i].CurrentSystem.transform.Find("barrier").gameObject;
        //    if(_barrier != null )
        //    {
        //        barriers.Add(_barrier);
        //    }
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {



        workPls();
    }
    void workPls()
    {
        for (int i = 0; i < Action.Count; i++)
        {
            if (Action[i].Switch.GetComponent<ButtonControl>().hit == true)
            {

                switch (Action[i].type)
                {
                    case SwitchSystem.DOOR:

                        if (Action[i].Open == false)
                        {
                            PositionChange.y = 3f;
                            Action[i].Open = true;
                            Action[i].CurrentSystem.transform.position += PositionChange;
                        }


                        break;
                    case SwitchSystem.PLATFORM:


                        GameObject Point2 = Action[i].CurrentSystem.transform.Find("P2").GetComponent<Transform>().gameObject;

                        GameObject Point1 = Action[i].CurrentSystem.transform.Find("P1").GetComponent<Transform>().gameObject;
                        GameObject CurrentPlatform = Action[i].CurrentSystem.transform.Find("Platform").GetComponent<Transform>().gameObject;


                        if (Action[i].Open == false)
                        {

                            CurrentPlatform.transform.position = Vector2.MoveTowards(CurrentPlatform.transform.position, Point2.transform.position, 2f * Time.deltaTime);
                            float dist = Vector2.Distance(CurrentPlatform.transform.position, Point2.transform.position);
                          
                            if (dist < 0.1f)
                            {
                                Action[i].Open = true;
                             


                            }
                           

                        }
                        else
                        {

                            CurrentPlatform.transform.position = Vector2.MoveTowards(CurrentPlatform.transform.position, Point1.transform.position, 2f * Time.deltaTime);
                            float dist = Vector2.Distance(CurrentPlatform.transform.position, Point1.transform.position);

                            if (dist < 0.1f)
                            {
                                Action[i].Open = false;
                               


                            }

                        }
                        break;
                    case SwitchSystem.TUBE:

                        Collider2D detect = Physics2D.OverlapCircle(Action[i].CurrentSystem.transform.position, 0.5f);


                        if (detect != null || inWay != null) 
                        {

                           
                            if(inWay == null)
                                inWay = detect.gameObject;

                            inWay.transform.position = Vector2.MoveTowards(inWay.transform.position, Action[i].Points[currentPoint].transform.position, 5f * Time.deltaTime);
                            float dist = Vector2.Distance(inWay.transform.position, Action[i].Points[currentPoint].transform.position);
                         

                            if (inWay.GetComponent<Rigidbody2D>() != null)
                            {
                                inWay.GetComponent<Rigidbody2D>().isKinematic = true;
                                inWay.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                            }
                            if (dist < 0.1f)
                            {
                                currentPoint++;

                            }
                            if (currentPoint >= Action[i].Points.Count)
                            {
                                inWay.GetComponent<Rigidbody2D>().isKinematic = false;
                                inWay.transform.position = inWay.transform.position;
                                inWay = null;
                                currentPoint = 0;
                                
                            }


                        }


                        break;
                    case SwitchSystem.OTHER:
                        break;
                    default:
                        break;
                }

            }
            else if (Action[i].Switch.GetComponent<ButtonControl>().hit == false && Action[i].Open == true)
            {
                switch (Action[i].type)
                {
                    case SwitchSystem.DOOR:
                        if (Action[i].OpenForever == false)
                        {
                            Action[i].Open = false;
                            Action[i].CurrentSystem.transform.position -= PositionChange;
                        }

                        break;
                    case SwitchSystem.PLATFORM:
                        GameObject Point1 = Action[i].CurrentSystem.transform.Find("P1").GetComponent<Transform>().gameObject;
                        GameObject CurrentPlatform = Action[i].CurrentSystem.transform.Find("Platform").GetComponent<Transform>().gameObject;
                        CurrentPlatform.transform.position = Vector2.MoveTowards(CurrentPlatform.transform.position, Point1.transform.position, 2f * Time.deltaTime);

                        float dist = Vector2.Distance(CurrentPlatform.transform.position, Point1.transform.position);
                        if (dist < 0.1f)
                        {
                            Action[i].Open = false;
                            
                        }



                        break;
                    case SwitchSystem.TUBE:
                        break;
                    case SwitchSystem.OTHER:
                        break;
                    default:
                        break;
                }

            }
            else if (Action[i].Switch.GetComponent<ButtonControl>().hit == false && Action[i].Open == false)
            {
                switch (Action[i].type)
                {
                    case SwitchSystem.DOOR:

                        break;
                    case SwitchSystem.PLATFORM:
                        GameObject Point1 = Action[i].CurrentSystem.transform.Find("P1").GetComponent<Transform>().gameObject;
                        GameObject CurrentPlatform = Action[i].CurrentSystem.transform.Find("Platform").GetComponent<Transform>().gameObject;
                        CurrentPlatform.transform.position = Vector2.MoveTowards(CurrentPlatform.transform.position, Point1.transform.position, 2f * Time.deltaTime);

                        float dist = Vector2.Distance(CurrentPlatform.transform.position, Point1.transform.position);
                        if (dist < 0.1f)
                        {
                            Action[i].Open = false;
                          
                        }



                        break;
                    case SwitchSystem.TUBE:
                        break;
                    case SwitchSystem.OTHER:
                        break;
                    default:
                        break;
                }
            }
         

                       



        }

    }




    public void DoorStaysOpen(GameObject _currentDoor)
    {
      
        for (int i = 0; i < Action.Count; i++)
        {
            if (_currentDoor == Action[i].CurrentSystem)
            {
                Action[i].Open = true;
                Action[i].OpenForever = true;
            }
        }

    }


}
