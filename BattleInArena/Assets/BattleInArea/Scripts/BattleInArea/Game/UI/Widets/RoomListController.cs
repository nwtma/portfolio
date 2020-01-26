using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using BattleInArea.Core;

namespace BattleInArea.Game
{
    public class RoomListController : Core.UI.Controller
    {
        public Event.Selected evt;
        public GameObject roomRoot;
        public GameObject loadingUI;

        private Image myTypeImg;
        private Image myItemImg;
        private Text roomTxt;

        [SerializeField]
        public GameObject typeParent;
        [SerializeField]
        public GameObject itemParent;
        [SerializeField]
        public Types.Character type;
        [SerializeField]
        public UI.ItemID item;
        [SerializeField]
        protected Button joinButton;
        [SerializeField]
        protected Button roomButton;
        protected bool isCreated = false;
        protected bool isSelected = false;
        protected bool isStart = false;
        public bool[] isRoomState;
        //protected Color selectedColor = new Color32(255, 0, 0, 255);
        protected Color nullColor = new Color32(181, 181, 181, 255);
        protected Color nomalColor = new Color32(255, 255, 255, 255);
        protected string selectedRoom;
        protected GameObject selectedButton;

        public List<string> roomNameList = new List<string>();
        public List<Text> roomsName = new List<Text>();
        protected List<bool> RoomStates = new List<bool>();

        public override int ID
        {
            get { return (int)UI.ID.RoomListController; }
        }

        private new void Awake()
        {
            myTypeImg = typeParent.GetComponent<Image>();
            myItemImg = itemParent.GetComponent<Image>();
            for(int i = 0; i < 6; ++i)
            {
                RoomStates.Add(false);
            }
        }
        private void Start()
        {
            //joinButton.enabled = false;
            //joinButton.image.color = nullColor;

            roomNameList.Add("자신있으면 드루와!!");
            roomNameList.Add("너만 오면 고");
            roomNameList.Add("내가 짱이야");
            roomNameList.Add("내가 다 이겨");
            roomNameList.Add("너한테는 안져");

            selectedRoom = "";
            evt = Core.Event.Getter.Get<Event.Selected>();

        }

        public void Update()
        {
            if (this.gameObject.activeSelf)
            {
                RoomCheck();
                ShowRoomList();
                if (isStart == false)
                {
                    SetMyItem();
                    SetMyType();
                    isStart = true;
                }
            }
        }
        public void GetMyInfo(Types.Character type, UI.ItemID item)
        {
            this.type = type;
            this.item = item;
            Debug.Log(this.type);
            Debug.Log(this.item);

        }
        public void SetMyItem()
        {
            switch (item)
            {
                case UI.ItemID.HpItem:
                    {
                        myItemImg.sprite = Instantiate(Resources.Load<Sprite>("RoomList/Item/HP"));
                    }
                    break;
                case UI.ItemID.ApItem:
                    {
                        myItemImg.sprite = Instantiate(Resources.Load<Sprite>("RoomList/Item/AP"));
                    }
                    break;
                case UI.ItemID.ShieldItem:
                    {
                        myItemImg.sprite = Instantiate(Resources.Load<Sprite>("RoomList/Item/Shield"));
                    }
                    break;
            }
        }
        public void SetMyType()
        {
            switch (type)
            {
                case Types.Character.Musa:
                    {
                        myTypeImg.sprite = Resources.Load<Sprite>("RoomList/Type/Musa");
                    }
                    break;
                case Types.Character.Thief:
                    {
                        myTypeImg.sprite = Resources.Load<Sprite>("RoomList/Type/Thief");
                    }
                    break;
                case Types.Character.Archer:
                    {
                        myTypeImg.sprite = Resources.Load<Sprite>("RoomList/Type/Archer");
                    }
                    break;
                case Types.Character.Magician:
                    {
                        myTypeImg.sprite = Resources.Load<Sprite>("RoomList/Type/Magician");
                    }
                    break;
            }
        }
        public int ReturnRoomName(int idx)
        {
            int roomidx = Random.Range(0, 5);
            int roomCount = Random.Range(0, 1000);
            switch (idx)
            {
                case 1:
                    {
                        return roomidx;
                    }
                case 2:
                    {
                        return roomCount;
                    }
            }
            return 0;
        }
        public void MakeRoom()
        {
            loadingUI.SetActive(true);
            PhotonNetwork.CreateRoom(roomNameList[ReturnRoomName(1)] + ReturnRoomName(2).ToString(), new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
            isCreated = true;
            Debug.Log("방 생성 완료" + " " + "방 이름은" + roomNameList[ReturnRoomName(1)] + ReturnRoomName(2).ToString());

            Event.Selected evt = Core.Event.Getter.Get<Event.Selected>();
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.JoinTheRoom;
            Core.Event.Dispatcher.Dispatch(evt);
            isStart = false;
        }
        public void JoinedRoom()
        {
            if (selectedRoom == "") { return; }
            loadingUI.SetActive(true);
            PhotonNetwork.JoinRoom(selectedRoom);

            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            Event.Selected evt = Core.Event.Getter.Get<Event.Selected>();
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.JoinTheRoom;
            Core.Event.Dispatcher.Dispatch(evt);
            isStart = false;
        }
        public void ShowRoomList()
        {
            RoomInfo[] RoomList = PhotonNetwork.GetRoomList();
            for (int i = 0; i < roomsName.Count; ++i)
            {
                if (RoomList.Length == 0) { roomsName[i].text = ""; Debug.Log("키읔키읔"); }
                else
                {
                    for (int j = 0; j < RoomList.Length; ++j)
                    {
                        if(!RoomStates[j])
                        {
                            roomsName[j].text = RoomList[j].Name;
                            Debug.Log("방이름" + RoomList[j].Name);
                            Debug.Log(RoomList.Length);
                            if (i == 4) { return; }
                            if (roomsName[j].text == roomsName[j + 1].text) // 룸리스트가 
                            {
                                roomsName[j + 1].text = "";
                            }
                          // RoomStates.Remove(true);
                        }
                    }
                }
            }
        }
        public void RoomCheck()
        {
            RoomInfo[] roomList = PhotonNetwork.GetRoomList();
            for (int i = 0; i < roomList.Length; ++i)
            {
                RoomStates[i] = false;
                if (roomList[i].PlayerCount == 2)
                {
                    if (roomsName[i].text == roomList[i].Name)
                    {
                        roomsName[i].text = "";
                    }
                    RoomStates[i] = true;
                }
            }
        }
        public void OnClickBackButton()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            isStart = false;
            this.type = new Types.Character();
            this.item = new UI.ItemID();
            myTypeImg.sprite = null;
            myItemImg.sprite = null;
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.CreditButton;
            Core.Event.Dispatcher.Dispatch(evt);
        }

        public void OnClickRoomButton(string selectedRoom)
        {
            isSelected = true;
            this.selectedRoom = selectedRoom;
        }

        public void HideLoding()
        {
            loadingUI.SetActive(false);
        }

        public void StartGame()
        {
            isStart = false;
        }
    }
}