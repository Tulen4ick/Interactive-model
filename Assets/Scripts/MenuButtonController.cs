using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    public Transform panel;
    public void OnOffUI ()
    {
        if (panel != null)
        {
            if (!panel.gameObject.activeInHierarchy)
            {
                panel.gameObject.SetActive(true);
            }else
            {
                panel.gameObject.SetActive(false);
            }
            
        }
    }
}
