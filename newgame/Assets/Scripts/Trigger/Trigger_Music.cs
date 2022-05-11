using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Music : Trigger
{
    [SerializeField] private grumbleAMP MusicManager;
    [Space]
    [SerializeField] private int SongNumber;
    [SerializeField] private int LayerNumber;
    [Space]
    [SerializeField] private bool stopPlaying;
    [Space]
    [SerializeField] private float crossfadeTime;

    public override void OnTrigger()
    {
        if (!stopPlaying)
        {
            if (MusicManager.isPlaying())
            {
                MusicManager.CrossFadeToNewSong(SongNumber, LayerNumber, crossfadeTime);
            }
            else
            {
                MusicManager.PlaySong(SongNumber, LayerNumber, crossfadeTime);
            }
        }
        else
        {
            MusicManager.StopAll(crossfadeTime);
        }
    }
}
