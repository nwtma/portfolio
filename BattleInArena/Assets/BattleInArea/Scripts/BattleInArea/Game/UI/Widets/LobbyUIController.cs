using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace BattleInArea.Game
{
    public class LobbyUIController : Core.UI.Controller
    {
        [SerializeField]
        protected GameObject[] Item;
        public Event.Selected evt;
        public Event.Item e;
        [SerializeField]
        protected GameObject StartButton;
        [SerializeField]
        protected GameObject Musa;
        [SerializeField]
        protected GameObject Thief;
        [SerializeField]
        protected GameObject Archer;
        [SerializeField]
        protected GameObject Magician;
        [SerializeField]
        protected GameObject[] selectBack;
        [SerializeField]
        public GameObject itemSelected;
        [SerializeField]
        public bool isSelectedItem;
        public int check = 1;


        public override int ID
        {
            get { return (int)UI.ID.LobbyUIController; }
        }

        private void Awake()
        {
            e = Core.Event.Getter.Get<Event.Item>();
            evt = Core.Event.Getter.Get<Event.Selected>();
        }

        private void Start()
        {
            isSelectedItem = false;
        }

        public void OnClickBackButton()
        {
            PhotonNetwork.Disconnect();
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.CreditButton;
            Core.Event.Dispatcher.Dispatch(evt);
        }

        public void OnClickStartGame()
        {
            if(!isSelectedItem)
            {
                if(itemSelected.activeSelf == false)
                {
                    SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
                    itemSelected.SetActive(true);
                    Invoke("OffItemSelected", 1.5f);
                    return;
                }
                return;
            }

                SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
                Event.Selected evt = Core.Event.Getter.Get<Event.Selected>();
                evt.hashTable["select_type"] = Event.SelectedType.UI;
                evt.hashTable["select_ui_id"] = UI.ID.GameStartButton;
                Core.Event.Dispatcher.Dispatch(e);
                Core.Event.Dispatcher.Dispatch(evt);
                SetMyInfo();
 
        }

        public void SetMyInfo()
        {
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.GetMyInfo;
            Core.Event.Dispatcher.Dispatch(e);
            Core.Event.Dispatcher.Dispatch(evt);

        }


        void ShowImage(int a)
        {
            switch(a)
            {
                case 1:
                    {
                        Musa.SetActive(true);
                        Thief.SetActive(false);
                        Archer.SetActive(false);
                        Magician.SetActive(false);
                        evt.hashTable["selected_character_type"] = Types.Character.Musa;
                        selectBack[0].SetActive(true);
                        selectBack[1].SetActive(false);
                        selectBack[2].SetActive(false);
                        selectBack[3].SetActive(false);
                    }
                    break;
                case 2:
                    {
                        Musa.SetActive(false);
                        Thief.SetActive(true);
                        Archer.SetActive(false);
                        Magician.SetActive(false);
                        evt.hashTable["selected_character_type"] = Types.Character.Thief;
                        selectBack[1].SetActive(true);
                        selectBack[0].SetActive(false);
                        selectBack[2].SetActive(false);
                        selectBack[3].SetActive(false);
                    }
                    break;
                case 3:
                    {
                        Musa.SetActive(false);
                        Thief.SetActive(false);
                        Archer.SetActive(true);
                        Magician.SetActive(false);
                        evt.hashTable["selected_character_type"] = Types.Character.Archer;
                        selectBack[2].SetActive(true);
                        selectBack[0].SetActive(false);
                        selectBack[1].SetActive(false);
                        selectBack[3].SetActive(false);
                    }
                    break;
                case 4:
                    {
                        Musa.SetActive(false);
                        Thief.SetActive(false);
                        Archer.SetActive(false);
                        Magician.SetActive(true);
                        evt.hashTable["selected_character_type"] = Types.Character.Magician;
                        selectBack[3].SetActive(true);
                        selectBack[0].SetActive(false);
                        selectBack[1].SetActive(false);
                        selectBack[2].SetActive(false);
                    }
                    break;
            }
        }

        public void ClickRightArrow()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");

            if (check == 4)
            {
                check = 1;
                ShowImage(check);
                evt.hashTable["select_type"] = Event.SelectedType.UI;
                evt.hashTable["select_ui_id"] = UI.ID.CharacterChangeButten;
                Core.Event.Dispatcher.Dispatch(evt);
                return;
            }
            ++check;
            ShowImage(check);
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.CharacterChangeButten;
            Core.Event.Dispatcher.Dispatch(evt);
        }

        public void ClickLeftArrow()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            if(check == 1)
            {
                check = 4;
                ShowImage(check);
                evt.hashTable["select_type"] = Event.SelectedType.UI;
                evt.hashTable["select_ui_id"] = UI.ID.CharacterChangeButten;
                Core.Event.Dispatcher.Dispatch(evt);
                return;
            }
            --check;
            ShowImage(check);
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.CharacterChangeButten;
            Core.Event.Dispatcher.Dispatch(evt);
        }
        public void HpItem()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            isSelectedItem = true;
            for(int i = 0; i < Item.Length; ++i)
            {
                Item[i].SetActive(false);
            }
            e.hashTable["select_item_id"] = UI.ItemID.HpItem;
            evt.hashTable["select_item_id"] = UI.ItemID.HpItem;
            Item[0].SetActive(true);
        }

        public void APItem()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            isSelectedItem = true;
            for (int i = 0; i < Item.Length; ++i)
            {
                Item[i].SetActive(false);
            }
            e.hashTable["select_item_id"] = UI.ItemID.ApItem;
            evt.hashTable["select_item_id"] = UI.ItemID.ApItem;
            Item[1].SetActive(true);
        }

        public void ShieldItem()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            isSelectedItem = true;
            for (int i = 0; i < Item.Length; ++i)
            {
                Item[i].SetActive(false);
            }
            e.hashTable["select_item_id"] = UI.ItemID.ShieldItem;
            evt.hashTable["select_item_id"] = UI.ItemID.ShieldItem;
            Item[2].SetActive(true);
        }
        public void OffItemSelected()
        {
            itemSelected.SetActive(false);
        }
        public void StartLobby()
        {
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.CharacterChangeButten;
            evt.hashTable["selected_character_type"] = Types.Character.Musa;
            Core.Event.Dispatcher.Dispatch(evt);
            StartSet();
        }
        public void StartSet()
        {
            Musa.SetActive(true);
            selectBack[0].SetActive(true);
            check = 1;
            Thief.SetActive(false);
            Archer.SetActive(false);
            Magician.SetActive(false);
            selectBack[1].SetActive(false);
            selectBack[2].SetActive(false);
            selectBack[3].SetActive(false);
            isSelectedItem = false;
            for (int i = 0; i < Item.Length; ++i)
            {
                Item[i].SetActive(false);
            }
        }
    }
}