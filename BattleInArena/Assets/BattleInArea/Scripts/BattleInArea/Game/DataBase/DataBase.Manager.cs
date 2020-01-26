


namespace BattleInArea.Game
{
    public partial class DataBase
    {
        public const string CharacterTableKey = "";

        public class Manager : Core.DataBase.Manager
        {
            public override void Prepare()
            {
                base.Add(new CharacterTable());
            }
        }
    }
}