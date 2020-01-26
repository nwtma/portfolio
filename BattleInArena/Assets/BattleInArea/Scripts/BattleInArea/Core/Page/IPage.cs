


using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Core
{
    public partial class Page
    {
        public interface IPage : Interfaces.IHasIdentity
        {
            IEnumerator OnPreEnter();
            void OnEnter();
            void OnExecute();
            void OnExit();
        }
    }
}