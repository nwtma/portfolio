
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    public partial class Page
    {
        /// <summary>
        /// Battle In Area UI Manager
        /// </summary>
        public class Loding : Core.Page.IPage
        {

            public int ID
            {
                get { return (int)Page.ID.Loding; }
            }

            public IEnumerator OnPreEnter()
            {
                yield return null;
            }

            public void OnEnter()
            {
            }

            public void OnExecute()
            {

            }

            public void OnExit()
            {

            }
        }
    }
}