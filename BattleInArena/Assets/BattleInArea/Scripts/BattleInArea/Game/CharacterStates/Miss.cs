
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    public partial class CharacterController
    {
        public class Miss : CharacterState
        {
            public const string MyKey = "Miss";

            public Miss(CharacterController ctn, params string[] newTransition) : base(ctn, MyKey, newTransition)
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

                if (ctn.AP > 0)
                {
                    ctn.OnCollider();
                }
                ctn.ConsumeAP(CharacterData.instance.GetConsumption(ctn.selectedSkillIndex, ctn.CharacterType.ToString()));
                Helper.RPC.Send("RpcSetSkill", PhotonTargets.Others, ctn.PlayerID, CharacterData.instance.GetConsumption(ctn.selectedSkillIndex, ctn.CharacterType.ToString()));

                SoundManager.instance.SetSoundRPC(ctn.characterType, ctn.selectedSkillIndex, "Turm");
            }


            public override bool IsTransition()
            {
                return false;
            }
        }
    }
}