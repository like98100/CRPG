using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.IO;
using System.Text;

public class RewardJsonCreator : MonoBehaviour
{
    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }

    public class DataManager
    {
        public Dictionary<int, RewardClass> RewardDict { get; set; } = new Dictionary<int, RewardClass>();
    }

    [System.Serializable]
    public class RewardClass
    {
        public int stageID;
        public List<int> itemID = new List<int>();
        public List<int> itemCnt = new List<int>();


        public RewardClass()
        {
            stageID = 1;
            for(int i = 1; i < 4; i++)
            {
                itemID.Add(i);
                itemCnt.Add(i);
            }
        }

        public void Log()
        {
            Debug.Log("stageID : " + stageID);
            for(int i = 0; i < itemID.Count; i++)
            {
                Debug.Log("itemID : " + itemID[i]);
                Debug.Log("itemCnt : " + itemCnt[i]);
            }
        }
    }

    [System.Serializable]
    public class RewardDataClass : ILoader<int, RewardClass>
    {
        public List<RewardClass> rewards = new List<RewardClass>();

        public Dictionary<int, RewardClass> MakeDict()
        {
            Dictionary<int, RewardClass> dict = new Dictionary<int, RewardClass>();
            foreach (RewardClass rewardClass in rewards) dict.Add(rewardClass.stageID, rewardClass);

            return dict;
        }

        public RewardDataClass()
        {
            RewardClass nullItem = new RewardClass();
            rewards.Add(nullItem);
        }
    }

    RewardDataClass rewardData;

    public RewardDataClass GetRewardData()
    {
        return rewardData;
    }

    // Start is called before the first frame update
    void Start()
    {
        rewardData = new RewardDataClass();

        //createRewardJSON();
        bringRewardJSON();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createRewardJSON()
    {
        string rewardToJsonData = this.gameObject.GetComponent<ControlJSON>().ObjectToJson(rewardData);
        this.gameObject.GetComponent<ControlJSON>().CreateJsonFile(Application.dataPath, "rewardData", rewardToJsonData);
        Debug.Log("Create REWARD Json File.");
    }

    void bringRewardJSON()
    {
        string data = this.gameObject.GetComponent<ControlJSON>().LoadJsonFile<RewardDataClass>(Application.dataPath, "rewardData");
        rewardData = JsonUtility.FromJson<RewardDataClass>(data);
    }
}
