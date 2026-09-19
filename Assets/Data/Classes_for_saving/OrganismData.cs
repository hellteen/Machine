using System;

[Serializable]
public class OrganismData
{
	public int id;
	public PositionData position;
	public StateData state;
	public BehaviorData behavior;
	public SensorData sensors;
	public MemoryData memory;
}

[Serializable]
public class PositionData
{
	public float x;
	public float y;
	public float z;
}

[Serializable]
public class StateData
{
	public float health;
	public float energy;
	public float age;
	public float hunger;
}

[Serializable]
public class BehaviorData
{
	public float movement;
	public float resourceSeeking;
	public float reproduction;
	public float waiting;
}

[Serializable]
public class SensorData
{
	public float visionRange;
	public float temperatureSensitivity;
	public float radiationSensitivity;
	public float resourceSensitivity;
}

[Serializable]
public class MemoryData
{
	public int capacity;
}