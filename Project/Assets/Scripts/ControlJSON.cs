using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

public class ControlJSON : MonoBehaviour
{
    //JSON 입출력 전용 cs 파일이므로 start 함수와 update 함수는 사용하지 않는다.
    // Start is called before the first frame update
    //void Start()
    //{
        //FileInfo fileInfo = new FileInfo(Application.dataPath + "/Test.json");
        //if (!fileInfo.Exists)
        //{
            
        //    JTestClass jtc = new JTestClass(true);
        //    string jsonData = ObjectToJson(jtc);
        //    Debug.Log(jsonData);

        //    CreateJsonFile(Application.dataPath, "Test", jsonData);
        //}
        //else Debug.Log("The file already exists.");
    //}
    
    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public string ObjectToJson(object obj) //오브젝트를 json 데이터로
    {
        return JsonUtility.ToJson(obj);
    }

    public T JsonToObject<T>(string jsonData) //json 데이터를 원하는 타입의 객체로
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    //public T JsonToObjectOverwrite<T>(string jsonData, object obj) //json 데이터를 원하는 타입의 객체로, overwrite 버전
    //{
    //    return JsonUtility.FromJsonOverwrite<T>(jsonData, obj);
    //}

    public void CreateJsonFile(string createPath, string fileName, string jsonData)    // json 파일이 없을 때 최초로 생성할 함수
    {
        FileStream fileStream = new FileStream(string.Format("{0}/Data/{1}.json", createPath, fileName), FileMode.CreateNew);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();

        //Debug.Log("Success to Create JSON");
    }

    public void OverWriteJsonFile(string createPath, string fileName, string jsonData) // 존재하는 json 파일 수정을 위한 함수
    {
        FileStream fileStream = new FileStream(string.Format("{0}/Data/{1}.json", createPath, fileName), FileMode.Truncate);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();

        //Debug.Log("Success to OverWrite JSON");
    }

    public string LoadJsonFile<T>(string loadPath, string fileName) // json 파일에 저장되어 있는 정보를 가져오는 함수
    {
        FileStream fileStream = new FileStream(string.Format("{0}/Data/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();

        string jsonData = Encoding.UTF8.GetString(data);

        //Debug.Log("Success to Bring JSON");

        return jsonData;
    }

    //public T LoadJsonFile<T>(string loadPath, string fileName) // json 파일에 저장되어 있는 정보를 가져오는 함수
    //{
    //    FileStream fileStream = new FileStream(string.Format("{0}/Data/{1}.json", loadPath, fileName), FileMode.Open);
    //    byte[] data = new byte[fileStream.Length];
    //    fileStream.Read(data, 0, data.Length);
    //    fileStream.Close();

    //    string jsonData = Encoding.UTF8.GetString(data);

    //    Debug.Log("Success to Bring JSON");

    //    return JsonToObject<T>(jsonData);
    //}

    //public T LoadJsonFileOverWrite<T>(string loadPath, string fileName, T obj) // json 파일에 저장되어 있는 정보를 가져오는 함수, overwrite 버전
    //{
    //    FileStream fileStream = new FileStream(string.Format("{0}/Data/{1}.json", loadPath, fileName), FileMode.Open);
    //    byte[] data = new byte[fileStream.Length];
    //    fileStream.Read(data, 0, data.Length);
    //    fileStream.Close();

    //    string jsonData = Encoding.UTF8.GetString(data);
    //    var json;
    //    Debug.Log("Success to Bring JSON_Overwrite version");

    //    return JsonToObjectOverwrite<T>(jsonData, obj);
    //}
}
