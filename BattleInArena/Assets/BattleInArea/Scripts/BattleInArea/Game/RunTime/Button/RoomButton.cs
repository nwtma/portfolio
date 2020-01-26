using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using BattleInArea.Core;

namespace BattleInArea.Game
{
    public class RoomButton : MonoBehaviour
    {
        public Event.Room e;
        public GameObject roomRoot;
        private Button roomButton;
        private Text buttonText;

        public void Awake()
        {
            e = Core.Event.Getter.Get<Event.Room>();
            buttonText = this.gameObject.GetComponentInChildren<Text>();
        }

        public void OnClickButton()
        {
            e.hashTable["selected_room"] = buttonText.text;

            Core.Event.Dispatcher.Dispatch(e);
        }

        public void FindRoom()
        {
            RoomInfo[] RoomList = PhotonNetwork.GetRoomList();

            for (int i = 0; i < RoomList.Length; ++i)
            {
                if (this.gameObject.name == RoomList[i].Name)
                {
                    continue;
                }

                this.gameObject.name = "";
                buttonText.text = "";
            }
        }

        protected void Update()
        {
            //FindRoom();
        }
    }
}