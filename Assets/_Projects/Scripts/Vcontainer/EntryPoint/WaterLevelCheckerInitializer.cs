using App.Features.WaterTank.Baseline;
using App.Features.WaterTank.Water;
using VContainer.Unity;

namespace App.Vcontainer.EntryPoint
{
    /// <summary>
    /// WaterLevelCheckerにBaselineDisplayを注入するEntryPoint
    /// </summary>
    public class WaterLevelCheckerInitializer : IStartable
    {
        private readonly WaterLevelChecker _waterLevelChecker;
        private readonly BaselineDisplay _baselineDisplay;

        public WaterLevelCheckerInitializer(
            WaterLevelChecker waterLevelChecker,
            BaselineDisplay baselineDisplay)
        {
            _waterLevelChecker = waterLevelChecker;
            _baselineDisplay = baselineDisplay;
        }

        public void Start()
        {
            _waterLevelChecker.Initialize(_baselineDisplay);
        }
    }
}
