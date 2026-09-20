using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using Random = UnityEngine.Random;
using Application = UnityEngine.Application;

public class Export: MonoBehaviour
{
    StringBuilder data = new StringBuilder();

    float time;


    void Start()
    {
        data.AppendLine(
            "Time,Population,Power,Efficiency,Entropy"
        );
    }


    void Update()
    {
        time += Time.deltaTime;


        if (time >= 1f)
        {
            time = 0;

            int population =
                FindObjectsOfType<OrganismView>().Length;


            float power = population * 0.5f;
            float efficiency = Random.Range(60, 90);
            float entropy = Random.Range(20, 50);


            data.AppendLine(
                $"{Time.time:F1},{population},{power:F1},{efficiency:F1},{entropy:F1}"
            );
        }
    }


    public void Exp()
    {
        string path =
            Application.dataPath + "/SimulationResult.csv";


        File.WriteAllText(
            path,
            data.ToString()
        );

    }
}