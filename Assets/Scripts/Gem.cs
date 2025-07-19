using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 VarianPosition = GameObject.Find("Varian").transform.position;
        if (Vector2.Distance(transform.position, VarianPosition) <= 5)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, VarianPosition, 1f * Time.deltaTime);
            if (Vector2.Distance(transform.position, VarianPosition) <= 0.5f)
            {
                GameObject.Find("Varian").SendMessage("AddBlueGem");
                Destroy(gameObject);
            }
        }
    }

}
