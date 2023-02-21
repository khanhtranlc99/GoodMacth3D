using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class LevelLogic : MonoBehaviour
{
    #region Var
    private Level level;
    public List<BirdMechanic> lsBirdEndGame;
    public List<IdAndNumb> lsIdAndNumb; 
    public GameObject losePanel;
    public GameObject winPanel;
    public List<IdAndNumbBirdDuplicate> lsCount;
    private List<int> lsBirdTemp;
    public Transform parentSlotBird;
    public SlotBird slotBird;
    public List<SlotBird> lsSlotBird = new List<SlotBird>();
    public List<SlotBird> lsRedoSlotBird = new List<SlotBird>();
    public List<Post> lsPost;
    public List<int> lsLoockBooster;
    #endregion
    #region Get
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
    #endregion
    #region InitMethod
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
    #endregion
    #region Sort
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
    #endregion
    #region EndGame
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
    #endregion
    #region BoosterRedo
    private DataLevel dataLevel;
    private DoubleBird tempDoubleBird;
    public void BoosterRedo()
    {
        if (lsLoockBooster.Count > 0)
        {
            return;
        }
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
                RedoSlotToData(item);
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
                RedoSlotToData(item);
            }

        }
    }
    public void RedoSlotToData(SlotBird slotBird)
    {
        Debug.Log("RedoSlotToData2");
        var levelData = level.levelSpawn.levelData2.lsDataLevel;      
        var ran = Random.RandomRange(0, levelData.Count);
        dataLevel = new DataLevel();
        dataLevel = levelData[ran];
        slotBird.birdMechanic.behindBird = null;
        if (dataLevel.lsIdItem.Count > 0)
        {
            dataLevel.lsIdItem.Add(slotBird.birdMechanic.id);
            RotateBirdRedo(slotBird.birdMechanic, level.levelSpawn.leftPost.position);
            slotBird.birdMechanic.animBird.SetAnim(slotBird.birdMechanic.animBird.FlY, true);
            slotBird.RedoSlot(level.levelSpawn.leftPost);          
            Debug.Log("Count > 0");
            return;
        }
        else
        {
            tempDoubleBird = new DoubleBird();
             tempDoubleBird = level.levelSpawn.levelData2.GetDoubleBird(dataLevel.id);
            if (tempDoubleBird.birdInFront != null && tempDoubleBird.birdInTheBack != null)
            {
                dataLevel.lsIdItem.Add(slotBird.birdMechanic.id);
                slotBird.birdMechanic.animBird.SetAnim(slotBird.birdMechanic.animBird.FlY, true);
                RotateBirdRedo(slotBird.birdMechanic, level.levelSpawn.leftPost.position);
                slotBird.RedoSlot(level.levelSpawn.leftPost);
                Debug.Log("birdInFront != null // birdInTheBack != null");
                return;
            }
            if (tempDoubleBird.birdInFront != null && tempDoubleBird.birdInTheBack == null)
            {
             
                tempDoubleBird.birdInTheBack = slotBird.birdMechanic;
                tempDoubleBird.birdInFront.behindBird = slotBird.birdMechanic;
                slotBird.birdMechanic.idCowInData = tempDoubleBird.idCowInData;
                slotBird.birdMechanic.transform.parent = tempDoubleBird.transform;
                slotBird.birdMechanic.animBird.SetOrderInLayer(1);            
                slotBird.birdMechanic.animBird.SetAnim(slotBird.birdMechanic.animBird.FlY, true);
               // RotateBirdRedo(slotBird.birdMechanic, tempDoubleBird.postInBack);
                slotBird.birdMechanic.transform.DOMove(tempDoubleBird.postInBack, 0.3f).OnComplete(delegate
                {           
                    slotBird.birdMechanic.LockClick();
                    slotBird.birdMechanic.animBird.SetAnim(slotBird.birdMechanic.animBird.IDLE, true);
                });
                Debug.Log("birdInFront != null // birdInTheBack == null");
                return;
            }
            if (tempDoubleBird.birdInFront == null)
            {
                
                tempDoubleBird.birdInFront = slotBird.birdMechanic;
                slotBird.birdMechanic.idCowInData = tempDoubleBird.idCowInData;
                slotBird.birdMechanic.transform.parent = tempDoubleBird.transform;
                slotBird.birdMechanic.animBird.SetOrderInLayer(2);
                slotBird.birdMechanic.animBird.SetAnim(slotBird.birdMechanic.animBird.FlY, true);
             //   RotateBirdRedo(slotBird.birdMechanic, tempDoubleBird.postFront);
                slotBird.birdMechanic.transform.DOMove(tempDoubleBird.postFront, 0.3f).OnComplete(delegate
                {
                    slotBird.birdMechanic.UnlockClick();
                    slotBird.birdMechanic.animBird.SetAnim(slotBird.birdMechanic.animBird.IDLE, true);
                });
                Debug.Log("birdInFront == null");
                return;
            }
        
           
        }

    }
    private void RotateBirdRedo(BirdMechanic birdMechanic,Vector3 paramTranform)
    {
        var tempLocalScaleBird = birdMechanic.animBird.transform.localScale;
        var tempLocalScaleBirdBeforAbs = System.Math.Abs(tempLocalScaleBird.x);
        //Debug.LogError(tempLocalScaleBirdBeforAbs);
        birdMechanic.animBird.transform.localScale = new Vector3(tempLocalScaleBirdBeforAbs, tempLocalScaleBird.y, tempLocalScaleBird.z);


        if (paramTranform.x < this.transform.position.x)
        {

            birdMechanic.animBird.transform.localScale = new Vector3(-tempLocalScaleBirdBeforAbs, tempLocalScaleBird.y, tempLocalScaleBird.z);

        }

    }
    #endregion
    #region BoosterSuport
    public void BoosterSuport()
    {
        if (lsLoockBooster.Count > 0)
        {
            return;
        }
        if (lsSlotBird.Count <= 0)
        {
            return;
        }
        bool wasHasTwoItem = false;
        foreach (var item in lsIdAndNumb)
        {
            if (item.numb == 2)
            {
                Debug.LogError("item.numb == 2");
                wasHasTwoItem = true;
                var tempInBack = FindBirdInBackNeedSuportDoubleBird(item.id);
                var tempInFront = FindBirdInFrontNeedSuportDoubleBird(item.id);

                if (tempInFront.Count > 0)
                {
                    tempInFront[0].OnClick();
                    Debug.LogError("co thang dang truoc");
                    break;
                    
                }

                if (tempInBack.Count > 0)
                {
                    tempInBack[0].HandleBirdBehindByBooster();
                    Debug.LogError("co thang dang sau ");
                    break;
                }

                if (tempInFront.Count == 0 && tempInBack.Count == 0)
                {
                    Debug.LogError("khong co thang nao tren ban do");
                    HandleNoSuitableBird(item.id);
                    break;
                }
            }
        }
        if (!wasHasTwoItem)
        {
            HandleNoTwoSuitableBird();
            Debug.LogError("chi co 1 thang");
        }


        Debug.LogError("BoosterSuport");
    }
    private void HandleNoTwoSuitableBird()
    {

        var tempListBirdSuport = FindBirdOneBird(lsSlotBird[0].birdMechanic.id);
        if(tempListBirdSuport.Count >= 2)
        {
            if(tempListBirdSuport[0].behindBird != null ) //dang truoc
            {
                tempListBirdSuport[0].OnClick();
            }
            else
            {
                tempListBirdSuport[0].HandleBirdBehindByBooster();
            }

            if (tempListBirdSuport[1].behindBird != null) //dang truoc
            {
                tempListBirdSuport[1].OnClick();
            }
            else
            {
                tempListBirdSuport[1].HandleBirdBehindByBooster();
            }
        }
        else
        {
            if(tempListBirdSuport.Count == 1)
            {
                if (tempListBirdSuport[0].behindBird != null) //dang truoc
                {
                    tempListBirdSuport[0].OnClick();
                }
                else
                {
                    tempListBirdSuport[0].HandleBirdBehindByBooster();
                }
                HandleNoSuitableBird(lsSlotBird[0].birdMechanic.id);
                Debug.LogError("Count = 1");
            }
          else
            {
                HandleNoSuitableBird(lsSlotBird[0].birdMechanic.id);
                HandleNoSuitableBird(lsSlotBird[0].birdMechanic.id);
                Debug.LogError("Count = 0");
            }
        }

    }
    [SerializeField] private List<BirdMechanic> TestFront;
    [SerializeField] private List<BirdMechanic> TestInBack;
    [SerializeField] private List<BirdMechanic> TestOneBird;
    private List<BirdMechanic> FindBirdOneBird(int idBird)
    {
        var dataDoubleBird = level.levelSpawn.levelData2.doubleBird;
        var lsBirdSuport = new List<BirdMechanic>();
        foreach (var item in dataDoubleBird)
        {
            if (item.birdInFront != null)
            {
                if (item.birdInFront.id == idBird)
                {
                    lsBirdSuport.Add(item.birdInFront);
                }
            }
            if (item.birdInTheBack != null)
            {
                if (item.birdInTheBack.id == idBird)
                {
                    lsBirdSuport.Add(item.birdInTheBack);
                }
            }
        }
        TestOneBird = lsBirdSuport;
        return lsBirdSuport;
    }
    private List<BirdMechanic> FindBirdInFrontNeedSuportDoubleBird(int idBird)
    {
        var dataDoubleBird = level.levelSpawn.levelData2.doubleBird;
        var lsBirdSuport = new List<BirdMechanic>();
        foreach (var item in dataDoubleBird)
        {
            if(item.birdInFront != null)
            {
                if (item.birdInFront.id == idBird)
                {
                    lsBirdSuport.Add(item.birdInFront);
                }
            }            
        }
        TestFront = lsBirdSuport;
        return lsBirdSuport;
    }
    private List<BirdMechanic> FindBirdInBackNeedSuportDoubleBird(int idBird)
    {
        var dataDoubleBird = level.levelSpawn.levelData2.doubleBird;
        var lsBirdSuport = new List<BirdMechanic>();
        foreach (var item in dataDoubleBird)
        {
            if (item.birdInTheBack != null)
            {
                if (item.birdInTheBack.id == idBird)
                {
                    lsBirdSuport.Add(item.birdInTheBack);
                }
            }
        }
        TestInBack = lsBirdSuport;
        return lsBirdSuport;
    }
    private void HandleNoSuitableBird(int idBird)
    {
        Debug.Log("HandleNoSuitableBird");
        foreach (var item in level.levelSpawn.levelData2.lsDataLevel)
        {
            for(int i = item.lsIdItem.Count -1 ; i >= 0  ; i--)
            {
                if(item.lsIdItem[i] == idBird)
                {
                    var CurrentScale = new Vector3();
                    var tempBird = SimplePool2.Spawn(level.levelSpawn.birdMechanic, level.levelSpawn.leftPost.position, Quaternion.identity);
                    tempBird.id = idBird;
                    tempBird.animBird = SimplePool2.Spawn(level.levelSpawn.GetAnimBird(idBird).animBird, level.levelSpawn.leftPost.position, Quaternion.identity);
                    tempBird.animBird.transform.SetParent(tempBird.transform);
                    tempBird.animBird.transform.localScale = new Vector3(tempBird.animBird.transform.localScale.x, tempBird.animBird.transform.localScale.y, tempBird.animBird.transform.localScale.z);
                    AddBirdToListSlot(tempBird);
                    item.lsIdItem.RemoveAt(i);
                    return;
                }
            }
        }
    }
  
    #endregion


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