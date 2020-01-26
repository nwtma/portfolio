
namespace BattleInArea.Game
{

    public partial class CharacterController
    {
        public class Nomal : CharacterState
        {
            public const string MyKey = "Nomal";

            public Nomal(CharacterController ctn, params string[] newTransition) : base(ctn, MyKey, newTransition)
            {

            }

            public override void OnEnter()
            {
                ctn.evt2.hashTable["character_id"] = ctn.PlayerID;
                ctn.evt2.hashTable["result_type"] = Event.Result.Die;
                Core.Event.Dispatcher.Dispatch(ctn.evt2);
            }

            public override bool OnExecute()
            {
                return true;
            }

            public override void OnExit()
            {

            }
            public override bool IsTransition()
            {
                return true;
            }
        }
    }
}