using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideQuestManager : MonoBehaviour
{
    public static SideQuestManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public SideQuest currentSideQuest;
    public bool active;
    
    public void SetSideQuest(SideQuest sideQuest)
    {
        currentSideQuest = sideQuest;
    }
    public SideQuest GetActiveSideQuest()
    {
        return currentSideQuest;
    }
    public bool Active()
    {
        return active;
    }
    public void SetActive(bool active)
    {
        this.active = active;
    }
}