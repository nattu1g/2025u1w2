using AudioConductor.Runtime.Core;
using AudioConductor.Runtime.Core.Models;
using Common.Settings;
using Cysharp.Threading.Tasks;

namespace Common.Vcontainer.Entity
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

        public async UniTask PlayBGM(string bgmName)
        {
            _controllers[(int)AudioType.BGM].Play(bgmName);
            await UniTask.CompletedTask; // UniTaskを返すように修正
        }
        public async UniTask PlaySE(string seName)
        {
            _controllers[(int)AudioType.SE].Play(seName);
            await UniTask.CompletedTask; // UniTaskを返すように修正
        }
        public async UniTask StopBGM()
        {
            _controllers[(int)AudioType.BGM].Stop(false);
            await UniTask.CompletedTask; // UniTaskを返すように修正
        }


    }
}

