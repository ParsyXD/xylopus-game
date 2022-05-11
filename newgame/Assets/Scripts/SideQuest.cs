using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideQuest
{
    public enum SideQuestTypes
    {
        getItem,
        killEnemy
    }

    private SideQuestTypes type;
    private float money;




    public void SetQuestType(SideQuestTypes type)
    { 
        this.type = type; 
    }
    public SideQuestTypes GetSideQuestType()
    {
        return this.type;
    }

    public void SetMoney(float money)
    {
        this.money = money;
    }
    public float GetMoney()
    {
        return this.money;
    }
}
