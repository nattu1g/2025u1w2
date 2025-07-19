using Scripts.Mono;
using Scripts.Vcontainer.Entity;
using UnityEngine;

namespace Scripts.Vcontainer.Handler
{
    public class GameInitializationHandler : IHandler
    {
        readonly ComponentAssembly _componentAssembly;
        // readonly StudentEntity _studentEntity;
        // readonly TeacherEntity _teacherEntity; // TeacherEntityをインジェクト
        // readonly EventItemEntity _eventItemEntity;
        // readonly ClubItemEntity _clubItemEntity;



        public GameInitializationHandler(
            ComponentAssembly componentAssembly
            // StudentEntity studentEntity,
            // TeacherEntity teacherEntity,
            // EventItemEntity eventItemEntity,
            // ClubItemEntity clubItemEntity
            )
        {
            _componentAssembly = componentAssembly;
            // _studentEntity = studentEntity;
            // _teacherEntity = teacherEntity; // TeacherEntityを初期化
            // _eventItemEntity = eventItemEntity;
            // _clubItemEntity = clubItemEntity;
        }
        public void Initialize()
        {
            // _studentEntity.InitializeStudentMasterList(_componentAssembly.StudentList);
            // _teacherEntity.InitializeTeacherMasterList(_componentAssembly.TeacherList); // 先生リストの初期化
            // _eventItemEntity.InitializeEventItemMasterList(_componentAssembly.EventItemList);
            // _clubItemEntity.InitializeClubItemMasterList(_componentAssembly.ClubItemList);
        }


        public void Clear()
        {
        }

        public void Dispose()
        {
        }
    }
}
