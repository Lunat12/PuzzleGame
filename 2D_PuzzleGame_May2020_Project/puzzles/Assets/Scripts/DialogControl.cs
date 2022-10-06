using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogControl : MonoBehaviour
{
    
    public Text DialogText;
   public GameObject dialogParent;
    [TextArea]
    public List<string> currentText;
    public string finalText;
    private int currentLetter;
    private int currentDialog;
    private float letterCount, DialogCount;
    public float speedText;
    private bool startShowLetter;
    private bool PrintingText, firstTime;
    
        
      
    
  

    // Start is called before the first frame update
    void Start()
    {
        dialogParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(startShowLetter == true)
        {
            letterCount += Time.deltaTime;
            if(letterCount >= speedText)
            {
                ShowLetter();
                letterCount = 0;
            }
            if(PrintingText==true)
            {
                speedText -= 0.4f;
            }
           
        }
        else
        {
            if (firstTime == false)
            {

                DialogCount += Time.deltaTime;
                if (PrintingText == true && DialogCount >= 5f)
                {
                    for (int i = 0; i < currentText.Count; i++)
                    {
                        if (i == currentDialog)
                        {
                            finalText = currentText[i];
                            Dialog();
                            DialogCount = 0;
                            return;
                        }
                    }
                    gameObject.SetActive(false);
                    dialogParent.SetActive(false);
                    PrintingText = false;

                    
                    
                   
                    
                        GameObject.Find("Player").GetComponent<PlayerControl>().speed = 5;
                    

                }
            }
            else
            {
                if (PrintingText == true)
                {
                    for (int i = 0; i < currentText.Count; i++)
                    {
                        if (i == currentDialog)
                        {
                            finalText = currentText[i];
                            Dialog();
                            DialogCount = 0;
                            firstTime = false;
                            return;
                        }
                    }
                }
            }

        }

        if(dialogParent.activeSelf==true)
        {
            
           
          
            
                GameObject.Find("Player").GetComponent<PlayerControl>().speed = 0;
            
        }

    }

    private void ShowLetter()
    {
        string tempShow = "";
        tempShow = finalText.Substring(0, currentLetter);
        print(tempShow);
        DialogText.text = tempShow;
        currentLetter++;
        if(currentLetter > finalText.Length)
        {
            startShowLetter = false;
        }
    }

    private void Dialog()
    {
        currentDialog++;
        currentLetter = 0;
        speedText -= 0.4f;
        startShowLetter = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            dialogParent.SetActive(true);
            DialogText.text = null;
            PrintingText = true;
            firstTime = true;
        }
       
    }
}
