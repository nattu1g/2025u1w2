namespace BBSim.Vcontainer.Entity
{
    // ClubEntityを継承する
    public class OpponentClubEntity : ClubEntity
    {
        // 基底クラスのコンストラクタを呼び出す
        public OpponentClubEntity(StudentEntity studentEntity) : base(studentEntity)
        {
        }

        // 今後、AIの対戦相手特有のロジックが必要になればここに追加する
    }
}