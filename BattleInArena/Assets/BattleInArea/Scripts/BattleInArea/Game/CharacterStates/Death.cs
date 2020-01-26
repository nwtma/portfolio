
namespace BattleInArea.Game
{

    public partial class CharacterController
    {
        public class Death : CharacterState
        {
            public Event.GameResult evt;
            public float time = 0;

            public const string MyKey = "Death";

            public Death(CharacterController ctn, params string[] newTransition) : base(ctn, MyKey, newTransition)
            {

            }

            public override void OnEnter()
            {
                Helper.RPC.Send("RpcPlayAnimation", PhotonTargets.Others, ctn.PlayerID, "death", false);
                ctn.PlayAnimation("death", false, 1f);

                SoundManager.instance.SetDeathSound(ctn.characterType);
            }

            public override bool OnExecute()
            {
                if (!ctn.IsPlayAnimation())
                {
                    return true;
                }
                return false;
            }

            public override void OnExit()
            {
            }
            public override bool IsTransition()
            {
                return false;
            }
        }
    }
}