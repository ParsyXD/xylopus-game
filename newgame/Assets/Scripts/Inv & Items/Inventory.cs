using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    // Amount of the items
    public class Content
    {
        public int prototype_sphere;
        public int prototype_cube;
        public int money;
    }
    public Content content = new Content();

    // Max Amounts of the items (set this in inspector)
    public int prototype_sphere_max;
    public int prototype_cube_max;


    private void Awake()
    {
        Instance = this;
    }


    public bool AddItem(Item item)
    {
        bool returning = false;
        switch(item.itemType)
        {
            case Item.itemTypes.prototype_sphere:
                if(content.prototype_sphere + item.amount <= prototype_sphere_max)
                {
                    content.prototype_sphere += item.amount;
                    returning = true;
                }
                break;

            case Item.itemTypes.prototype_cube:
                if (content.prototype_cube + item.amount <= prototype_cube_max)
                {
                    content.prototype_cube += item.amount;
                    returning = true;
                }
                break;
            case Item.itemTypes.money:
                content.money += item.amount;
                returning = true;
                break;
        }

        return returning;
    }

    public void LoadInventory(Content content)
    {
        this.content.prototype_sphere = content.prototype_sphere;
        this.content.prototype_cube = content.prototype_cube;
        this.content.money = content.money;
    }
}
