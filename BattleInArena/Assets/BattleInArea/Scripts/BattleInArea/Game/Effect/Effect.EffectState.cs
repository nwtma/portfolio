using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;

namespace BattleInArea.Game
{

    public partial class Effect
    {
        public class EffectState : Core.Effect.IEffect
        {
            //protected CharacterController ctn { private set; get; }
            //public EffectState(CharacterController ctn, string newKey, params string[] newTransition)
            //{
            //    this.ctn = ctn;
            //}

            public EffectState( string newKey, params string[] newTransition)
            {
           
            }

            string Keytest;
            public string Key
            {
                get { return Keytest; }
                set { Keytest = value; }
            }

            string[] Transitiontest;
            public string[] Transition
            {
                get { return Transitiontest; }
                set { Transitiontest = value; }
            }

            public virtual void CloneCraft()                                                   //이펙트 생성 메소드
            {
                

            }


            public virtual void OnEnter()
            {
                CloneCraft();

            }

            public virtual bool OnExecute()
            {
                return false;
            }

            public virtual void OnExit()
            {

            }

            public virtual bool IsTransition()
            {
                return false;
            }
        }
    }
}


