
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{

    public partial class CharacterController
    {
        public class MoveSkillMiss : CharacterState
        {
            public const string MyKey = "MoveSkillMiss";

            public MoveSkillMiss(CharacterController ctn, params string[] newTransition) : base(ctn, MyKey, newTransition)
            {
                skillInfos.Add(2, new SkillInfo("s2"));
            }

            public class SkillInfo
            {
                public SkillInfo(string animationName)
                {
                    this.animationName = animationName;
                }

                public string animationName;
            }
            Dictionary<int, SkillInfo> skillInfos = new Dictionary<int, SkillInfo>();

            protected int SkillTileX;
            protected int SkillTileZ;

            public override void OnEnter()
            {
                CharacterController MyPlayer = Character.Manager.Single.Get(PhotonNetwork.player.ID);
                CharacterController targetPlayer = Character.Manager.Single.GetOther();
                ctn.SkillRemove();
                ctn.OffSkillTrigger();
                SkillTileX = ctn.MoveTileX;
                SkillTileZ = ctn.MoveTileZ;
                string animationName = skillInfos[ctn.selectedSkillIndex].animationName;
                Helper.RPC.Send("RpcPlayAnimation", PhotonTargets.Others, ctn.PlayerID, animationName, true);
                ctn.PlayAnimation(animationName, true, 1f);
                SoundManager.instance.SetSoundRPC(ctn.characterType, ctn.selectedSkillIndex, "Enter");
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
                ctn.OffCollider();
                if (ctn.AP > 0)
                {
                    ctn.OnCollider();
                }

                CharacterController targetPlayer = Character.Manager.Single.GetOther();
                ctn.ConsumeAP(CharacterData.instance.GetConsumption(ctn.selectedSkillIndex, ctn.CharacterType.ToString()));
                Helper.RPC.Send("RpcSetSkill", PhotonTargets.Others, ctn.PlayerID, CharacterData.instance.GetConsumption(ctn.selectedSkillIndex, ctn.CharacterType.ToString()));

                if (targetPlayer.CharPos().z == SkillTileZ)
                {
                    if (ctn.CharPos().z <= SkillTileZ)
                    {
                        if (SkillTileZ == 7)
                        {
                            return;
                        }
                        ctn.SetPosition(SkillTileX, SkillTileZ + 1);
                    }

                    else if (ctn.CharPos().z >= SkillTileZ)
                    {
                        if (SkillTileZ == 0)
                        {
                            return;
                        }
                        ctn.SetPosition(SkillTileX, SkillTileZ - 1);
                    }
                }
                else
                {
                    ctn.SetPosition(SkillTileX, SkillTileZ);
                }
                ctn.OnSkillTrigger();
            }

            public override bool IsTransition()
            {
                return false;
            }
        }
    }
}