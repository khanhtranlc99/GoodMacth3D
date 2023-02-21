using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BlackPanel : MonoBehaviour
{
    public Image blackPanel;
    public LevelLogic level;
    public void OnBlackPanel()
    {
        if(level.lsLoockBooster.Count > 0)
        {
            return;
        }
        blackPanel.gameObject.SetActive(true);
        blackPanel.fillAmount = 1;
        blackPanel.DOFillAmount(0, 1).OnComplete(delegate { blackPanel.gameObject.SetActive(false); });
    }
}
