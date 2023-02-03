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
    private void Update()
    {
        if(level != null)
        {
            Win();
        }
      
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
            lsBird[i].transform.DOMove(lsPost[i].post.position, 0.15f).OnComplete(delegate { SortIdElementBirds(); DeleteBirds();  });
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

                    Destroy(lsIdAndNumb[i].lsBlock[j].gameObject);
                    lsIdAndNumb[i].lsBlock.RemoveAt(j);
                }
                lsIdAndNumb[i].numb = 0;
                SortIdElementBirds();
                MoveBlocks();
            }

        }
      
    }

    public void HandleEndGame(BirdMechanic paramBlock)
    {
        if (lsBird.Count >= 6)
        {
            lsBirdEndGame.Add(paramBlock);
            if (lsBirdEndGame.Count <= 1)
            {
                var temp = GetIdAndNumb(lsBirdEndGame[0].id);
                if (temp.numb >= 2)
                {
                    return;
                }
            }
            else
            {
                for (int i = lsBirdEndGame.Count - 1; i >= 0; i--)
                {
                    var temp = GetIdAndNumb(lsBirdEndGame[i].id);
                    if (temp.numb >= 2)
                    {
                        return;
                    }
                }
            }
            losePanel.SetActive(true);
            lsBirdEndGame.Clear();
        }

        
    }

    public void Win()
    {
        //if (level.levelSpawn.levelData.IsWin)
        //{
        //    winPanel.SetActive(true);
        //}
    }


}
[System.Serializable]
public class IdAndNumb
{
    public int id;
    public int numb;
    public List<BirdMechanic> lsBlock;
    public IdAndNumb()
    {
        lsBlock = new List<BirdMechanic>();
    }
}
