using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public List<DataLevel> lsDataLevel;
    public int GetDataLevel(int idCown)
    {
        int tempId = 0;
        foreach(var item in lsDataLevel)
        {
            if(item.id == idCown)
            {
                for(int i = item.lsIdItem.Count -1; i >= 0 ; i --)
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



    public void Init()
    {
       foreach(var item in doubleBird)
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