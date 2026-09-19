using System;

[Serializable]
public class PlanetData
{
    public int id;
    public string name;
    public string description;
    public EnvironmentData environment;
    public OrganismData organism;
}

[Serializable]
public class EnvironmentData
{
    public float temperature;
    public float radiation;
    public float pressure;
    public float resources;
    public float energy;
    public float resourceRegeneration;
}