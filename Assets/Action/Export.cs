using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class Export : MonoBehaviour
{
    private StringBuilder data =
        new StringBuilder();

    private float time;

    [SerializeField]
    private Allmetrics metrics;

    private void Start()
    {
        data.AppendLine(
            "Время;Население;Мощность;Эффективность;Энтропия;Задержка"
        );
    }

    private void Update()
    {
        if (metrics == null)
            return;

        time += Time.deltaTime;

        if (time >= 1f)
        {
            time = 0f;

            int population =
                FindObjectsOfType<OrganismView>().Length;

            data.AppendLine(
                $"{Time.time:F1};" +
                $"{population};" +
                $"{metrics.Power:F1};" +
                $"{metrics.Efficiency:F1};" +
                $"{metrics.Entropy:F1};" +
                $"{metrics.ResponseDelay:F1}"
            );
        }
    }

    public void Exp()
    {
        string folderPath =
            Path.Combine(
                Application.persistentDataPath,
                "Exports"
            );

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fileName =
            $"SimulationResult_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";

        string path =
            Path.Combine(
                folderPath,
                fileName
            );

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