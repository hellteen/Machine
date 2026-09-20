using System.Collections.Generic;

public static class SimulationEvents
{
    private static Queue<string> events =
        new Queue<string>();

    public static void Add(string message)
    {
        events.Enqueue(message);

        // Сейвим последние 20 событий
        while (events.Count > 20)
        {
            events.Dequeue();
        }
    }

    public static string GetNext()
    {
        if (events.Count == 0)
        {
            return null;
        }

        return events.Dequeue();
    }

    public static int Count
    {
        get
        {
            return events.Count;
        }
    }

    public static void Clear()
    {
        events.Clear();
    }
}