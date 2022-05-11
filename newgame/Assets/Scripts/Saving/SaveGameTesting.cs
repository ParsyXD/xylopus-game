using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameTesting : MonoBehaviour
{
    SaveGameSystem saveGame;

    private void Start()
    {
        saveGame = SaveGameSystem.Instance;
    }

    [ContextMenu("TestSave")]
    public void TestSave()
    {
        TestObject testObject = new TestObject
        {
            variable_int = 4,
            variable_vector3 = transform.forward,
            variable_color = new Color(0.4f, 0.1f, 0.745f, 0.0f),
            list_string = new List<string> { "string 1", "string 2", "string 3" }
        };

        SaveGameSystem.Save("saves/testsave", testObject);
    }

    private class TestObject
    {
        public int variable_int;
        public Vector3 variable_vector3;
        public Color variable_color;
        public List<string> list_string;
    }
}
