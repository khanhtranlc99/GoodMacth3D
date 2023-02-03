using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LevelData : SerializedMonoBehaviour
{
    public int level;
    public List<Data> lsData;
    public List<int> idBlock;
    public int numBlock;
    #region Edit
    [Header("===Value Edit===")]
    public int deeps;
    public int cow;
    public int row;
    [Button]
    private void SpawnArray()
    {

      for(int i = 0; i < deeps; i++)
        {
            lsData.Add(new Data() { data = new GameObject[cow, row] });
        }

    }
    #endregion

    public void Init()
    {
        SetUpLevel();
    }
    public void SetUpLevel()
    {
        int index = 0;
        for(int i = 0; i < lsData.Count; i ++)
        {
            index = i;

            for (int j = 0; j < lsData[index].data.GetLength(0); j++)
            {
                for (int k = 0; k < lsData[index].data.GetLength(1); k++)
                {
                    lsData[index].data[j, k].GetComponent<BirdMechanic>().LockClick();
                    if (index + 1 < lsData.Count)
                    {
                        lsData[index].data[j, k].GetComponent<BirdMechanic>().unlockBlock = lsData[index + 1].data[j, k].GetComponent<BirdMechanic>();
                    }
                }
            }
        }


        for (int j = 0; j < lsData[0].data.GetLength(0); j++)
        {
            for (int k = 0; k < lsData[0].data.GetLength(1); k++)
            {

                lsData[0].data[j, k].GetComponent<BirdMechanic>().UnlockClick();

            }
        }


    }


    public bool IsWin
    {
        get
        {
            if(numBlock <= 0)
            {
                return true;
            }
            return false;
        }
       

    }
}

public class Data
{
    public GameObject[,] data;

}