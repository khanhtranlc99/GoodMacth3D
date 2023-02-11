using DG.Tweening;
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
        SetUpPrefaptBird();
        levelData2.Init();
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
        //temp.SetAnim(temp.FlY, true);
        //  temp.transform.localScale = transform.localScale;
        //temp.transformFollow = transform;
        //temp.wasFollow = true;
         temp.transform.position = transform.position;
        if (GetAnimBird(idBird).lsAnimEffect.Count < 3)
        {
            GetAnimBird(idBird).lsAnimEffect.Add(temp.gameObject);

            if (GetAnimBird(idBird).lsAnimEffect.Count == 3)
            {
                GetAnimBird(idBird).lsAnimEffect[0].transform.DOMoveY(GetAnimBird(idBird).lsAnimEffect[0].gameObject.transform.position.y + 0.2f, 0.2f);
                GetAnimBird(idBird).lsAnimEffect[1].transform.DOMoveY(GetAnimBird(idBird).lsAnimEffect[1].gameObject.transform.position.y + 0.2f, 0.2f);
                GetAnimBird(idBird).lsAnimEffect[2].transform.DOMoveY(GetAnimBird(idBird).lsAnimEffect[2].gameObject.transform.position.y + 0.2f, 0.2f).OnComplete(delegate {

                    GetAnimBird(idBird).lsAnimEffect[0].transform.DOMove(GetAnimBird(idBird).lsAnimEffect[1].gameObject.transform.position, 0.2f);
                    GetAnimBird(idBird).lsAnimEffect[2].transform.DOMove(GetAnimBird(idBird).lsAnimEffect[1].gameObject.transform.position, 0.2f).OnComplete(delegate {
                        for (int j = GetAnimBird(idBird).lsAnimEffect.Count - 1; j >= 0; j--)
                        {
                            SimplePool2.Despawn(GetAnimBird(idBird).lsAnimEffect[j].gameObject);
                            GetAnimBird(idBird).lsAnimEffect.RemoveAt(j);
                        }
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
    public List<GameObject> lsAnimEffect;
}