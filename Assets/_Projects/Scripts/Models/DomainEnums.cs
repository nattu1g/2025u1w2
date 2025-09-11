namespace BBSim.Models
{
    public enum ClassType
    {
        Warrior,
        Archer,
        Wizard,
    }
    public enum TrainingType
    {
        None, Strength, Stamina, Fate, // StudentモデルのPower, Stamina, Fate に対応
        Rest,
        StrengthUp, StaminaUp, FateUp,
    }
}
