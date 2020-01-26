
namespace BattleInArea.Game
{

    public partial class CharacterController
    {
        public class Move : CharacterState
        {
            public const string MyKey = "Move";

            public Move(CharacterController ctn, params string[] newTransition) : base(ctn, MyKey, newTransition)
            {

            }

            protected int moveTileX;
            protected int moveTileZ;
            protected UnityEngine.Vector3 goal;
            protected UnityEngine.Vector3 pos;

            public override void OnEnter()
            {
                ctn.OffSkillTrigger();
                ctn.SkillRemove();
                moveTileX = ctn.MoveTileX;
                moveTileZ = ctn.MoveTileZ;

                pos = ctn.Position;
                goal = new UnityEngine.Vector3(moveTileX, 0, moveTileZ);                    //goal의 좌표는 클릭한 좌표

                Helper.RPC.Send("RpcPlayAnimation", PhotonTargets.Others,  ctn.PlayerID, "walk", true);
                ctn.PlayAnimation("walk", true, 1f);
                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/State/Female/Move/MoveEffect");
            }

            public override bool OnExecute()
            {
                UnityEngine.Vector3 direction = UnityEngine.Vector3.Normalize((goal - pos));
                float distance = UnityEngine.Vector3.Distance(ctn.Position, goal); //(ctn.Position - goal).magnitude;
                ctn.Position = ctn.Position + (direction * UnityEngine.Time.deltaTime * 1f);


                if (0.1f >= distance)
                {
                    ctn.Position = new UnityEngine.Vector3((int)System.Math.Round(ctn.Position.x), 0, (int)System.Math.Round(ctn.Position.z));
                    ctn.OnSkillTrigger();
   
                    return true;
                }

                if(ctn.Position == goal)
                {
                    return false;
                }


                return false;
            }
           
            public override void OnExit()
            {
                CharacterController myCharacter = Character.Manager.Single.Get(ctn.PlayerID);

                if (ctn.AP > 0)
                {
                    ctn.OnCollider();
                }
                myCharacter.ConsumeAP(10);
                Helper.RPC.Send("RpcConsumeAP", PhotonTargets.Others, myCharacter.PlayerID);
            }

            public override bool IsTransition()
            {
                return false;
            }
        }
    }
}