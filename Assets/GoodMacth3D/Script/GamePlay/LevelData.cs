using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LevelData : MonoBehaviour
{
    public int id;
    public int numOfPost;
    public List<NumbEditLevel> lsDataEdit;
    public List<int> tempID;


    [Button]
    private void ShufferId()
    {
        tempID = new List<int>();
        lsDataLevel = new List<DataLevel>();
        foreach (var item in lsDataEdit)
        {
            for (int i = 0; i < item.count; i++)
            {
                tempID.Add(item.id);
            }
        }
        tempID.Shuffle();
        Debug.LogError(tempID.Count);
        for (int i = 1; i <= numOfPost; i++)
        {
            lsDataLevel.Add(new DataLevel() { id = i, lsIdItem = new List<int>() }); ;
        }

        for (int i = tempID.Count - 1; i >= 0; i--)
        {
            var ran = Random.RandomRange(0, lsDataLevel.Count);
            lsDataLevel[ran].lsIdItem.Add(tempID[i]);
            tempID.RemoveAt(i);
        }
    }
    public List<DataLevel> lsDataLevel;
    public int sumBird;
    public int GetDataLevel(int idCown)
    {
        int tempId = 0;
        foreach(var item in lsDataLevel)
        {
            if(item.id == idCown)
            {
                for(int i = item.lsIdItem.Count -1; i >= 0 ; i--)
                {                  
                    tempId = item.lsIdItem[0];
                    item.lsIdItem.RemoveAt(0);
                    return tempId;                  
                }
            }
        }
        return tempId;
    }
    public int GetCountLsDataLevel(int idCown)
    {
        int tempId = 0;
        foreach (var item in lsDataLevel)
        {
            if (item.id == idCown)
            {
                tempId = item.lsIdItem.Count;
                    return tempId;
                
            }
        }
        return tempId;
    }

    public List<DoubleBird> doubleBird;
    public DoubleBird GetDoubleBird(int paramIdCowInData)
    {
        foreach (var item in doubleBird)
        {
            if(item.idCowInData == paramIdCowInData)
            {
                return item;
            }
        }
        return null;
    }


    public void Init()
    {
        foreach (var item in lsDataLevel)
        {
            sumBird += item.lsIdItem.Count;
        }
        foreach (var item in doubleBird)
        {
            item.Init();
        }
      
    }

 

}
[System.Serializable]
public class DataLevel
{
    public int id;
    public List<int> lsIdItem;
}
[System.Serializable]
public class NumbEditLevel
{
    public int id;
    public int count;
}