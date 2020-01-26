
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace BattleInArea.Game
//{
//    public partial class Effect
//    {
//        /// <summary>
//        /// Battle In Area UI Manager
//        /// </summary>
//        public class LaunchEffect : Effect.EffectState
//        {
//            public GameObject[] LaunchObjects;
//            GameObject clones;

//            public const string MyKey = "LaunchEffect";

//            public LaunchEffect(string key, params string[] newTransition) : base( MyKey, newTransition)
//            {
//                Key = key;
//                Transition = newTransition;

//                effectInfos.Add(1, new EffectInfo("MusaS1"));
//                effectInfos.Add(2, new EffectInfo("MagicianS1"));
//                effectInfos.Add(3, new EffectInfo("MagicianS2"));
//                effectInfos.Add(4, new EffectInfo("MagicianS2"));
//                effectInfos.Add(5, new EffectInfo("ArcherS1"));
//                effectInfos.Add(6, new EffectInfo("ArcherS2"));
//                effectInfos.Add(7, new EffectInfo("ArcherS3"));
//                effectInfos.Add(8, new EffectInfo("ThiefS3"));

//            }

//            public class EffectInfo
//            {
//                public EffectInfo(string effectName)
//                {
//                    this.effectName = effectName;
//                }

//                public string effectName;
//            }

//            Dictionary<int, EffectInfo> effectInfos = new Dictionary<int, EffectInfo>();


//            GameObject Test;
//            public override void CloneCraft()                                                   //좌우 이펙트 생성 메소드
//            {
//                string effectName = effectInfos[ctn.selectedSkillIndex].animationName;

//                Helper.RPC.Send("RpcPlayAnimation", PhotonTargets.Others, ctn.PlayerID, effectName, false);
//                ctn.PlayAnimation(effectName, false);


//                for (int i = 0; i < LaunchObjects.Length; i++)
//                {
//                    if(LaunchObjects[i].name == effectName)
//                    {
//                        //게임 오브젝트 생성
//                        clones = GameObject.Instantiate(LaunchObjects[i], Vector3.zero, Quaternion.identity);
//                    }


//                }
//                Helper.RPC.Send("RpcPlayAnimation", PhotonTargets.Others, ctn.PlayerID, effectName, false);
//            }

  
//            public override void OnEnter()
//            {
//                CloneCraft();

//            }

//            public override bool OnExecute()
//            {
//                return false;
//            }

//            public override void OnExit()
//            {
//                Event.PageModify evt = Core.Event.Getter.Get<Event.PageModify>();
//                evt.hashtable["page"] = Page.ID.MainMenu;
//                evt.hashtable["state"] = Event.PageState.OnExit;
//                Core.Event.Dispatcher.Dispatch(evt);
//            }

//            public override bool IsTransition()
//            {
//                return false;
//            }
//        }
//    }
//}