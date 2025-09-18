namespace BBSim.Vcontainer.Entity
{
    // ClubEntityを継承する
    public class PlayerClubEntity : ClubEntity
    {
        // 基底クラスのコンストラクタを呼び出す
        public PlayerClubEntity(StudentEntity studentEntity) : base(studentEntity)
        {
        }

        // 今後、プレイヤー特有のロジックが必要になればここに追加する
        // スタメン
    }
}