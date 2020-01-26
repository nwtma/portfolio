


using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleInArea.Core
{
    public partial class Effect
    {
        public interface IEffect : Interfaces.IStringSave, Interfaces.IArrayString, Interfaces.ICraft
        {

            void OnEnter();
            bool OnExecute();
            void OnExit();

            bool IsTransition();
        }
    }
}