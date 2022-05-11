using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class UI : MonoBehaviour
{
    /*
     * This Class is for handeling the UI systems like Tooltips, Displaying the Inventory, achievments and more.
     * 
     * To use it simply write UI.Instance and then the method you wish to call in any script.
     */ 

    public static UI Instance { get; private set; }


    // private variables
    private Player player;
    private FirstPersonMovement firstPersonMovement;
    private FirstPersonLook firstPersonLook;
    private bool inPauseMenu = false;


    // UI Elements
    [Header("Inventory")]
    [SerializeField] private GameObject InventoryHolder;    // Beeing able to deactivate the inventory contents while the UIInventory object is still active
    [SerializeField] private TextMeshProUGUI prototype_sphere_amount;
    [SerializeField] private TextMeshProUGUI prototype_cube_amount;
    [SerializeField] private TextMeshProUGUI money_amount;
    [Header("PauseMenu")]
    [SerializeField] private GameObject pauseMenuHolder;
    [Header("Other")]
    [SerializeField] private TextMeshProUGUI ToolTips;
    [SerializeField] private Animator tooltipsAnimator;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = Player.Instance;
        HideInventory();
    }


    /*
     * ====================
     *    Public Methods
     * ====================
     */

    // General


    // Inventory
    public void TriggerInventoryVisibility(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (InventoryHolder.activeSelf)
            {
                HideInventory();
            }
            else if(!inPauseMenu)
            {
                ShowInventory();
            }
        }
    }
    public void ShowInventory()
    {
        RefreshInventory();
        InventoryHolder.SetActive(true);
        player.SetControllsActive(false);
    }
    public void HideInventory()
    {
        InventoryHolder.SetActive(false);
        player.SetControllsActive(true);
    }
    public void RefreshInventory()
    {
        prototype_cube_amount.text = Inventory.Instance.content.prototype_cube.ToString() + "/" + Inventory.Instance.prototype_cube_max.ToString();
        prototype_sphere_amount.text = Inventory.Instance.content.prototype_sphere.ToString() + "/" + Inventory.Instance.prototype_sphere_max.ToString();
        money_amount.text = Inventory.Instance.content.money.ToString();
    }


    // Pause Menu
    public void TriggerPausemenuVisibility(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (pauseMenuHolder.activeSelf)
            {
                HidePausmenu();
            }
            else
            {
                HideInventory();    //This is so that you cant have the inventory and the pause menu open at the same time
                ShowPausemenu();
            }
        }

        if(pauseMenuHolder.activeSelf)  // Pausing the Physics when in pause menu
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void ShowPausemenu()
    {
        inPauseMenu = true;
        pauseMenuHolder.SetActive(true);
        player.SetControllsActive(false);
    }
    public void HidePausmenu()
    {
        inPauseMenu = false;
        pauseMenuHolder.SetActive(false);
        player.SetControllsActive(true);
    }



    // Tooltips
    public void ShowTooltip(string text, float time = 3f)
    {
        tooltipsAnimator.Play("tooltips_in");
        ToolTips.text = text;
        Invoke("HideTooltip", time);
        
    }
    public void HideTooltip()
    {
        tooltipsAnimator.Play("tooltips_out");
    }
    
}
