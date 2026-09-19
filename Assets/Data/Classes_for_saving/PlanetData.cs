using System;
using System.Collections.Generic;

[Serializable]
public class PlanetData
{
    public int id;
    public string name;
    public string description;

    // Текущие параметры окружающей среды.
    public EnvironmentData environment;

    // На планете теперь может быть много организмов.
    public List<OrganismData> organisms;
}

[Serializable]
public class EnvironmentData
{
    public float temperature;
    public float radiation;
    public float pressure;

    // Количество доступных ресурсов.
    public float resources;

    // Общая энергия среды.
    public float energy;

    // Скорость восстановления ресурсов.
    public float resourceRegeneration;
}