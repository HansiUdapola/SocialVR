using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    [System.Serializable]
    public class Tab
    {
        public GameObject tabMenu;
        public GameObject tabContent;
        public Color tabMenuDisableColor;
        public Color tabMenuEnableColor;
    }

    public List<Tab> TabList;   

    public void HideAllTabContents()
    {
        
        foreach(Tab x in TabList)
        {
            x.tabContent.SetActive(false);            
            x.tabMenu.GetComponent<Button>().image.color = x.tabMenuDisableColor;
        }
    }

    public void ShowTab(int i)
    {
        HideAllTabContents();
        TabList[i].tabContent.SetActive(true);
        TabList[i].tabMenu.GetComponent<Button>().image.color = TabList[i].tabMenuEnableColor;
    }
}
