using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[RequireComponent(typeof(Text))]
public class InputFieldSaveTest : MonoBehaviour
{
    // The UI object used to display the current data
    private Text text;

    // The key used for associating the data being saved or loaded in the PlayerPrefs
    private string saveKey = "SavedData";

    // The path local to the persistent data path we will use to save in binary
    static private string localPath = "/savePOC.poc";

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();

        if (PlayerPrefs.HasKey(saveKey))
            text.text = PlayerPrefs.GetString(saveKey);
        else
            text.text = "I did not load anything...";
    }

    // Simple method for changing data
    public void SetTextYes()
    {
        text.text = "Yes";
    }

    // Simple method for changing data
    public void SetTextNo()
    {
        text.text = "No";
    }

    // Saves the text data to the PlayerPrefs
    // The PlayerPrefs Unity object can be used to save and load string, ints and floats
    // Each data point saved or loaded is associated with a key in the form of a string
    // Use saving to PlayerPrefs for simple/small data like option menu states, current level number, player name, etc.
    public void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetString(saveKey, text.text);
    }

    // Loads the text data from the PlayerPrefs, if it exists
    public void LoadFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(saveKey))
            text.text = PlayerPrefs.GetString(saveKey);
        else
            text.text = "There is nothing to load, try saving first";
    }

    // Saves the text data as a serializable object to a binary file
    // All data being saved must either be a basic type (int, string, float, enum...)
    // or a built in type from Unity such as Vector2/3/4 Quaternion or Matrix4x4
    // or a class inheriting from Object or another class marked with "[System.Serializable]"
    // whose internal members meet these requirements.
    public void SaveBinary()
    {
        SerializableData serData  = new SerializableData();
        serData.Data = text.text;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + localPath);
        formatter.Serialize(file, serData);
        file.Close();
    }

    public void LoadBinary()
    {
        if (File.Exists(Application.persistentDataPath + localPath))
        {
            SerializableData serData = new SerializableData();
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + localPath, FileMode.Open);
            serData = (SerializableData)formatter.Deserialize(file);
            file.Close();

            text.text = serData.Data;
        }
    }
}
