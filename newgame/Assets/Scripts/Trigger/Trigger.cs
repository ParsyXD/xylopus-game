using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(DrawCollider))]
public class Trigger : MonoBehaviour
{
    // Base class for all Trigger variations

    public Manager baseManager;
    [HideInInspector] public bool abortDestroy;
    [SerializeField][Tooltip("Should the Trigger destroy itself (forever) after beeing triggered once? (useful for quest triggers)")]bool destroyAfterTriggered;
    public bool hasId;
    public int triggerId;

    private void Start()
    {
        baseManager = Manager.Instance;
        Invoke("Load", 0.1f);
    }
    private void Load()
    {
        if (destroyAfterTriggered && SaveGameSystem.Instance.Load<SaveGameSystem.WorldSpecificSaveObject>(SaveGameSystem.saveDirectory + SceneManager.GetActiveScene().name).destroyedTriggers.Contains(triggerId))
        {
            Debug.Log("Destroying " + gameObject.name + "...");
            Object.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player.Instance.gameObject)
        {
            if (baseManager == null)
            {
                baseManager = Manager.Instance;
            }
            Trigger lastTrigger = null;
            if (baseManager.lastTriggeredTrigger != null)
            {
                lastTrigger = baseManager.lastTriggeredTrigger;
            }

            if (lastTrigger != this)
            {
                OnTrigger();
                baseManager.lastTriggeredTrigger = this;
                if (destroyAfterTriggered)
                {
                    Destroy();
                }
            }
        }
    }

    public virtual void OnTrigger() 
    {
        baseManager.lastTriggeredTrigger = this;
    }

    public void Destroy(bool onlyAddToDestroyedList = false)
    {
        // This is for destroying the object and adding its id to the destroyedTriggers list in the SaveGameSystem so
        // that it can be saved, which triggers have been triggered and are therefor no longer in the world
        if (!abortDestroy)
        {
            SaveGameSystem.Instance.destroyedTriggers.Add(triggerId);
            if (!onlyAddToDestroyedList)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            abortDestroy = true;
        }
    }

    [ContextMenu("GetTriggerID")]
    public void GetTriggerID()
    {
        // This function gives every item in the world a unique id, so that it can be saved, which items have been picked
        // up and are therefor no longer in the world

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Trigger");
        int i = 0;
        List<int> existingIds = new List<int>();
        foreach (GameObject obj in objects)
        {
            Trigger component;
            if (obj.TryGetComponent<Trigger>(out component))
            {
                if (component.destroyAfterTriggered)
                {
                    if (!component.hasId)
                    {
                        while (existingIds.Contains(i))
                        {
                            i++;
                        }
                        component.triggerId = i;
                        component.hasId = true;
                    }
                    else
                    {
                        existingIds.Add(component.triggerId);
                    }
                }
            }
            i++;
        }
    }
}
