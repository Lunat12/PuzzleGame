using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    [System.Serializable]
    public enum butons { BUTTON, COUNTERBUTTON, KEYBUTTON, CODE};
    public butons buttonType;
    public GameObject PointA, PointB;
    public LayerMask GameLayer;
    private Vector3 scaleChange, positionChange;
    public bool hit;
    private float counter;
    public float TimeLeft;
    public GameObject timePanel;
    public List<int> code;
    public List<int> finalcode;
    private bool correct;
    public InputField fieldCode;
    




    // Start is called before the first frame update
    void Start()
    {
        counter = 10f;
        hit = false;
        timePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        scaleChange.y = -0.35f;
        positionChange.y = -0.2f;
        Collider2D detect= Physics2D.OverlapArea(PointA.transform.position, PointB.transform.position, GameLayer);
      


        switch (buttonType)
        {
            case butons.BUTTON:
                if (detect != null && hit == false)
                {
                    hit = true;
                    gameObject.transform.localScale += scaleChange;
                    gameObject.transform.position += positionChange;

                }
                else if (detect == null && hit == true)
                {
                    hit = false;
                    gameObject.transform.localScale -= scaleChange;
                    gameObject.transform.position -= positionChange;
                }

                break;
            case butons.COUNTERBUTTON:
                if (detect != null && hit == false)
                {
                    hit = true;
                    gameObject.transform.localScale += scaleChange;
                    gameObject.transform.position += positionChange;
                    

                }
                else if (detect == null && hit == true)
                {
                    timePanel.SetActive(true);
                    counter -= Time.deltaTime;
                    TimeLeft = Mathf.Floor(counter);
                    timePanel.transform.Find("Text").GetComponent<Text>().text = TimeLeft.ToString();

                    if (counter<=0f)
                    {
                        hit = false;
                        gameObject.transform.localScale -= scaleChange;
                        gameObject.transform.position -= positionChange;
                        counter = 10f;
                        timePanel.SetActive(false);
                    }
                    
                }
                else if(detect != null && hit == true)
                {
                    counter = 10f;
                    timePanel.SetActive(false);
                }
                break;
            case butons.KEYBUTTON:
                if(detect != null && hit == false)
                {
                    if (detect.gameObject.GetComponent<PlayerControl>() != null )
                    {
                        if(detect.gameObject.GetComponent<PlayerControl>().hasKey >= 1)
                        {
                            if(Input.GetKeyDown(KeyCode.LeftShift))
                            {
                                hit = true;

                                detect.GetComponent<PlayerControl>().hasKey -= 1;
                            }
                          
                        }
                        

                    }
                     else if( detect.gameObject.GetComponent<CloneControl>() != null)
                    {

                        if (detect.gameObject.GetComponent<CloneControl>().hasKey >= 1)
                        {
                            if (Input.GetKeyDown(KeyCode.LeftShift))
                            {
                                hit = true;

                                GameObject.Find("Player").GetComponent<PlayerControl>().hasKey -= 1;
                            }
                                
                        }
                    }

                }
               
                break;
            case butons.CODE:
                if(detect != null && hit == false)
                {
                    timePanel.SetActive(true);
                    Time.timeScale = 0;
                 
                    
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Space))
                    {
                        timePanel.SetActive(false);
                        
                        Time.timeScale = 1;
                    }
                }
                
                else
                {
                    timePanel.SetActive(false);
                    
                    Time.timeScale = 1;
                }
             if(correct == true)
                {
                    hit = true;
                    
                }
              
                break;
            default:
                break;
        }
    }

    public void addCode(int _value)
    {
        finalcode.Add(_value);
        PrintCode();
    }
    public void delete()
    {
        finalcode.RemoveAt(finalcode.Count - 1);
        PrintCode();
    }

    private void PrintCode()
    {
        string stringCode = "";
        for (int i = 0; i < finalcode.Count; i++)
        {
            stringCode += finalcode[i].ToString();
        }
        fieldCode.text = stringCode;
    }

    public void SetCode()
    {
        correct = correctCode(finalcode);
    }

    private bool correctCode(List<int> _code)
    {
        for (int i = 0; i < _code.Count; i++)
        {
            if (_code[i] != code[i]) return false;
        }
        return true;
    }
    
}
