using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;

[System.Serializable]
public class DrawingData
{
    public List<float> drawing;
}
[System.Serializable]
public class ResultsData
{
    public List<float> outputs;
}

public class NNCommunicator : MonoBehaviour
{
    private string drawingPath = "C:/Users/dyrek/OneDrive/Documents/Code Projects/Doodle Neural Network/Project/drawing.json";
    private string resultsPath = "C:/Users/dyrek/OneDrive/Documents/Code Projects/Doodle Neural Network/Project/results.json";
    private List<string> categories = new List<string> { "anvil", "book", "door", "hat", "lollipop", "rake" };
    public string prevState = "";
    public CanvasDrawer canvasDrawer;
    public TextMeshProUGUI topGuesses;
    public bool communicateWithNN;

    void Update()
    {
        if(communicateWithNN == false){return;}

        string currentState = getDrawingState(canvasDrawer);
        if (currentState == prevState)
        {
            return;
        }

        List<GameObject> tiles = canvasDrawer.tiles;
        List<float> drawing = new List<float>();
        foreach (GameObject tile in tiles)
        {
            drawing.Add(tile.GetComponent<SpriteRenderer>().color.r * 255.0f);
        }

        DrawingData drawingData = new DrawingData { drawing = drawing };
        string drawingJSON = JsonUtility.ToJson(drawingData);

        File.WriteAllText(drawingPath, drawingJSON);

        prevState = currentState;

        //
        string stringData = null;
        try
        {
            stringData = File.ReadAllText(resultsPath);
        }
        catch (IOException ex)
        {
            Debug.Log("File reading error: " + ex);
        }

        if(stringData == null)
        {
            return;
        }

        var jsonObject = JsonConvert.DeserializeObject<JObject>(stringData);

        List<float> results = new List<float>();
        List<string> resultNames = new List<string>();
        foreach (var kvp in jsonObject)
        {
            results.Add((float)kvp.Value);
            resultNames.Add((string)kvp.Key);
        }

        int i = 0;
        string finalOutput = "";
        foreach(float val in results)
        {
            string displayVal = (val * 100f).ToString();
            string displayName = resultNames[i];
            if(i==0)
            {
                finalOutput += "<color=green>" + (i+1).ToString() + ". " + displayName + " - " + displayVal + "%</color>\n";
            }
            else
            {
                finalOutput += (i+1).ToString() + ". " + displayName + " - " + displayVal + "%\n";
            }
            i++;
        }
        topGuesses.text = finalOutput;
    }

    string getDrawingState(CanvasDrawer canvasDrawer)
    {
        string state = "";
        List<GameObject> tiles = canvasDrawer.tiles;
        foreach (GameObject tile in tiles)
        {
            state += tile.GetComponent<SpriteRenderer>().color.r.ToString();
        }
        return state;
    }
}