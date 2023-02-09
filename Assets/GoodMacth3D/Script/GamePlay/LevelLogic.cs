using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class LevelLogic : MonoBehaviour
{
    public List<BirdMechanic> lsBirdEndGame;
    public List<BirdMechanic> lsBird;
    public List<int> tesst;
    public List<int> lsLockDelete;
    public List<IdAndNumb> lsIdAndNumb;
    public GameObject losePanel;
    public GameObject winPanel;
    private Level level;
    public List<IdAndNumbBirdDuplicate> lsCount;
    public List<int> lsBirdTemp;
    public void Init(Level param)
    {
        level = param;
        SetUp();
    }


    public IdAndNumb GetIdAndNumb(int id)
    {
        foreach (var item in lsIdAndNumb)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }
    public List<Post> lsPost;
    public Post GetPost(int id)
    {
        foreach (var item in lsPost)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }
   
    public void SetUp()
    {
        //for(int i = 0; i < level.levelSpawn.levelData.idBlock.Count; i ++)
        //{
        //    lsIdAndNumb.Add(new IdAndNumb() { id = level.levelSpawn.levelData.idBlock[i], numb = 0 });
        //}
    }
    public void SortBird(BirdMechanic bird)
    {
        if (lsBird.Count <= 0)
        {
            lsBird.Add(bird);
            tesst.Add(bird.id);
        }
        else
        {
            bool same = false;
            int order = 0;
            for (int i = 0; i < lsBird.Count; i++)
            {
                if (lsBird[i].id == bird.id)
                {
                    same = true;
                    order = i;
                }
            }
            if (same)
            {
                lsBird.Insert(order + 1, bird);
                tesst.Insert(order + 1, bird.id);
            }
            else
            {
                lsBird.Add(bird);
                tesst.Add(bird.id);
            }
        }
        SortIdElementBirds();
        MoveBlocks();
    }
    public void SortIdElementBirds()
    {
        for (int i = 0; i < lsBird.Count; i++)
        {
            lsBird[i].idElement = i;
        }
    }
    public void MoveBlocks()
    {
        
        for (int i = 0; i < lsBird.Count; i++)
        {
            if (lsBird[i] != null)
            {
                //lsBird[i].transform.DOKill();
                lsBird[i].transform.DOMove(lsPost[i].post.position, 0.7f).OnComplete(delegate { SortIdElementBirds(); DeleteBirds(); });
            }
        
        }
    }
    public void HandleDeleteBirds(BirdMechanic paramBlock)
    {
        var temp = GetIdAndNumb(paramBlock.id);
        if (temp != null)
        {
            if (temp.lsBlock.Count < 3)
            {
                temp.numb += 1;
                temp.lsBlock.Add(paramBlock);
                temp.lsAnimBird.Add(paramBlock.animBird);
                if (lsLockDelete.Count > 0)
                {
                    return;
                }
               
                DeleteBirds();
            }
        }
    }


    private void DeleteBirds()
    {
        for (int i = lsIdAndNumb.Count - 1; i >= 0; i--)
        {
            if (lsIdAndNumb[i].numb == 3)
            {
                for (int j = lsIdAndNumb[i].lsBlock.Count - 1; j >= 0; j--)
                {
                    lsBird.RemoveAt(lsIdAndNumb[i].lsBlock[j].idElement);
                    tesst.RemoveAt(lsIdAndNumb[i].lsBlock[j].idElement);
                }

                for (int j = lsIdAndNumb[i].lsBlock.Count - 1; j >= 0; j--)
                {
                    lsIdAndNumb[i].lsBlock[j].gameObject.gameObject.SetActive(false);
                  //  Destroy(lsIdAndNumb[i].lsBlock[j].gameObject);
                    level.levelSpawn.levelData2.sumBird -= 1;
                    lsIdAndNumb[i].lsBlock.RemoveAt(j);
                }


              
                lsIdAndNumb[i].numb = 0;
         
                SortIdElementBirds();
                MoveBlocks();
          
            }

        }



      //  HandleWin();

    }
    private void TestEffect()
    {
        for (int i = lsIdAndNumb.Count - 1; i >= 0; i--)
        {
            if (lsIdAndNumb[i].numb == 3)
            {
                lsIdAndNumb[i].HandleOffBird(this.transform);
            }
        }
    }

    public void HandleCheckLose(BirdMechanic paramBlock)
    {
        if (lsBird.Count >= 6)
        {
            lsBirdEndGame.Add(paramBlock);
            if (lsBirdEndGame.Count <= 1)
            {
                var temp = GetIdAndNumb(lsBirdEndGame[0].id);
                if (temp.numb >= 2)
                {
                    Debug.LogError("numb " );
                    return;
                }

                if (SuportCheckLose)
                {
                    return;
                }
                Debug.LogError("One ");
            }
            else
            {
                for (int i = lsBirdEndGame.Count - 1; i >= 0; i--)
                {
                    var temp = GetIdAndNumb(lsBirdEndGame[i].id);
                    /////dayyyyroiiiii
                    ///
                    if (temp.numb >= 2)
                    {
                       return;
                    }
                }

                var conditionHas1Bird = false;
                var conditionHas2BirdFlying = false;
                var birdCondition = new BirdMechanic();
                for (int i = 0; i < lsBirdEndGame.Count - 1; i++)
                {
                    for (int j = i + 1; j < lsBirdEndGame.Count; j++)
                    {
                        if (lsBirdEndGame.ElementAt(i) == lsBirdEndGame.ElementAt(j))
                        {
                            conditionHas2BirdFlying = true;
                            birdCondition = lsBirdEndGame[i];
                        }
                       
                    }
                }
                var tempBirdCount = GetIdAndNumb(birdCondition.id);
                if (tempBirdCount.numb == 1)
                {
                    conditionHas1Bird = true;
                }
                if(conditionHas1Bird && conditionHas2BirdFlying)
                {
                    return;
                }

                Debug.LogError("Double");
            }
            losePanel.SetActive(true);
 
        }
        else
        {
            lsBirdEndGame.Clear();
        }

        
    }

    private bool SuportCheckLose
    {
        get
        {
            lsCount = new List<IdAndNumbBirdDuplicate>();
            lsBirdTemp = new List<int>();

            foreach (var item in lsBird)
            {
                lsBirdTemp.Add(item.id);
            }

            var ienum = lsBirdTemp.Distinct();
            lsBirdTemp = ienum.ToList<int>();

            foreach (var item in lsBirdTemp)
            {
                lsCount.Add(new IdAndNumbBirdDuplicate() { id = item });
            }


            for (int i = 0; i < lsBird.Count - 1; i++)
            {
                foreach (var item in lsCount)
                {
                    if (item.id == lsBird[i].id)
                    {
                        item.numb += 1;
                    }
                }
            }

            foreach (var item in lsCount)
            {
                if (item.numb >= 3)
                {
                    return true;
                }
            }

            return false;

        }
    

    }

    public void HandleWin()
    {
        if (level.levelSpawn.levelData2.sumBird <= 0)
        {
            winPanel.SetActive(true);
        }
    }


}
[System.Serializable]
public class IdAndNumb
{
    public int id;
    public int numb;
    public List<BirdMechanic> lsBlock;
    public List<AnimBird> lsAnimBird;
    public IdAndNumb()
    {
        lsBlock = new List<BirdMechanic>();
        lsAnimBird = new List<AnimBird>();
    }
    public void HandleOffBird(Transform param)
    {
       
        foreach(var item in lsAnimBird)
        {
            item.gameObject.transform.SetParent(param);
        }

        lsAnimBird[0].transform.DOMove(lsAnimBird[1].gameObject.transform.position, 0.2f);
        lsAnimBird[2].transform.DOMove(lsAnimBird[1].gameObject.transform.position, 0.2f).OnComplete(delegate {
            for (int j = lsAnimBird.Count - 1; j >= 0; j--)
            {
                SimplePool2.Despawn(lsAnimBird[j].gameObject);
                lsAnimBird.RemoveAt(j);
            }
        });

       

      
    }
}
[System.Serializable]
public class IdAndNumbBirdDuplicate
{
    public int id;
    public int numb;
  
}