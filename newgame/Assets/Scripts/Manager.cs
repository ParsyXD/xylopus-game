using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }

    [HideInInspector] public Trigger lastTriggeredTrigger;
    public SaveGame saveGame;
    [SerializeField] [Tooltip("Is this a world, that the Player can walk around in?")] private bool thisIsGameWorld;
    [Header("Player Music On Enable")]
    [SerializeField] private bool playMusicOnEnable;
    public grumbleAMP MusicManager;
    public int songNumber;
    public int layerNumber;
    [Header("Scene Names")]
    [SerializeField] private string mainScene;
    [SerializeField] private string questOne;

    private GameObject player;
    public enum scenes
    {
        main,
        quest1
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = Player.Instance.gameObject;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Playmusic(bool ignorePlayOnEnableSetting = false)
    {
        if (playMusicOnEnable || ignorePlayOnEnableSetting)
        {
            MusicManager.PlaySong(songNumber, layerNumber);
        }
    }
}
