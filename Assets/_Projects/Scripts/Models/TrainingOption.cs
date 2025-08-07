using System;
using Scripts.Setting;

public class TrainingOption
{
    public string Name { get; }
    public TrainingType Type { get; }

    public TrainingOption(string name, TrainingType type)
    {
        Name = name;
        Type = type;
    }
}
