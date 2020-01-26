
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    public partial class Page
    {
        /// <summary>
        /// Battle In Area UI Manager
        /// </summary>
        public class InGame : Core.Page.IPage
        {

            public int ID
            {
                get { return (int)Page.ID.InGame; }
            }

            public IEnumerator OnPreEnter()
            {
                while (true)
                {
                    yield return null;

                    if (Helper.Sever.IsJoinedRoom)
                        break;
                }
            }


            public void OnEnter()
            {
                Event.PageModify evt = Core.Event.Getter.Get<Event.PageModify>();
                evt.hashtable["page"] = Page.ID.InGame;
                evt.hashtable["state"] = Event.PageState.OnEnter;
                Core.Event.Dispatcher.Dispatch(evt);

            }

            public void OnExecute()
            {
                Event.PageModify evt = Core.Event.Getter.Get<Event.PageModify>();
                evt.hashtable["page"] = Page.ID.InGame;
                evt.hashtable["state"] = Event.PageState.OnExecute;
                Core.Event.Dispatcher.Dispatch(evt);
            }
            public void OnExit()
            {
                Event.PageModify evt = Core.Event.Getter.Get<Event.PageModify>();
                evt.hashtable["page"] = Page.ID.InGame;
                evt.hashtable["state"] = Event.PageState.OnExit;
                Core.Event.Dispatcher.Dispatch(evt);

            }
        }
    }
}