
namespace BattleInArea.Game
{

    public partial class CharacterController 
    {
        public class Hit : CharacterState
        {
            public const string MyKey = "Hit";

            public Hit(CharacterController ctn, params string[] newTransition) : base(ctn, MyKey, newTransition)
            {

            }

            public override void OnEnter()
            {
                Helper.RPC.Send("RpcPlayAnimation", PhotonTargets.Others, ctn.PlayerID, "damage", true);
                ctn.PlayAnimation("damage", true, 1f);
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