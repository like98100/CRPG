using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCreator : MonoBehaviour
{
    public static float CLOUD_WIDTH = 40.0f;  // 구름 이미지의 길이
    public static float CLOUD_HEIGHT = 6.0f; // 구름 이미지의 높이
    public static int CLOUD_NUM_IN_SCREEN = 3; // 화면 안에 들어갈 구름 스프라이트 개수

    private struct Cloud
    {   // 구름 정보
        public bool isCreated;
        public Vector3 position;
    }

    private Cloud lastCloud;
    public GameObject cloudPrefab;

    // Start is called before the first frame update
    void Start()
    {
        this.lastCloud.isCreated = false;   
    }

    private void CreateCloud()
    {
        Vector3 cloudPosition;
        if(!this.lastCloud.isCreated)
        {// 구름이 생성되지 않았을 때
            cloudPosition = new Vector3(0f, 0.33f, 0f);
            cloudPosition.x -= CLOUD_WIDTH * ((float)CLOUD_NUM_IN_SCREEN / 2.0f);
        }
        else
        {
            cloudPosition = this.lastCloud.position;
        }

        cloudPosition.x += CLOUD_WIDTH;

        // 구름 생성
        GameObject ob = GameObject.Instantiate(this.cloudPrefab) as GameObject;
        ob.transform.position = cloudPosition;

        this.lastCloud.position = cloudPosition;
        this.lastCloud.isCreated = true;

    }

    public bool isDelete(GameObject ob)
    {
        bool result = false;

        float leftLimit = new Vector3(0f, 1f, 0f).x - CLOUD_WIDTH * ((float)CLOUD_NUM_IN_SCREEN / 2.0f);

        if (ob.transform.position.x < leftLimit) result = true;

        return result;
    }

    // Update is called once per frame
    void Update()
    {
        float generatePos_x = 0f;
        generatePos_x += CLOUD_WIDTH * ((float)CLOUD_NUM_IN_SCREEN + 1) / 2.0f;
        while(this.lastCloud.position.x < generatePos_x)
        {
            this.CreateCloud();
        }
        
    }
}
