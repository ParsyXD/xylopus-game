using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class WorldItem : MonoBehaviour
{
    // Add this script to an Item in the world that the player should be able to pick up (configure amount and type in inspector)


    [SerializeField] private Item.itemTypes ItemType;
    [SerializeField] private int amount;
    [SerializeField] private bool hasId = false;
    [SerializeField] private int ItemId;

    public Item GetItem()
    {
        return new Item { itemType = this.ItemType, amount = this.amount };
    }

    private void Start()
    {
        // If The list with the gameobjects, that have been picked up contains this items id, then destory this item.
        // (it has been picked up so it shouldn't be in the world anymore.
        try
        {
            if (SaveGameSystem.Instance.Load<SaveGameSystem.WorldSpecificSaveObject>(SaveGameSystem.saveDirectory + SceneManager.GetActiveScene().name).destroyedItems.Contains(ItemId))
            {
                Destroy(gameObject);
            }
        }
        catch { }
    }

    public void Destroy()
    {
        // This is for destroying the object and adding its id to the destroyedItems list in the SaveGameSystem so
        // that it can be saved, which items have been picked up and are therefor no longer in the world
        SaveGameSystem.Instance.destroyedItems.Add(ItemId);
        Destroy(gameObject);
    }

    public void GetItemId()
    {
        // This function gives every item in the world a unique id, so that it can be saved, which items have been picked
        // up and are therefor no longer in the world

        GameObject[] objects = GameObject.FindGameObjectsWithTag("ItemPickUp");
        int i = 0;
        List<int> existingIds = new List<int>();
        foreach(GameObject obj in objects)
        {
            WorldItem component;
            if(obj.TryGetComponent<WorldItem>(out component))
            {
                if (!component.hasId)
                {
                    while(existingIds.Contains(i))
                    {
                        i++;
                    }
                    component.ItemId = i;
                    component.hasId = true;
                }
                else
                {
                    existingIds.Add(component.ItemId);
                }
            }
            i++;
        }
    }
}
