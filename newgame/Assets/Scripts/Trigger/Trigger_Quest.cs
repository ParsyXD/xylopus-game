using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trigger_Quest : Trigger
{
    [SerializeField] private string sceneName;
    public override void OnTrigger()
    {
        if (SideQuestManager.Instance.Active())
        {
            base.abortDestroy = true;
            UI.Instance.ShowTooltip("Du musst erst deine Nebenquest abschlieﬂen");
            Debug.Log("Tooltip Showing");
            return;
        }

        base.Destroy(true);
        base.baseManager.saveGame.Save();
        base.baseManager.LoadScene(sceneName);
    }

}
