using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class ItemBar : MonoBehaviour
{
    public int activeItem = 0;
    private List<Button> buttons = new List<Button>();
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int index = i;
            Button btn = transform.GetChild(i).gameObject.GetComponent<Button>();
            btn.onClick.AddListener(() => OnClick(index));
            buttons.Add(btn);
        }

    }

    private void OnClick(int index)
    {   
        this.activeItem = index;
        foreach (Button btn in buttons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
        buttons[index].GetComponent<Image>().color = Color.gray;
    }
}
