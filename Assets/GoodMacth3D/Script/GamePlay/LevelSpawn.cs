using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSpawn : MonoBehaviour
{
    public int CurrentLevelTest
    {
        get 
        {
            return PlayerPrefs.GetInt("CurrentLevelTest",0);
        }
        set
        {
             PlayerPrefs.SetInt("CurrentLevelTest", value);
        }
    }
    public List<AnimBirdWithId> prefaptBird;
    public LevelData levelData;
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
    public List<LevelData> lsLevelDatas;
    public GameObject vfx;
    public bool AllBirdisReady
    {
        get
        {
            foreach(var item in levelData.lsAllBird)
            {
                if(item.isReady == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
    public void Init()
    {
   
    //    levelData = Instantiate(lsLevelDatas[CurrentLevelTest]);
        if(levelData != null)
        {
            SetUpPrefaptBird();
            levelData.Init();
        }
      
    }
    public void SetUpPrefaptBird()
    {
        foreach(var item in prefaptBird)
        {
            SimplePool2.Preload(item.animBird.gameObject);
            SimplePool2.Preload(item.animBird.gameObject);
            SimplePool2.Preload(item.animBird.gameObject);
            SimplePool2.Preload(item.animBird.gameObject);
        }
    }
   
    public void SpawnEffectBird(int idBird, Transform transform)
    {
        var temp = SimplePool2.Spawn(GetAnimBird(idBird).animBird, transform.position, Quaternion.identity);
         temp.transform.position = transform.position;
        if (GetAnimBird(idBird).lsAnimEffect.Count < 3)
        {
            GetAnimBird(idBird).lsAnimEffect.Add(temp);
          

            if (GetAnimBird(idBird).lsAnimEffect.Count == 3)
            {
                GetAnimBird(idBird).lsAnimEffect[1].SetOrderInLayer(2);
                GetAnimBird(idBird).lsAnimEffect[0].SetOrderInLayer(1);
                GetAnimBird(idBird).lsAnimEffect[2].SetOrderInLayer(1);
                var bird0 = GetAnimBird(idBird).lsAnimEffect[0];
                var bird1 = GetAnimBird(idBird).lsAnimEffect[1];
                var bird2 = GetAnimBird(idBird).lsAnimEffect[2];

                bird0.transform.DOMoveY(bird0.gameObject.transform.position.y+0.75f, 0.2f);
                bird1.transform.DOMoveY(bird0.gameObject.transform.position.y + 0.75f, 0.2f);
                bird2.transform.DOMoveY(bird0.gameObject.transform.position.y + 0.75f, 0.2f).OnComplete(delegate {

                    bird0.transform.DOMove(bird1.gameObject.transform.position, 0.2f);
                    bird2.transform.DOMove(bird1.gameObject.transform.position, 0.2f).OnComplete(delegate {
                        bird0.gameObject.SetActive(false);
                        bird2.gameObject.SetActive(false);
                        var tempScale = bird1.gameObject.transform.localScale;
                        bird1.transform.DOScale(tempScale * 1.1f, 0.2f).OnComplete(delegate {
                            bird1.transform.DOScale(tempScale, 0.2f).OnComplete(delegate {
                                var vecVfx = bird1.transform.position;
                                SimplePool2.Spawn(vfx, new Vector3(vecVfx.x, vecVfx.y + 0.2f, vecVfx.z), Quaternion.identity);
                                for (int j = GetAnimBird(idBird).lsAnimEffect.Count - 1; j >= 0; j--)
                                {
                                    SimplePool2.Despawn(GetAnimBird(idBird).lsAnimEffect[j].gameObject);
                                    GetAnimBird(idBird).lsAnimEffect.RemoveAt(j);
                                }
                            });
                        });
                    });

                });

                         
            }



        }
      
    }

   

}
[System.Serializable]
public class AnimBirdWithId
{
    public int id;
    public AnimBird animBird;
    public List<AnimBird> lsAnimEffect;
}