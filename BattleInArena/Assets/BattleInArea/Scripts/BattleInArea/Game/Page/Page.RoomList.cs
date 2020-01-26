
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BattleInArea.Game
{
    public partial class Page
    {
        public class RoomList : Core.Page.IPage
        {

            public int ID
            {
                get { return (int)Page.ID.RoomList; }
            }


            public IEnumerator OnPreEnter()
            {

                yield break;

            }

            public void OnEnter()
            {
                Event.PageModify evt = Core.Event.Getter.Get<Event.PageModify>();
                evt.hashtable["page"] = Page.ID.RoomList;
                evt.hashtable["state"] = Event.PageState.OnEnter;
                Core.Event.Dispatcher.Dispatch(evt);

            }

            public void OnExecute()
            {

            }

            public void OnExit()
            {
                Event.PageModify evt = Core.Event.Getter.Get<Event.PageModify>();
                evt.hashtable["page"] = Page.ID.RoomList;
                evt.hashtable["state"] = Event.PageState.OnExit;
                Core.Event.Dispatcher.Dispatch(evt);
            }
        }
    }
}