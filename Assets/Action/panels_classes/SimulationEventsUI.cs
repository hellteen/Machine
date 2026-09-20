using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimulationEventsUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text eventsText;

    [SerializeField]
    private float updateInterval = 0.7f;

    private float timer;

    private List<string> displayedEvents =
        new List<string>();

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= updateInterval)
        {
            timer = 0f;

            UpdateEvents();
        }
    }
    //private void UpdateEvents()
    //{
    //    string newEvent = SimulationEvents.GetNext();

    //    if (newEvent == null)
    //    {
    //        return;
    //    }

    //    Debug.Log("UI EVENT: " + newEvent);

    //    displayedEvents.Add(newEvent);

    //    while (displayedEvents.Count > 7)
    //    {
    //        displayedEvents.RemoveAt(0);
    //    }

    //    eventsText.text = string.Join(
    //        "\n",
    //        displayedEvents
    //    );
    //}
    private void UpdateEvents()
    {
        string newEvent =
            SimulationEvents.GetNext();

        if (newEvent == null)
        {
            return;
        }

        displayedEvents.Add(newEvent);

        // Оставляем только последние 7 сообщений
        while (displayedEvents.Count > 7)
        {
            displayedEvents.RemoveAt(0);
        }

        eventsText.text =
            string.Join(
                "\n",
                displayedEvents
            );
    }
}