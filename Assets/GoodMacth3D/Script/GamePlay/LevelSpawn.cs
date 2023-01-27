using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class LevelSpawn : SerializedMonoBehaviour
{
    public LevelData levelData;
    public List<LevelData> levelDatas;
    public void Init()
    {
        SpawnLevel();
        levelData.Init();
    }

    public void SpawnLevel()
    {
        levelData = Instantiate(levelDatas[Test.Instance.id]);
    }

    public void OnClickLevel1()
    {

        Test.Instance.id = 0;
        SceneManager.LoadScene("SceneGamePlay");
    }
    public void OnClickLevel2()
    {
        Test.Instance.id = 1;
        SceneManager.LoadScene("SceneGamePlay");
    }
}
