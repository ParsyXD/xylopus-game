using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum itemTypes
    {
        prototype_sphere,
        prototype_cube,
        money
    }

    public itemTypes itemType;
    public int amount;
}
