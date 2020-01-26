using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    public partial class UI 
    {
        /// <summary>
        /// Battle In Area UI Manager
        /// </summary>
        public class Manager : Core.UI.Manager
        {
            public static UI.Manager uiManager;

            public override void Prepare()
            {
                base.Prepare();

                if (uiManager == null)
                {
                    uiManager = this;
                }
            }

            GameObject myItem;

            public InGameUIController GetIngameUI()
            {
                return Get<InGameUIController>((int)UI.ID.IngameUIController);
            }

            public bool ResultUIActive()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);

                if(ctn.firstResultImage.activeSelf || ctn.secondResultImage.activeSelf)
                {
                    return false;
                }
                return true;
            }
            public void WinSend()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.SetWinResultUI();
                SoundManager.instance.StopSE("Ingame(Clone)");
                SoundManager.instance.PlaySE("Sound/BGM/Victory");
            }

            public void LoseSend()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.SetDefeatResultUI();
                SoundManager.instance.StopSE("Ingame(Clone)");
                SoundManager.instance.PlaySE("Sound/BGM/Lose");
            }

            public void SetResultPlayerCheck(CharacterController myCharacter, CharacterController targetCharacter) //플레이어
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                if (targetCharacter.RemainHP() == 0)
                {
                    ctn.SetWinResultUI();
                    Debug.Log("SetResultPlayerCheck");
                    //결과창 UI를 띄우주기 위해 인게임 UI 컨트롤러 스크립트에 접속한다. (패배 결과창)
                }
                else if(myCharacter.RemainHP() == 0)
                {
                    ctn.SetDefeatResultUI();
                }
                
                //else
                //{
                //    ctn.SetFirstResultUI(1);
                //    //결과창 UI를 띄우주기 위해 인게임 UI 컨트롤러 스크립트에 접속한다. (승리 결과창)
                //}
            }
            public void Escape()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.EscapeCheck();
            }
            public void ResultInGameUIConnect() //적플레이어
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);

                ctn.SetWinResultUI();
            }
            public void SetFirstCharacter(Types.Character c, bool IsPlayer)
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.SetFirstCharacter(c);

                if (IsPlayer)
                {
                    ctn.SetFirstCharacterSKill(c);
                }
            }

            public void SetSecondCharacter(Types.Character c, bool IsPlayer)
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.SetSecondCharacter(c);

                if (IsPlayer)
                {
                    ctn.SetSecondCharacterSkill(c);
                }
            }
            public void SetSlider(CharacterController player, CharacterController enemy)
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.SetSlider(player, enemy);
            }

            public void UpdatePlayerSlider(CharacterController player)
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.UpdatePlayerApSlider(player);
                ctn.UpdatePlayerHpSlider(player);
            }
            public void UpdateOtherSlider(CharacterController enemy)
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.UpdateTargetApSlider(enemy);
                ctn.UpdateTargetHpSlider(enemy);
            }
            public void FirstPlayerTurn()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.FirstItemEnable("true");
                ctn.SecondItemEnable("false");
                ctn.FirstSkillEnable("true");
                ctn.SecondSkillEnable("false");
            }
            public void FirstPlayerUIColor()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.SetColorWhite(ctn.secondPlayerImage); 
                ctn.SetColorDark(ctn.firstPlayerImage); 
            }
            public void SecondPlayerUIColor()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.SetColorWhite(ctn.firstPlayerImage);
                ctn.SetColorDark(ctn.secondPlayerImage);
            }
            public void SecondPlayerTurn()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.FirstItemEnable("false");
                ctn.SecondItemEnable("true");
                ctn.FirstSkillEnable("false");
                ctn.SecondSkillEnable("true");
            }
            public void TurnEnableTrue()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.changeTurnButton.SetActive(true);
            }
            public void TurnEnableFalse()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.changeTurnButton.SetActive(false);
            }
           
            public void SetItemImage(UI.ItemID item)
            {
                int ItemList;
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                if(PhotonNetwork.isMasterClient)
                {
                    ItemList = 0;
                }
                else
                {
                    ItemList = 1;
                }
                myItem = ctn.itemList[ItemList].transform.GetChild((int)item).gameObject;
                myItem.SetActive(true);
            }
            public void SetFirstCharacterStat(CharacterController character)
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.SetFirstCharacterMax(character);
            }
            public void SetSecondCharacterStat(CharacterController character)
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.SetSecondCharacterMax(character);
            }

            public void UsingItem()
            {
                myItem.SetActive(false);
            }



            public void APChecker()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.APCheck();
            }
            public void HPFull()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.HpTrue();
            }
            public void APFull()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);
                ctn.ApTrue();
            }

            public void FirstSkillOnUpdate(CharacterController player)
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);

                ctn.FirstSkillUpdate(player);
            }
            public void SecondSkillOnUpdate(CharacterController player)
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);

                ctn.SecondSkillUpdate(player);
            }
            public void EndGame()
            {
                InGameUIController ctn = Get<InGameUIController>((int)UI.ID.IngameUIController);

                ctn.EndResult();
            }
            
            public void StartLobby()
            {
                LobbyUIController ctn = Get<LobbyUIController>((int)UI.ID.LobbyUIController);

                ctn.StartLobby();
            }
            public void SetRoomName(string selectedRoom)
            {
                RoomListController ctn = Get<RoomListController>((int)UI.ID.RoomListController);
                ctn.OnClickRoomButton(selectedRoom);
            }

            public void GetMyInfo(Types.Character type, UI.ItemID item)
            {
                RoomListController ctn = Get<RoomListController>((int)UI.ID.RoomListController);

                ctn.GetMyInfo(type, item);
            }

            public void HideLoding()
            {
                RoomListController ctn = Get<RoomListController>((int)UI.ID.RoomListController);

                ctn.HideLoding();
            }
            
            public void StartGame()
            {
                RoomListController ctn = Get<RoomListController>((int)UI.ID.RoomListController);

                ctn.StartGame();
            }
        }
    }
}