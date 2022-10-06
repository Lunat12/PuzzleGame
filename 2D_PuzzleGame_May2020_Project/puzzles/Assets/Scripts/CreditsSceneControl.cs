using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsSceneControl : MonoBehaviour
{

    private float counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        if(counter >=5f)
        {
            SceneManager.LoadScene(0);
        }
    }
}
