using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    GameObject ParentObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ParentObj = GameObject.FindAnyObjectByType<CameraProtector>().gameObject;
    }

    public void DestroyAll()
    {
        SceneManager.LoadScene("Level_1");
        Destroy(ParentObj);
    }
}
