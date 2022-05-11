using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickUp : MonoBehaviour
{
    // Description:  Put this script on the trigger collider in front of the camea to make the player be able to pick items tagged with "ItemPickUp" up (in the inventory).

    //
    // IMPORTANT: Make sure to add the PickUp() function to the PlayerInput component at the PickUp action.
    //

    private GameObject currentTarget;

    private void OnTriggerEnter(Collider other)
    {
        // If theres pickable object colliding with the pick up collider, then set the currentTarget to that object.

        if(other.gameObject.tag == "ItemPickUp")
        {
            WorldItem worldItem;
            if(other.gameObject.TryGetComponent<WorldItem>(out worldItem))
            {
                currentTarget = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == currentTarget)
        {
            currentTarget = null;
        }
    }

    public void PickUp(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(currentTarget != null)
            {
                Item item = currentTarget.GetComponent<WorldItem>().GetItem();  // Getting the WorldItem script of the Item that should be picked up
                Inventory.Instance.AddItem(item);                               // Adding the item to the inventory
                currentTarget.GetComponent<WorldItem>().Destroy();              // Destroying the item in the world (using the worlditem.destory() function because of saving)
                currentTarget = null;                                           // setting the currentTarget to null because the item has been destroyed anyway. (you cant pick an item that has been destoryed up.
            }
        }
    }
}
