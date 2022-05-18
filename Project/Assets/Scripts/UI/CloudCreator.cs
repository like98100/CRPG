using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCreator : MonoBehaviour
{
    public static float CLOUD_WIDTH = 40.0f;  // ���� �̹����� ����
    public static float CLOUD_HEIGHT = 6.0f; // ���� �̹����� ����
    public static int CLOUD_NUM_IN_SCREEN = 3; // ȭ�� �ȿ� �� ���� ��������Ʈ ����

    private struct Cloud
    {   // ���� ����
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
        {// ������ �������� �ʾ��� ��
            cloudPosition = new Vector3(0f, 0.33f, 0f);
            cloudPosition.x -= CLOUD_WIDTH * ((float)CLOUD_NUM_IN_SCREEN / 2.0f);
        }
        else
        {
            cloudPosition = this.lastCloud.position;
        }

        cloudPosition.x += CLOUD_WIDTH;

        // ���� ����
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
