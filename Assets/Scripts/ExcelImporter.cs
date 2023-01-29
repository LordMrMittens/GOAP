using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class ExcelImporter : MonoBehaviour
{
    public TextAsset[] csvFile;
    public string jsonFileName = "csvData.json";
    public Dictionary<string, List<string>> text = new Dictionary<string, List<string>>();
    //public List<string> names = new List<string>();
    private void Awake()
    {
        for (int i = 0; i < csvFile.Length; i++)
        {
            if (csvFile[i] != null)
            {
                List<string> names = ReadCsv(csvFile[i].text);
                text.Add(csvFile[i].name, names);
            }
        }
        if (text.ContainsKey("Names")){
            foreach (string item in text["Names"])
            {
                //Debug.Log(item);
            }
        }
    }

    private List<string> ReadCsv(string csvText)
    {
        string[] lines = csvText.Split('\n');
        List<string> csvData = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            string row = lines[i];
            if (!string.IsNullOrEmpty(row))
            {
                csvData.Add(row);
            }
        }

        return csvData;
    }

}
