

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    public class TurnManager : Core.Interfaces.IManager
    {
        private int Index = 0;
        private List<int> playerIDList = new List<int>();

        public int PlayerCount
        {
            get { return playerIDList.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        private int CurrentTurnPlayerID
        {
            get
            {
                return playerIDList[Index];
            }
        }

        private int NextTurnPlayerID
        {
            get
            {
                ++Index;

                if (Index >= playerIDList.Count)
                {
                    Index = 0;
                }

                return playerIDList[Index];
            }
        }

        public void Prepare()
        {

        }

        public void AddPlayerID(int id)
        {
            playerIDList.Add(id);
        }

        public void StartGame()
        {
            Event.TurnModify evt = Core.Event.Getter.Get<Event.TurnModify>();
            evt.hashtable["player_id"] = CurrentTurnPlayerID;

            Core.Event.Dispatcher.Dispatch(evt);
        }

        public void ChangeTurn()
        {
            Event.TurnModify evt = Core.Event.Getter.Get<Event.TurnModify>();
            evt.hashtable["player_id"] = NextTurnPlayerID;

            Core.Event.Dispatcher.Dispatch(evt);
        }

        public bool IsMyTurn(int playerID)
        {
            if(CurrentTurnPlayerID == PhotonNetwork.player.ID)
            {
                return true;
            }

            return false;
        }
    }
}