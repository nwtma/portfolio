
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{

    public partial class CharacterController
    {
        public class UsingMoveSkill : CharacterState
        {
            public const string MyKey = "UsingMoveSkill";

            public UsingMoveSkill(CharacterController ctn, params string[] newTransition) : base(ctn, MyKey, newTransition)
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
                ctn.OffCollider();
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
                CharacterController MyPlayer = Character.Manager.Single.Get(PhotonNetwork.player.ID);
                CharacterController targetPlayer = Character.Manager.Single.GetOther();
                targetPlayer.ChangeHitState();                                            //자신의 스킬 애니매이션을 했으니 상태는 데미지를 받는 애니매이션을 하거라

                if (ctn.AP > 0)
                {
                    ctn.OnCollider();
                }
                ctn.ConsumeAP(CharacterData.instance.GetConsumption(ctn.selectedSkillIndex, ctn.CharacterType.ToString()));
                Helper.RPC.Send("RpcSetSkill", PhotonTargets.Others, ctn.PlayerID, CharacterData.instance.GetConsumption(ctn.selectedSkillIndex, ctn.CharacterType.ToString()));
                
                staticEffectManager.instance.SetEffectRPC(ctn.characterType, ctn.selectedSkillIndex, "MoveSkill", MyPlayer, targetPlayer);
                Helper.RPC.Send("RpcSetEffect", PhotonTargets.Others, ctn.characterType, ctn.selectedSkillIndex, "MoveSkill", targetPlayer.PlayerID, MyPlayer.PlayerID, MyPlayer.SkillTileZ);

                // Helper.RPC.Send("RpcSetEffect", PhotonTargets.Others, MyPlayer.CharacterType, ctn.selectedSkillIndex, "Exit", MyPlayer, targetPlayer);


                if (targetPlayer.Position.z == ctn.MoveTileZ)//ctn.GetSkillTiles("skill02Tiles")[i].Z)
                {
                    if (ctn.Position.z <= SkillTileZ)
                    {
                        if (SkillTileZ != 7)
                        {
                            ctn.SkillRemove();
                            ctn.SetPosition(SkillTileX, SkillTileZ + 1);
                        }
                        else
                        {
                            ctn.SetPosition(SkillTileX, SkillTileZ - 1);
                        }

                    }

                    else if (ctn.Position.z >= SkillTileZ)
                    {
                        if (SkillTileZ != 0)
                        {
                            ctn.SkillRemove();
                            ctn.SetPosition(SkillTileX, SkillTileZ - 1);
                        }
                        else
                        {
                            ctn.SetPosition(SkillTileX, SkillTileZ + 1);
                        }
                    }
                }
                else
                {
                    ctn.SkillRemove();
                    ctn.SetPosition(SkillTileX, SkillTileZ);
                }
                ctn.OnSkillTrigger();

                if (targetPlayer.usedShield)
                {
                    targetPlayer.usedShield = false;
                    staticEffectManager.instance.SetItemEffect("ShieldItem", targetPlayer, MyPlayer);
                    Helper.RPC.Send("RpcSetItemEffect", PhotonTargets.Others, "ShieldItem", MyPlayer.PlayerID, targetPlayer.PlayerID); //ctn = MyPlayer
                    return;
                }
                SoundManager.instance.SetSoundRPC(ctn.characterType, ctn.selectedSkillIndex, "Exit");
                SoundManager.instance.SetHitSound(targetPlayer.characterType);
                targetPlayer.GiveDamage(CharacterData.instance.GetDamage(ctn.selectedSkillIndex, ctn.CharacterType.ToString()));
                Helper.RPC.Send("RpcTakedamage", PhotonTargets.Others, CharacterData.instance.GetDamage(ctn.selectedSkillIndex, ctn.CharacterType.ToString()), targetPlayer.PlayerID);
                targetPlayer.FloatingTextOnEnter(CharacterData.instance.GetDamage(ctn.selectedSkillIndex, ctn.CharacterType.ToString()));
                Helper.RPC.Send("RPCFloatingTextEntry", PhotonTargets.Others, CharacterData.instance.GetDamage(ctn.selectedSkillIndex, ctn.CharacterType.ToString()), targetPlayer.PlayerID);
            }

            public override bool IsTransition()
            {
                return false;
            }
        }
    }
}