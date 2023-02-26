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
        lsTotalNumberOfBirdAtLocation = new List<TotalNumberOfBirdAtLocation>();
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
            lsTotalNumberOfBirdAtLocation.Add(new TotalNumberOfBirdAtLocation() { id = i, lsIdItem = new List<int>() }); ;
        }

        for (int i = tempID.Count - 1; i >= 0; i--)
        {
            var ran = Random.RandomRange(0, lsTotalNumberOfBirdAtLocation.Count);
            lsTotalNumberOfBirdAtLocation[ran].lsIdItem.Add(tempID[i]);
            tempID.RemoveAt(i);
        }
    }
    public List<TotalNumberOfBirdAtLocation> lsTotalNumberOfBirdAtLocation;
    public int sumBird;
    public int GetTotalNumberOfBirdAtLocation(int idCown)
    {
        int tempId = 0;
        foreach(var item in lsTotalNumberOfBirdAtLocation)
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
    public int GetDataLevelToCheckNull (int idCown)
    {
        int tempId = 0;
        foreach (var item in lsTotalNumberOfBirdAtLocation)
        {
            if (item.id == idCown)
            {
                for (int i = item.lsIdItem.Count - 1; i >= 0; i--)
                {
                    tempId = item.lsIdItem[0];
                    return tempId;
                }
            }
        }
        return tempId;
    }
    public int GetCountLsDataLevel(int idCown)
    {
        int tempId = 0;
        foreach (var item in lsTotalNumberOfBirdAtLocation)
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
    public List<BirdMechanic> lsAllBird;

    public LevelData nextStep;
    public void Init()
    {
        ShufferId();
        foreach (var item in lsTotalNumberOfBirdAtLocation)
        {
            sumBird += item.lsIdItem.Count;
        }
        foreach (var item in doubleBird)
        {
            item.Init();
        }

        foreach (var item in doubleBird)
        {
            if(item.birdInFront != null)
            {
                lsAllBird.Add(item.birdInFront);
            }
            if (item.birdInTheBack != null)
            {
                lsAllBird.Add(item.birdInFront);

            }
        }

    }

 

}
[System.Serializable]
public class TotalNumberOfBirdAtLocation
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