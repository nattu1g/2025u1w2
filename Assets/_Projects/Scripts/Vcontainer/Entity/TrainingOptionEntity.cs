using System.Collections.Generic;
using System.Linq;
using Scripts.Setting;
using UnityEngine;

public class TrainingOptionEntity
{
    private readonly List<TrainingOption> _allOptions;

    public TrainingOptionEntity()
    {
        _allOptions = new List<TrainingOption>
        {
            new TrainingOption("筋トレ", TrainingType.Strength),
            new TrainingOption("持久走", TrainingType.Stamina),
            new TrainingOption("集中力トレ", TrainingType.Fate),
            new TrainingOption("休養", TrainingType.Rest),
            new TrainingOption("筋トレ（強化）",TrainingType.StrengthUp),
            new TrainingOption("持久走(強化)", TrainingType.StaminaUp),
            new TrainingOption("集中力トレ(強化)", TrainingType.FateUp),
        };
    }

    public List<TrainingOption> GetRandomOptions(int count = 5)
    {
        return _allOptions.OrderBy(_ => UnityEngine.Random.value).Take(count).ToList();
    }
}
