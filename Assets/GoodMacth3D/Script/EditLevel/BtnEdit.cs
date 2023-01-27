using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BtnEdit : MonoBehaviour
{
    public int id;
    public Text tvBtn;
    private void Start()
    {
        id = 0;
        tvBtn.text = "" + id;

    }
    public void OnClick()
    {
        id += 1;
        tvBtn.text = "" + id;
        if(id > 10)
        {
            id = 0;
            tvBtn.text = "" + id;
        }
    }
}
