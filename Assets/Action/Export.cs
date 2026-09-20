using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class Export : MonoBehaviour
{
    private StringBuilder data = new StringBuilder();

    private float time;

    private void Start()
    {
        
        data.AppendLine(
            "Время;Население;Мощность;Эффективность;Энтропия"
        );
    }

    private void Update()
    {
        time += Time.deltaTime;

        
        if (time >= 1f)
        {
            time = 0f;

            int population =
                FindObjectsOfType<OrganismView>().Length;

            // ВРЕМЕННО:
            float power = population * 0.5f;
            float efficiency = Random.Range(60f, 90f);
            float entropy = Random.Range(20f, 50f);

            data.AppendLine(
                $"{Time.time:F1};" +
                $"{population};" +
                $"{power:F1};" +
                $"{efficiency:F1};" +
                $"{entropy:F1}"
            );
        }
    }

    public void Exp()
    {
        
        string folderPath =
            Path.Combine(Application.persistentDataPath, "Exports");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        
        string fileName =
            $"SimulationResult_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";

        string path =
            Path.Combine(folderPath, fileName);

      
        string csv =
            "\uFEFF" + data.ToString();

        File.WriteAllText(
            path,
            csv,
            Encoding.UTF8
        );

        UnityEngine.Debug.Log(
            $"Экспорт завершён: {path}"
        );

        
        OpenFile(path);
    }

    private void OpenFile(string path)
    {
        try
        {
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                }
            );
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(
                $"Не удалось открыть файл: {e.Message}"
            );
        }
    }
}