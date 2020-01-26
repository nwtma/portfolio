

using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Core
{
    public partial class Effect
    {
        public abstract class Manager : Interfaces.IManager
        {
            public abstract void Prepare();

            protected Dictionary<string, IEffect> effects = new Dictionary<string, IEffect>();
            private IEffect effect;

           public virtual void Update()                                          //  클라이언트에서 기타 잡 기능을 없애고 이 업데이트로 자동으로 할당되게 했다. !
            {                                                                    //  흑흑흑 너무 어렵다. 그러면 이펙트를 넣어주는것은 클라이언트가 아닌 캐릭터에서 해주는 것이 학계의 정론
                if (effect == null) { return; }
           
               if (effect.OnExecute())
               {
                   for (int i = 0, ii = effect.Transition.Length; ii > i; ++i)
                   {
                       IEffect Next = Get(effect.Transition[i]);
           
                       if (Next.IsTransition())
                       {
                           effect.OnExit();
           
                           effect = Next;
                           effect.OnEnter();
                       }
                   }
               }
           }

            public void Add(IEffect effect)
            {
                if (!effects.ContainsKey(effect.Key))                                      // 키의 값이 포함이 안되어있으면 키를 추가해준다.
                {
                    effects.Add(effect.Key, effect);
                }
            }


            public void Change(string Key)
            {
                if (effect != null)
                {
                    effect.OnExit();                        //현재 친구들을 빼준다.
                }

                effect = Get(Key);
                effect.OnEnter();
            }

            public IEffect Get(string key)                  //키 값의 친구를 불러줌(그 키 값 친구를 반환해줌)
            {
                if (effects.ContainsKey(key))
                {
                    return effects[key];
                }

                return null;
            }



     
        }

    }
}
