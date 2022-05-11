using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour
{


    public void Save()
    {
        SaveGameSystem.MainSaveObject saveObject = new SaveGameSystem.MainSaveObject();

        saveObject.inv_prototypeCubes = Inventory.Instance.content.prototype_cube;
        saveObject.inv_prototypeSpheres = Inventory.Instance.content.prototype_sphere;
        saveObject.inv_money = Inventory.Instance.content.money;

        saveObject.playerPosition = Player.Instance.transform.position;
        saveObject.playerRotation = Player.Instance.transform.rotation;

        SaveGameSystem.Save(SaveGameSystem.saveDirectory + "save", saveObject);



        SaveGameSystem.WorldSpecificSaveObject worldSpecificSaveObject = new SaveGameSystem.WorldSpecificSaveObject();

        worldSpecificSaveObject.destroyedItems = SaveGameSystem.Instance.destroyedItems;
        worldSpecificSaveObject.destroyedTriggers = SaveGameSystem.Instance.destroyedTriggers;

        worldSpecificSaveObject.music_song = Manager.Instance.MusicManager.getCurrentSongNumber();
        worldSpecificSaveObject.music_layer = Manager.Instance.MusicManager.getCurrentLayerNumber();

        SaveGameSystem.Save(SaveGameSystem.saveDirectory + SceneManager.GetActiveScene().name.Replace(" ", ""), worldSpecificSaveObject);
    }

    private void Start()
    {
        Load();
    }

    private void Load()
    {
        SaveGameSystem.MainSaveObject mainSaveObject = new SaveGameSystem.MainSaveObject();
        mainSaveObject = SaveGameSystem.Instance.Load<SaveGameSystem.MainSaveObject>(SaveGameSystem.saveDirectory + "save");

        SaveGameSystem.WorldSpecificSaveObject worldSpecificSaveobject = new SaveGameSystem.WorldSpecificSaveObject();
        worldSpecificSaveobject = SaveGameSystem.Instance.Load<SaveGameSystem.WorldSpecificSaveObject>(SaveGameSystem.saveDirectory + SceneManager.GetActiveScene().name);

        Inventory.Content inv = new Inventory.Content();                        // Creating an inventory with the values of the saveObject
        inv.prototype_cube = mainSaveObject.inv_prototypeCubes;
        inv.prototype_sphere = mainSaveObject.inv_prototypeSpheres;
        inv.money = mainSaveObject.inv_money;


        // Applying the values
        Inventory.Instance.LoadInventory(inv);
        Player.Instance.transform.position = mainSaveObject.playerPosition;
        Player.Instance.transform.rotation = mainSaveObject.playerRotation;


        Manager.Instance.songNumber = worldSpecificSaveobject.music_song;
        Manager.Instance.layerNumber= worldSpecificSaveobject.music_layer;
        Manager.Instance.Playmusic();
        SaveGameSystem.Instance.destroyedItems = worldSpecificSaveobject.destroyedItems;
        SaveGameSystem.Instance.destroyedTriggers = worldSpecificSaveobject.destroyedTriggers;
    }
}
