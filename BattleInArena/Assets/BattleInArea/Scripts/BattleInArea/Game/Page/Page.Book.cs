
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
        public class Book : Core.Page.IPage
        {

            public int ID
            {
                get { return (int)Page.ID.Book; }
            }

            public IEnumerator OnPreEnter()
            {

                yield break;

            }


            public void OnEnter()
            {
                Event.PageModify evt = Core.Event.Getter.Get<Event.PageModify>();
                evt.hashtable["page"] = Page.ID.Book;
                evt.hashtable["state"] = Event.PageState.OnEnter;
                Core.Event.Dispatcher.Dispatch(evt);

            }

            public void OnExecute()
            {

            }

            public void OnExit()
            {
                Event.PageModify evt = Core.Event.Getter.Get<Event.PageModify>();
                evt.hashtable["page"] = Page.ID.Book;
                evt.hashtable["state"] = Event.PageState.OnExit;
                Core.Event.Dispatcher.Dispatch(evt);

            }
        }
    }
}