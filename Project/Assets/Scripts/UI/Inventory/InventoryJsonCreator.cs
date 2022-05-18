using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.IO;
using System.Text;

public class InventoryJsonCreator : MonoBehaviour
{
    public enum ItemType
    {
        None,               // 0
        Weapon,             // 1
        Equipment,          // 2
        Card                // 3
    };

    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }

    public class DataManager
    {
        public Dictionary<int, ItemClass> ItemDict { get; private set; } = new Dictionary<int, ItemClass>();
    }

    [System.Serializable]
    public class ItemClass
    {
        //public enum ItemType
        //{
        //    None,
        //    Weapon,
        //    Equipment,
        //    Card
        //};

        public ItemType itemType;
        public int itemID;
        public int itemCount;
        public string itemName;

        public ItemClass()
        {
            itemType = ItemType.None;
            itemID = -999;
            itemCount = -1;
            itemName = "NULL";
        }

        public void Log()
        {
            Debug.Log(itemType);
            Debug.Log(itemID);
            Debug.Log(itemCount);
            Debug.Log(itemName);
        }

        //public ItemType GetItemType()
        //{
        //    return itemType;
        //}

        //public int GetID()
        //{
        //    return itemID;
        //}

        //public int GetCount()
        //{
        //    return itemCount;
        //}

        //public string GetName()
        //{
        //    return itemName;
        //}

        //public void SetItemType(ItemType idx)
        //{
        //    itemType = idx;
        //}

        //public void SetID(int idx)
        //{
        //    itemID = idx;
        //}

        //public void SetCount(int idx)
        //{
        //    itemCount = idx;
        //}

        //public void SetName(string idx)
        //{
        //    itemName = idx;
        //}

    }

    [System.Serializable]
    public class ItemDataClass : ILoader<int, ItemClass>
    {
        public List<ItemClass> items = new List<ItemClass>();

        public Dictionary<int, ItemClass> MakeDict()
        {
            Dictionary<int, ItemClass> dict = new Dictionary<int, ItemClass>();
            foreach (ItemClass itemClass in items)
                dict.Add(itemClass.itemID, itemClass);

            return dict;
        }

        public ItemDataClass()
        {
            ItemClass nullItem = new ItemClass();
            items.Add(nullItem);
        }
    }

    public ItemDataClass itemData;

    public ItemDataClass GetItemData()
    {
        return itemData;
    }

    // Start is called before the first frame update
    void Start()
    {
        itemData = new ItemDataClass();

        BringItemJSON();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateItemJSON()
    {
        string itemToJsonData = this.gameObject.GetComponent<ControlJSON>().ObjectToJson(itemData);      // item 클래스 내부 변수와 값들을 string으로 변환
        this.gameObject.GetComponent<ControlJSON>().CreateJsonFile(Application.dataPath, "itemData", itemToJsonData); // data 폴더에 json 생성
        Debug.Log("Create ITEM Json File.");
    }

    public void BringItemJSON()
    {
        string data = this.gameObject.GetComponent<ControlJSON>().LoadJsonFile<ItemDataClass>(Application.dataPath, "itemData");
        //Debug.Log(data);
        itemData = JsonUtility.FromJson<ItemDataClass>(data);
    }

    public void OverwriteItemJSON()
    {
        string itemToJsonData = this.gameObject.GetComponent<ControlJSON>().ObjectToJson(itemData);
        this.gameObject.GetComponent<ControlJSON>().OverWriteJsonFile(Application.dataPath, "itemData", itemToJsonData);
        //Debug.Log("Overwrite Player Json File.");
    }
}
