using AudioConductor.Runtime.Core;
using AudioConductor.Runtime.Core.Models;
using Scripts.Setting;

namespace Scripts.Vcontainer.Entity
{
    public class AudioEntity
    {
        public ICueController[] _controllers;

        public void SetCueSheet(CueSheetAsset cueSheetAsset)
        {
            var cueList = cueSheetAsset.cueSheet.cueList;
            _controllers = new ICueController[cueList.Count];

            _controllers[(int)AudioType.BGM] ??= AudioConductorInterface.CreateController(cueSheetAsset, (int)AudioType.BGM);
            _controllers[(int)AudioType.SE] ??= AudioConductorInterface.CreateController(cueSheetAsset, (int)AudioType.SE);
        }
    }
}

