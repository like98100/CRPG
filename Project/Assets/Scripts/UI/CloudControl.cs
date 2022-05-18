using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudControl : MonoBehaviour
{
    public CloudCreator cloudCreator = null;

    // Start is called before the first frame update
    void Start()
    {
        cloudCreator = GameObject.Find("CloudCreator").GetComponent<CloudCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position -= new Vector3(0.5f * Time.deltaTime, 0.0f, 0f);
        if(this.cloudCreator.isDelete(this.gameObject))
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
