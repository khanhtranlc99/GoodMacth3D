using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSpawn : MonoBehaviour
{
 
    public List<AnimBirdWithId> prefaptBird;
    public LevelData levelData2;
    public Transform rightPost;
    public Transform leftPost;
    public BirdMechanic birdMechanic;
    public AnimBirdWithId GetAnimBird(int id)
    {
        foreach (var item in prefaptBird)
        {
            if(item.id == id)
            {
                return item;
            }
        }
        return null;
    }
    public void Init()
    {
        levelData2.Init();
    }

 

    
}
[System.Serializable]
public class AnimBirdWithId
{
    public int id;
    public AnimBird animBird;
}