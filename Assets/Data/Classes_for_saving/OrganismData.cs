using System;

[Serializable]
public class OrganismData
{
    public int id;

    // Положение организма в мире.
    public PositionData position;

    // Текущее состояние организма.
    public StateData state;

    // Наследуемые характеристики.
    // Они не являются "мозгом".
    // Это просто набор генов.
    public GenomeData genome;

    // Органы восприятия среды.
    public SensorData sensors;

    // Простая память.
    public MemoryData memory;

    public DirectionData direction;
}

[Serializable]
public class DirectionData
{
    public float x;
    public float y ;
}


// -----------------------------------------
// ПОЛОЖЕНИЕ
// -----------------------------------------

[Serializable]
public class PositionData
{
    public float x;
    public float y;
    public float z;
}


// -----------------------------------------
// СОСТОЯНИЕ
// -----------------------------------------

[Serializable]
public class StateData
{
    public float health;
    public float energy;
    public float age;
    public float hunger;
}


// -----------------------------------------
// ГЕНОМ
// -----------------------------------------

[Serializable]
public class GenomeData
{
    // Насколько организм склонен к движению.
    public float movement;

    // Насколько организм склонен искать ресурсы.
    public float resourceSeeking;

    // Насколько организм склонен размножаться.
    public float reproduction;

    // Насколько организм склонен ждать.
    public float waiting;

    // Устойчивость к радиации.
    public float radiationResistance;

    // Устойчивость к холоду.
    public float coldResistance;

    // Склонность исследовать окружающую среду.
    public float exploration;
}


// -----------------------------------------
// ДАТЧИКИ
// -----------------------------------------

[Serializable]
public class SensorData
{
    public float visionRange;

    public float temperatureSensitivity;

    public float radiationSensitivity;

    public float resourceSensitivity;
}


// -----------------------------------------
// ПАМЯТЬ
// -----------------------------------------

[Serializable]
public class MemoryData
{
    public int capacity;
}