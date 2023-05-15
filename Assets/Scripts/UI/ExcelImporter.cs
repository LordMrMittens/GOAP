using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class ExcelImporter : MonoBehaviour
{
    public TextAsset[] csvFile;
    public Dictionary<string, List<string>> text = new Dictionary<string, List<string>>();
    public Dictionary<string, int> genderData = new Dictionary<string, int>();
    public static ExcelImporter textImporterInstance;

    private void Awake()
    {
        textImporterInstance = this;
        for (int i = 0; i < csvFile.Length; i++)
        {
            if (csvFile[i] != null)
            {
                List<string> names = ReadCsv(csvFile[i].text);
                text.Add(csvFile[i].name, names);
                if(csvFile[i].name == "Names"){
                ReadCsvGender(csvFile[i].text);
                Debug.Log(csvFile[i].name);}
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
                string[] columns = row.Split(',');
                if (columns.Length >= 1)
                {
                    string name = columns[0];
                    csvData.Add(name);
                }
            }
        }

        return csvData;
    }

    private void ReadCsvGender(string csvText)
    {
        
        string[] lines = csvText.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string row = lines[i];
            if (!string.IsNullOrEmpty(row))
            {
                string[] columns = row.Split(',');
                if (columns.Length >= 2)
                {
                    string name = columns[0];
                    int gender;
                    if (int.TryParse(columns[1], out gender))
                    {
                        genderData.Add(name, gender);
                    }
                    else
                    {
                        Debug.LogWarning($"Invalid Gender Format for name: {name}");
                    }

                }
            }
        }
    }

}
