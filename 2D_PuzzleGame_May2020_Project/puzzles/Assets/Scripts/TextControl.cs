using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextControl : MonoBehaviour
{
    [System.Serializable]
    public class PCText
    {
        public GameObject CurrentPC;
        [TextArea]
        public string PCtext;
    }
    public List<PCText> Info;
    public Text screen;
    public LayerMask GameLayer;
    public GameObject TextPanel;
    // Start is called before the first frame update
    void Start()
    {
        TextPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        bool isPlayerOnPc = false;
        for (int i = 0; i < Info.Count; i++)
        {
            Collider2D detect = Physics2D.OverlapCircle(Info[i].CurrentPC.transform.position, 0.5f, GameLayer);



            if (detect != null)
            {
                isPlayerOnPc = true;

                screen.text = Info[i].PCtext;
               
            }
        }

        if (isPlayerOnPc == false)
        {
            TextPanel.SetActive(false);
        }
        if (isPlayerOnPc == true && Input.GetKeyDown(KeyCode.LeftShift))
        {
            TextPanel.SetActive(true);
        }
        
        


    }
}
