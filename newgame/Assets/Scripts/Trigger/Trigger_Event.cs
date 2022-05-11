using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger_Event : Trigger
{
    [SerializeField] private UnityEvent Function;

    public override void OnTrigger()
    {
        Function.Invoke();
    }
}
