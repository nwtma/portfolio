using UnityEngine;


namespace BattleInArea.Game
{

    public partial class CharacterController
    {
        public class Idle : CharacterState
        {
            public const string MyKey = "Idle";

            public Idle(CharacterController ctn, params string[] newTransition) : base(ctn, MyKey, newTransition)
            {

            }

            public override void OnEnter()
            {
                Helper.RPC.Send("RpcPlayAnimation", PhotonTargets.Others, ctn.PlayerID,  "idle", true);

                ctn.PlayAnimation("idle", true, 1f);
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