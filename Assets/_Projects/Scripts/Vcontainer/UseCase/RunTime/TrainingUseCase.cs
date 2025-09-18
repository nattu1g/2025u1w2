using System.Collections.Generic;
using BBSim.Events;
using BBSim.Models;
using BBSim.Vcontainer.Entity;
using MessagePipe;
using UnityEngine;

namespace BBSim.Vcontainer.UseCase
{
    public class TrainingUseCase
    {
        private readonly PlayerClubEntity _playerClubEntity;
        private readonly TrainingOptionEntity _trainingOptionEntity;
        private readonly TrainingSelectUseCase _trainingSelectUseCase;



        public TrainingUseCase(
            PlayerClubEntity playerClubEntity,
            TrainingOptionEntity trainingOptionEntity,
            TrainingSelectUseCase trainingSelectUseCase
            )
        {
            _playerClubEntity = playerClubEntity;
            _trainingOptionEntity = trainingOptionEntity;
            _trainingSelectUseCase = trainingSelectUseCase;
        }

        public void ExecuteTraining(TrainingType trainingType)
        {
            // トレーニングの種類に応じて、全生徒の能力値を変更
            foreach (var student in _playerClubEntity.Students)
            {
                ApplyEffect(student, trainingType);
            }
            // トレーニングが終了して週を進める
            _trainingSelectUseCase.AdvanceWeek();
        }

        private void ApplyEffect(Student student, TrainingType trainingType)
        {
            switch (trainingType)
            {
                case TrainingType.Strength:
                    student.Power += Random.Range(3, 8);
                    break;
                case TrainingType.Stamina:
                    student.Stamina += Random.Range(3, 8);
                    break;
                case TrainingType.Fate:
                    student.Fate += Random.Range(3, 8);
                    break;
                case TrainingType.Rest:
                    // 休養の効果（例：特定のパラメータが少し回復するなど）
                    break;
                case TrainingType.StrengthUp:
                    student.Power += Random.Range(10, 20);
                    break;
                case TrainingType.StaminaUp:
                    student.Stamina += Random.Range(10, 20);
                    break;
                case TrainingType.FateUp:
                    student.Fate += Random.Range(10, 20);
                    break;
            }
        }
    }
}
