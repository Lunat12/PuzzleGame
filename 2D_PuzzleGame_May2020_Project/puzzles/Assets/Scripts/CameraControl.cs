using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
      public GameObject target, target2;
    public GameObject limitsRU, limitsLD;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.parent == null)
        {
            if (target.GetComponent<PlayerControl>().cloned == false)
            {
                transform.position = Vector2.Lerp(transform.position, target.transform.position, 5 * Time.deltaTime);
            }
               
            if (target.GetComponent<PlayerControl>().cloned == true)
            {

                transform.position = Vector2.Lerp(transform.position, target2.transform.position, 5 * Time.deltaTime);

            }
        }

        
    }

    private void Update()
    {
        if(transform.parent != null)
        {
            if (target.GetComponent<PlayerControl>().cloned ==false)
            {
                transform.position = Vector2.Lerp(transform.position, target.transform.position, 5 * Time.deltaTime);
            }

            if (target.GetComponent<PlayerControl>().cloned == true)
            {

                transform.position = Vector2.Lerp(transform.position, target2.transform.position, 5 * Time.deltaTime);

            }
        }

        if (transform.position.x >= limitsRU.transform.position.x)
        {
            transform.position = new Vector2(limitsRU.transform.position.x, transform.position.y);
        }
        if(transform.position.y >= limitsRU.transform.position.y)
        {
            transform.position = new Vector2(transform.position.x, limitsRU.transform.position.y);
        }
        if (transform.position.x <= limitsLD.transform.position.x)
        {
            transform.position = new Vector2(limitsLD.transform.position.x, transform.position.y);
        }
        if(transform.position.y <= limitsRU.transform.position.y)
        {
            transform.position = new Vector2(transform.position.x, limitsLD.transform.position.y);
        }


      
    }

    
}
