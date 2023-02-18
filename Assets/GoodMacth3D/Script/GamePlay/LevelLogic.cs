using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class LevelLogic : MonoBehaviour
{
    private Level level;
    public List<BirdMechanic> lsBirdEndGame;
    public List<IdAndNumb> lsIdAndNumb;
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
    public GameObject losePanel;
    public GameObject winPanel;
    public List<IdAndNumbBirdDuplicate> lsCount;
    private List<int> lsBirdTemp;
    public Transform parentSlotBird;
    public SlotBird slotBird;
    public List<SlotBird> lsSlotBird = new List<SlotBird>();
    public List<SlotBird> lsRedoSlotBird = new List<SlotBird>();
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

    public void Init(Level param)
    {
        level = param;
        SetUp();
    }
    public void SetUp()
    {
        //for(int i = 0; i < level.levelSpawn.levelData.idBlock.Count; i ++)
        //{
        //    lsIdAndNumb.Add(new IdAndNumb() { id = level.levelSpawn.levelData.idBlock[i], numb = 0 });
        //}
    }
    public void AddBirdToListSlot(BirdMechanic bird)
    {
        int indexNewSlot = GetPostOfSlot(bird);
        var tempSlot = Instantiate(slotBird, GetPost(indexNewSlot).finalPost.transform.position, Quaternion.identity);
        tempSlot.birdMechanic = bird;
        tempSlot.transform.parent = parentSlotBird;
        bird.transform.parent = tempSlot.transform;
        RotateBird(bird, tempSlot.transform);
        bird.Fly(delegate { SetSlotFinished(tempSlot); });
        lsSlotBird.Insert(indexNewSlot, tempSlot);
        StartCoroutine(SetListSlot_ResetPosition_Now(0.01f));
    }
    public int GetPostOfSlot(BirdMechanic bird)
    {
        int indexSlot = lsSlotBird.Count;
        for (int i = lsSlotBird.Count - 1; i >= 0; i--)
        {
            if (lsSlotBird[i].birdMechanic.id == bird.id)
            {
                return i + 1;
            }
        }
        return indexSlot;
    } 
    public void SetSlotFinished(SlotBird slotBird)
    {
        var tempCount = GetIdAndNumb(slotBird.birdMechanic.id);

        tempCount.numb += 1;
        tempCount.lsBird.Add(slotBird);

        foreach (var item in lsIdAndNumb)
        {
            if(item.numb >= 3)
            {
               
                for(int i = 2; i >= 0; i --)
                {
                    lsSlotBird.Remove(item.lsBird[i]);
                }
                for (int i = 2; i >= 0; i--)
                {
                    item.lsBird[i].gameObject.SetActive(false);
                    item.lsBird.RemoveAt(i);
                    item.numb -= 1;
                    level.levelSpawn.levelData2.sumBird -= 1;
                }
               
                StartCoroutine(SetListSlot_ResetPosition_Now(0.01f));
            }       
        }
        HandleWin();
    }
    public IEnumerator SetListSlot_ResetPosition_Now(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < lsSlotBird.Count; i++)
        {
            lsSlotBird[i].ResetPosSlot(GetPost(i));
        }

    }
    private void RotateBird(BirdMechanic paramBird, Transform paramTranform)
    {
        var tempBirdLocalScale = paramBird.animBird.transform.localScale;
        if (paramBird.transform.position.x >= paramTranform.transform.position.x)
        {
            if (paramBird.right)
            {
                paramBird.animBird.transform.localScale = new Vector3(tempBirdLocalScale.x, tempBirdLocalScale.y, tempBirdLocalScale.z);
            }
            else
            {
                paramBird.animBird.transform.localScale = new Vector3(-tempBirdLocalScale.x, tempBirdLocalScale.y, tempBirdLocalScale.z);
            }
        }
        else
        {
            if (paramBird.right)
            {
                paramBird.animBird.transform.localScale = new Vector3(-tempBirdLocalScale.x, tempBirdLocalScale.y, tempBirdLocalScale.z);
            }
            else
            {
                paramBird.animBird.transform.localScale = new Vector3(tempBirdLocalScale.x, tempBirdLocalScale.y, tempBirdLocalScale.z);
            }

        }
    }
    public void HandleCheckLose(BirdMechanic paramBlock)
    {
        if (lsSlotBird.Count >= 6)
        {
            lsBirdEndGame.Add(paramBlock);
            if (lsBirdEndGame.Count <= 1)
            {
                var temp = GetIdAndNumb(lsBirdEndGame[0].id);
                if (temp.numb >= 2)
                {
                  
                    return;
                }
                if (SuportCheckLoseOneBird)
                {
                    return;
                }
                if(SuportCheckLoseMethodOneBird(lsBirdEndGame[0]))
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
                if (birdCondition != null)
                {
                    var tempBirdCount = GetIdAndNumb(birdCondition.id);
                    if (tempBirdCount.numb == 1)
                    {
                        conditionHas1Bird = true;
                    }
                    if (conditionHas1Bird && conditionHas2BirdFlying)
                    {
                        return;
                    }     
                }
                else
                {
                    for (int i = lsBirdEndGame.Count - 1; i >= 0; i--)
                    {
                        if(SuportCheckLoseMethodOneBird(lsBirdEndGame[i]))
                        {
                            return;
                        }
                    }
                }
            }
            losePanel.SetActive(true);
 
        }
        else
        {
            lsBirdEndGame.Clear();
        }      
    }
    private bool SuportCheckLoseOneBird
    {
        get
        {
            lsCount = new List<IdAndNumbBirdDuplicate>();
            lsBirdTemp = new List<int>();

            foreach (var item in lsSlotBird)
            {
                lsBirdTemp.Add(item.birdMechanic.id);
            }

            var ienum = lsBirdTemp.Distinct();
            lsBirdTemp = ienum.ToList<int>();

            foreach (var item in lsBirdTemp)
            {
                lsCount.Add(new IdAndNumbBirdDuplicate() { id = item });
            }


            for (int i = lsSlotBird.Count -1; i >=0 ; i--)
            {
                foreach (var item in lsCount)
                {
                    if (item.id == lsSlotBird[i].birdMechanic.id)
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
    private bool SuportCheckLoseMethodOneBird(BirdMechanic birdMechanic)
    {
        var count = 0;
        for (int i = 0; i < lsSlotBird.Count; i++)
        {
            if(lsSlotBird[i].birdMechanic.id == birdMechanic.id)
            {
                count += 1;
            }
        }
     
        if (count >= 2)
        {
            return true;
        }
        
        return false;
    }
    public void HandleWin()
    {
        if (level.levelSpawn.levelData2.sumBird <= 0)
        {
            winPanel.SetActive(true);
        }
    }

    public void BoosterRedo()
    {
        if(lsSlotBird.Count > 3)
        {
            lsRedoSlotBird = new List<SlotBird>();
            for (int i = lsSlotBird.Count - 3 ; i < lsSlotBird.Count; i ++)
            {
                Debug.LogError("lon hon ba");
                lsRedoSlotBird.Add(lsSlotBird[i]);        
            }
            foreach (var item in lsRedoSlotBird)
            {
                lsSlotBird.Remove(item);
                var count = GetIdAndNumb(item.birdMechanic.id);
                if (count.numb >0)
                {
                    count.numb -= 1;
                    count.lsBird.Remove(item);
                }
                // item.RedoSlot(level.levelSpawn.rightPost);
                //  RedoSlotToData(item.birdMechanic, 2);
                RedoSlotToData2(item);
            }
            
        }
        else
        {
            lsRedoSlotBird = new List<SlotBird>();
            for (int i = lsSlotBird.Count - 1; i >= 0; i--)
            {
                Debug.LogError("nho hon ba");
                lsRedoSlotBird.Add(lsSlotBird[i]);
            }
            foreach (var item in lsRedoSlotBird)
            {
                lsSlotBird.Remove(item);
                var count = GetIdAndNumb(item.birdMechanic.id);
                if (count.numb > 0)
                {
                    count.numb -= 1;
                    count.lsBird.Remove(item);
                }
                // item.RedoSlot(level.levelSpawn.rightPost);
                //  RedoSlotToData(item.birdMechanic, lsRedoSlotBird.Count);
                RedoSlotToData2(item);
            }

        }
    }
    public DataLevel dataLevel;
    public DoubleBird tempDoubleBird;

    public void RedoSlotToData2(SlotBird slotBird)
    {
        Debug.LogError("RedoSlotToData2");
        var levelData = level.levelSpawn.levelData2.lsDataLevel;      
        var ran = Random.RandomRange(0, levelData.Count);
        dataLevel = new DataLevel();
        dataLevel = levelData[ran];

        if (dataLevel.lsIdItem.Count > 0)
        {
            dataLevel.lsIdItem.Add(slotBird.birdMechanic.id);
            Debug.LogError("Count > 0");
         
        }
        else
        {
             tempDoubleBird = level.levelSpawn.levelData2.GetDoubleBird(dataLevel.id);
            if (tempDoubleBird.birdInFront == null)
            {
                tempDoubleBird.birdInFront = slotBird.birdMechanic;
                slotBird.birdMechanic.transform.parent = tempDoubleBird.transform;
                slotBird.birdMechanic.animBird.SetOrderInLayer(2);
                slotBird.birdMechanic.transform.DOMove(tempDoubleBird.postFront, 0.3f).OnComplete(delegate
                {
                    slotBird.birdMechanic.UnlockClick();
                });
                Debug.LogError("birdInFront == null");
                return;
            }
            if (tempDoubleBird.birdInFront != null && tempDoubleBird.birdInTheBack == null)
            {
                tempDoubleBird.birdInTheBack = slotBird.birdMechanic;
         
                slotBird.birdMechanic.transform.parent = tempDoubleBird.transform;
                slotBird.birdMechanic.animBird.SetOrderInLayer(1);
                slotBird.birdMechanic.transform.DOMove(tempDoubleBird.postInBack, 0.3f).OnComplete(delegate
                {

                });
                Debug.LogError("birdInFront != null // birdInTheBack == null");
                return;
            }
            if (tempDoubleBird.birdInFront != null && tempDoubleBird.birdInTheBack != null)
            {
                dataLevel.lsIdItem.Add(slotBird.birdMechanic.id);
                slotBird.RedoSlot(level.levelSpawn.rightPost);
                Debug.LogError("birdInFront != null // birdInTheBack != null");
                return;
            }
        }

    }

    //public void RedoSlotToData(BirdMechanic bird, int coutnRan)
    //{
    //    var levelData = level.levelSpawn.levelData2.lsDataLevel;
    //     listData = new List<DataLevel>();
    //    var tempLsRan = new List<DataLevel>();
    //    foreach(var item in levelData)
    //    {
    //        tempLsRan.Add(item);
    //    }

    //    for (int i =0; i <= coutnRan; i++ )
    //    {
    //        var ran = Random.RandomRange(0, tempLsRan.Count);
    //        try
    //        {
    //            listData.Add(levelData[ran]);
    //            tempLsRan.RemoveAt(ran);
    //        }
    //      catch
    //        {

    //        }
    //    }
    //    foreach(var item in listData)
    //    {
    //        if(item.lsIdItem.Count > 0)
    //        {
    //            item.lsIdItem.Add(bird.id);
    //            Debug.LogError("Count > 0");
    //            break;
    //        }
    //        else
    //        {
    //            var tempDoubleBird = level.levelSpawn.levelData2.GetDoubleBird(item.id);
    //            if (tempDoubleBird.birdInFront == null)
    //            {
    //                tempDoubleBird.birdInFront = bird;
    //                bird.transform.parent = tempDoubleBird.transform;
    //                bird.animBird.SetOrderInLayer(2);
    //                bird.transform.DOMove(tempDoubleBird.postFront.position,0.3f).OnComplete(delegate {
    //                    bird.UnlockClick();
    //                });
    //                    Debug.LogError("birdInFront == null");
    //                break;
    //            }
    //            if (tempDoubleBird.birdInFront != null && tempDoubleBird.birdInTheBack == null)
    //            {
    //                tempDoubleBird.birdInTheBack = bird;
    //                tempDoubleBird.birdInFront.behindBird = bird;
    //                bird.transform.parent = tempDoubleBird.transform;
    //                bird.animBird.SetOrderInLayer(1);
    //                bird.transform.DOMove(tempDoubleBird.postFront.position, 0.3f).OnComplete(delegate {

    //                });
    //                Debug.LogError("birdInFront != null // birdInTheBack == null");
    //                break;
    //            }
    //            if (tempDoubleBird.birdInFront != null && tempDoubleBird.birdInTheBack != null)
    //            {
    //                item.lsIdItem.Add(bird.id);
    //                Debug.LogError("birdInFront != null // birdInTheBack != null");
    //                break;
    //            }
    //        }
    //    }


    //    //foreach(var item in levelData.lsDataLevel)
    //    //{
    //    //    if(item.lsIdItem.Count > 0)
    //    //    {
    //    //        listData.Add(item);
    //    //    }
    //    //}
    //    //var ran = Random.RandomRange(0, listData.Count);
    //    //listData[ran].lsIdItem.Add(idBird);


    //}

}
[System.Serializable]
public class IdAndNumb
{
    public int id;
    public int numb;
    public List<SlotBird> lsBird;
   
    public IdAndNumb()
    {
        lsBird = new List<SlotBird>();
     
    }

  
}
[System.Serializable]
public class IdAndNumbBirdDuplicate
{
    public int id;
    public int numb;
  
}