using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
public class BirdMechanic : MonoBehaviour
{
    [HideInInspector] public int idCowInData;

    public int id;
   [HideInInspector] public int idElement;
    public bool wasLock;
    public BirdMechanic behindBird;
    public GameObject postBird;
    public AnimBird animBird;
    public BoxCollider2D boxCollider2;
    public SpriteRenderer note;
    public bool right;
    public int orderIndex;
    public SpriteRenderer dot;

    public void Init()
    {
        dot.color = new Color32(0, 0, 0, 0);
        var SpawnBird = Level.Instance.levelSpawn;
        var CurrentScale = new Vector3();
        id = SpawnBird.levelData2.GetDataLevel(idCowInData);
        animBird = SimplePool2.Spawn(SpawnBird.GetAnimBird(id).animBird, right ? SpawnBird.leftPost.position : SpawnBird.rightPost.position, Quaternion.identity);
        animBird.SetAnim(animBird.FlY, true);
        if (right)
        {
            CurrentScale = new Vector3(-animBird.transform.localScale.x, animBird.transform.localScale.y, animBird.transform.localScale.z);
        }
        else
        {
            CurrentScale = animBird.transform.localScale;
        }

        animBird.transform.SetParent(this.transform);
        animBird.transform.localScale = CurrentScale;

        animBird.transform.DOMove(postBird.transform.position, 1).OnComplete(
            delegate
            {
                animBird.transform.SetParent(postBird.transform);
                animBird.transform.localScale = CurrentScale;
                animBird.SetAnim(animBird.IDLE, true);
                animBird.transform.position = postBird.gameObject.transform.position;
            }
         );
        animBird.SetOrderInLayer(2);
        orderIndex = 2;

        if (behindBird != null)
        {
            behindBird.LockClick();
            behindBird.Init();
            behindBird.animBird.SetColor(true);
            behindBird.animBird.SetAnim(behindBird.animBird.IDLE, true);
            behindBird.animBird.SetOrderInLayer(1);
            behindBird.orderIndex = 1;

        }

    }

    public void OnMouseDown()
    {
        if(!wasLock)
        {
            OnClick();
        }
    }


    public void OnClick()
    {
        wasLock = true;
        UnlockClickBlockBehide();
        var controler = Level.Instance.levelLogic;
        controler.HandleCheckLose(this);
        controler.SortBird(this);
       // controler.lsLockDelete.Add(1);
        animBird.transform.position = postBird.gameObject.transform.position;   
        animBird.SetAnim(animBird.FlY, true);
        this.transform.DOMove(controler.GetPost(idElement).post.position, 1).OnComplete(delegate
        {
            //if (controler.lsLockDelete.Count > 0)
            //{
            //    controler.lsLockDelete.Remove(controler.lsLockDelete[0]);
            //}
            animBird.SetAnim(animBird.IDLE, true);
            controler.HandleDeleteBirds(this);
        });
    }

    public void UnlockClick()
    {
        wasLock = false;
        boxCollider2.enabled = true;
    }
    public void LockClick()
    {
        wasLock = true;
        boxCollider2.enabled = false;
    }
    public void UnlockClickBlockBehide()
    {
        var SpawnBird = Level.Instance.levelSpawn;
        
        if (behindBird != null )
        {

            var CurrentScale = new Vector3();
            var CurrentPossition = new Vector3();
            CurrentPossition = behindBird.transform.position;
            if (SpawnBird.levelData2.GetCountLsDataLevel(idCowInData) > 0)
            {
                behindBird.behindBird = SimplePool2.Spawn(SpawnBird.birdMechanic, CurrentPossition, Quaternion.identity);
                behindBird.behindBird.right = behindBird.right;        
                CurrentScale = behindBird.behindBird.transform.localScale;
                behindBird.behindBird.transform.SetParent(SpawnBird.levelData2.gameObject.transform);
                behindBird.behindBird.transform.localScale = CurrentScale;
                behindBird.behindBird.idCowInData = behindBird.idCowInData;
                behindBird.behindBird.Init();
                behindBird.behindBird.animBird.SetColor(true);
                behindBird.behindBird.orderIndex = behindBird.orderIndex;
                behindBird.behindBird.animBird.SetOrderInLayer(behindBird.orderIndex);
                behindBird.behindBird.LockClick();

            }
            behindBird.orderIndex = this.orderIndex;
            behindBird.animBird.SetOrderInLayer(this.orderIndex);
           
            behindBird.transform.DOJump(this.transform.position, 0.5f, 1, 0.5f).OnComplete(delegate {
                behindBird.UnlockClick();
            });
            behindBird.animBird.SetColor(false);

        }
    }
    private void OnDisable()
    {
        //Level.Instance.levelSpawn.levelData2.sumBird -= 1;
        //Level.Instance.levelLogic.HandleWin();
    }
}
