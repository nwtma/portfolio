
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    public partial class CharacterController
    {
        public class UsingSkill : CharacterState
        {
            public const string MyKey = "UsingSkill";

            public UsingSkill(CharacterController ctn, params string[] newTransition) : base(ctn, MyKey, newTransition)
            {
                skillInfos.Add(1, new SkillInfo("s1"));
                skillInfos.Add(2, new SkillInfo("s2"));
                skillInfos.Add(3, new SkillInfo("s3"));
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

            public override void OnEnter()
            {
                CharacterController MyPlayer = Character.Manager.Single.Get(PhotonNetwork.player.ID);
                CharacterController targetPlayer = Character.Manager.Single.GetOther();
                ctn.OffCollider();
                string animationName = skillInfos[ctn.selectedSkillIndex].animationName;

                Helper.RPC.Send("RpcPlayAnimation", PhotonTargets.Others, ctn.PlayerID, animationName, false);
                ctn.PlayAnimation(animationName, false);

                staticEffectManager.instance.SetEffectRPC(ctn.characterType, ctn.selectedSkillIndex, "NomalSkill", MyPlayer, targetPlayer); // 자신의 클라이언트
                Helper.RPC.Send("RpcSetEffect", PhotonTargets.Others, ctn.characterType, ctn.selectedSkillIndex, "NomalSkill", targetPlayer.PlayerID, MyPlayer.PlayerID, MyPlayer.SkillTileZ); 


                SoundManager.instance.SetSoundRPC(ctn.characterType, ctn.selectedSkillIndex, "Enter");
                SoundManager.instance.SetSoundRPC(ctn.characterType, ctn.selectedSkillIndex, "Turm");
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

                if (ctn.AP > 0)
                {
                    ctn.OnCollider();
                }

                CharacterController MyPlayer = Character.Manager.Single.Get(PhotonNetwork.player.ID);
                CharacterController targetPlayer = Character.Manager.Single.GetOther();
                ctn.ConsumeAP(CharacterData.instance.GetConsumption(ctn.selectedSkillIndex, ctn.CharacterType.ToString()));
                Helper.RPC.Send("RpcSetSkill", PhotonTargets.Others, ctn.PlayerID, CharacterData.instance.GetConsumption(ctn.selectedSkillIndex, ctn.CharacterType.ToString()));
                targetPlayer.ChangeHitState();

                if (targetPlayer.usedShield)
                {
                    targetPlayer.usedShield = false;
                    staticEffectManager.instance.SetItemEffect("ShieldItem", targetPlayer, MyPlayer); //여기
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