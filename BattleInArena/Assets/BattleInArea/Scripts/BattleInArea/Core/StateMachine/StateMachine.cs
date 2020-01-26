

using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Core
{
    public class StateMachine
    {
        private Dictionary<string, State> States = new Dictionary<string, State>();
        private State activeStae;

        public void Update()                                          //  클라이언트에서 기타 잡 기능을 없애고 이 업데이트로 자동으로 할당되게 했다. !
        {                                                             //  흑흑흑 너무 어렵다. 그러면 이펙트를 넣어주는것은 클라이언트가 아닌 캐릭터에서 해주는 것이 학계의 정론
            if (activeStae == null) { return; }

            if (activeStae.OnExecute())
            {
                for (int i = 0, ii = activeStae.Transition.Length; ii > i; ++i)
                {
                    State Next = GetState(activeStae.Transition[i]);

                    if (Next.IsTransition())   // Idle 모션 때문에 업데이트 함수가 존재?
                    {
                        activeStae.OnExit();

                        activeStae = Next;
                        activeStae.OnEnter();
                    }
                }
            }
        }

        public void Change(string Key)
        {
            if (activeStae != null)
            {
                activeStae.OnExit(); //현재 친구들을 빼준다.
            }

            activeStae = GetState(Key);
            activeStae.OnEnter();
        }
            

        public void Add(State newState)
        {
            if (!States.ContainsKey(newState.Key))
            {
                States.Add(newState.Key, newState);
            }
        }

        private State GetState(string key)
        {
            if (States.ContainsKey(key))
            {
                return States[key];
            }

            return null;
        }

    }
}
