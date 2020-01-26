

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BattleInArea.Game
{
    [RequireComponent(typeof(Board))]
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(CharacterData))]

    public partial class BattleInAreaClient : Core.Client
    {

        protected string[] characters = new string[] { "Characters/Warrior/Warrior", "Characters/Thief/Thief", "Characters/Archer/Archer", "Characters/Magicion/Magicion" };


        protected DataBase.Manager databaseManager;
        protected CharacterData characterData;
        protected Page.Manager pageManager;
        protected SoundManager soundManager;
        protected InputManager inputManager;
        protected UI.Manager uiManager;
        protected Board.Manager boardManager;
        protected Core.Event.Dispatcher dispatcher;
        protected CharacterController ctn;
        protected int selectedSkill;
        protected string classType;
        protected int buttonCount;
        protected int turnCount = 0;
        protected bool isSetStats = false;

        protected bool isMyTurn = false;

        protected bool isChangeTurn;

     


        protected Character.Manager characterManager;


        /// <summary>
        /// 
        /// </summary>
        protected override void Awake()
        {
            databaseManager = new DataBase.Manager();
            pageManager = new Page.Manager();
            soundManager = new SoundManager();
            inputManager = new InputManager();
            uiManager = new UI.Manager();
            boardManager = new Board.Manager();
            dispatcher = new Core.Event.Dispatcher(this);
            characterManager = new Character.Manager();
            characterData = new CharacterData();
            databaseManager.Prepare();
            pageManager.Prepare();
            uiManager.Prepare();
            characterManager.Prepare();
            //boardManager.Prepare();

        }

        protected void Start()
        {
            pageManager.Chage((int)Page.ID.Title);

        }

        /// <summary>
        /// 
        /// </summary>
        protected void Update()
        {
            pageManager.OnUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evt"></param>
        public override void Receive(Core.Event.IData evt)
        {
            switch (evt.ID)
            {

                case (int)Event.ID.Selected:
                    {
                        Event.Selected e = evt as Event.Selected;
                        Event.SelectedType type = (Event.SelectedType)e.hashTable["select_type"];

                        switch (type)
                        {
                            case Event.SelectedType.UI:
                                {
                                    UI.ID ui_id = (UI.ID)e.hashTable["select_ui_id"];
                                    switch (ui_id)
                                    {
                                        case UI.ID.TitleButton:
                                            {
                                                pageManager.Chage((int)Page.ID.MainMenu);
                                            }
                                            break;
                                        case UI.ID.GameStartButton:
                                            {
                                                SoundManager.instance.StopSE("Lobby(Clone)");
                                                pageManager.Chage((int)Page.ID.RoomList);
                                                //pageManager.Chage((int)Page.ID.InGame);
                                            }
                                            break;
                                        case UI.ID.CharacterChangeButten:
                                            {
                                                Types.Character selectedCharacter = (Types.Character)e.hashTable["selected_character_type"];
                                                UpdateCharacterInfo(selectedCharacter);
                                            }
                                            break;
                                        case UI.ID.CreditButton:
                                            {
                                                pageManager.Chage((int)Page.ID.MainMenu);
                                            }
                                            break;
                                        case UI.ID.MainMenuStartButton:
                                            {
                                                AwakeServer();
                                                StartCoroutine(Process());
                                                //PhotonNetwork.Reconnect();
                                                pageManager.Chage((int)Page.ID.Lobby);
                                            }
                                            break;
                                        case UI.ID.MainMenuCreditButton:
                                            {
                                                pageManager.Chage((int)Page.ID.Credit);
                                            }
                                            break;
                                        case UI.ID.MainMenuBookButton:
                                            {
                                                pageManager.Chage((int)Page.ID.Book);
                                            }
                                            break;
                                        case UI.ID.ChangeTurnButton:
                                            {
                                                CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
                                                CharacterController targetCharacter = characterManager.GetOther();
                                                if (myCharacter.IsPlayAnimation())
                                                {
                                                    return;
                                                }
                                                myCharacter.NextTurnAP();
                                                photonView.RPC("RpcNextTurnAP", PhotonTargets.Others, myCharacter.PlayerID);
                                                uiManager.TurnEnableFalse();
                                                photonView.RPC("RpcChangeButton", PhotonTargets.MasterClient);
                                            }
                                            break;
                                        case UI.ID.JoinTheRoom:
                                            {
                                                pageManager.Chage((int)Page.ID.InGame);
                                            }
                                            break;
                                        case UI.ID.GetMyInfo:
                                            {
                                                Types.Character myCharacterType = (Types.Character)e.hashTable["selected_character_type"];
                                                UI.ItemID myItem = (UI.ItemID)e.hashTable["select_item_id"];

                                                uiManager.GetMyInfo(myCharacterType, myItem);
                                            }
                                            break;
                                        case UI.ID.SkillButton:
                                            {
                                                CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
                                                CharacterController targetCharacter = characterManager.GetOther();
                                                Types.SkillButton skillID = (Types.SkillButton)e.hashTable["select_skill_id"];

                                                int playerID = PhotonNetwork.player.ID;
                                                Types.Character characterType = (Types.Character)PhotonNetwork.player.CustomProperties["character_id"];
                                                myCharacter.isSelected = false;
                                                myCharacter.OnCollider();
                                                if(myCharacter.skeletonAnimation.AnimationName != "idle") { return; }
                                                switch (characterType)
                                                {
                                                    case Types.Character.Musa:
                                                        {
                                                            boardManager.ShowSkillDisplay(boardManager.GetAllTiles(), "TileNomalTexture");
                                                            Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(boardManager.GetAllTiles()), Types.Tile.Nomal);

                                                            classType = "Musa";
                                                            switch (skillID)
                                                            {
                                                                case Types.SkillButton.Skill01:
                                                                    {

                                                                        if (!CanUsable(myCharacter.PlayerID, characterData.GetData("Musa").skill01ap))
                                                                        {
                                                                            APChecker();
                                                                            return;
                                                                        }
                                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill01Tiles"), "TileSkillTexture");
                                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill01Tiles")), Types.Tile.Skill);
                                                                        selectedSkill = 1;
                                                                        buttonCount = 0;
                                                                    }
                                                                    break;
                                                                case Types.SkillButton.Skill02:
                                                                    {
                                                                        if (!CanUsable(myCharacter.PlayerID, characterData.GetData("Musa").skill02ap))
                                                                        {

                                                                            APChecker();
                                                                            return;
                                                                        }
                                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill02Tiles"), "TileSkillTexture");
                                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill02Tiles")), Types.Tile.Skill);
                                                                        selectedSkill = 2;
                                                                        buttonCount = 0;
                                                                    }
                                                                    break;
                                                                case Types.SkillButton.Skill03:
                                                                    {
                                                                        if (!CanUsable(myCharacter.PlayerID, characterData.GetData("Musa").skill03ap))
                                                                        {

                                                                            APChecker();
                                                                            return;
                                                                        }
                                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill03Tiles"), "TileSkillTexture");
                                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill03Tiles")), Types.Tile.Skill);
                                                                        selectedSkill = 3;
                                                                 
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case Types.Character.Thief:
                                                        {
                                                            boardManager.ShowSkillDisplay(boardManager.GetAllTiles(), "TileNomalTexture");
                                                            Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(boardManager.GetAllTiles()), Types.Tile.Nomal);
                                                            classType = "Thief";
                                                            switch (skillID)
                                                            {
                                                                case Types.SkillButton.Skill01:
                                                                    {

                                                                        if (!CanUsable(myCharacter.PlayerID, characterData.GetData("Thief").skill01ap))
                                                                        {
                                                                            APChecker();
                                                                            return;
                                                                        }
                                                                        selectedSkill = 1;
                                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill01Tiles"), "TileSkillTexture");
                                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill01Tiles")), Types.Tile.Skill);
                                                                    }
                                                                    break;
                                                                case Types.SkillButton.Skill02:
                                                                    {
                                                                        if (!CanUsable(myCharacter.PlayerID, characterData.GetData("Thief").skill02ap))
                                                                        {

                                                                            APChecker();
                                                                            return;
                                                                        }
                                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill02Tiles"), "TileSkillTexture");
                                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill02Tiles")), Types.Tile.Skill);
                                                                        selectedSkill = 2;
                                                                        buttonCount = 0;
                                                                    }
                                                                    break;
                                                                case Types.SkillButton.Skill03:
                                                                    {
                                                                        if (!CanUsable(myCharacter.PlayerID, characterData.GetData("Thief").skill03ap))
                                                                        {

                                                                            APChecker();
                                                                            return;
                                                                        }
                                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill03Tiles"), "TileSkillTexture");
                                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill03Tiles")), Types.Tile.Skill);
                                                                        selectedSkill = 3;
                                                                        buttonCount = 0;
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case Types.Character.Archer:
                                                        {
                                                            boardManager.ShowSkillDisplay(boardManager.GetAllTiles(), "TileNomalTexture");
                                                            Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(boardManager.GetAllTiles()), Types.Tile.Nomal);
                                                            classType = "Archer";
                                                            switch (skillID)
                                                            {
                                                                case Types.SkillButton.Skill01:
                                                                    {
                                                                        if (!CanUsable(myCharacter.PlayerID, characterData.GetData("Archer").skill01ap))
                                                                        {

                                                                            APChecker();
                                                                            return;
                                                                        }
                                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill01Tiles"), "TileSkillTexture");
                                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill01Tiles")), Types.Tile.Skill);
                                                                        selectedSkill = 1;
                                                                        buttonCount = 0;
                                                                    }
                                                                    break;
                                                                case Types.SkillButton.Skill02:
                                                                    {
                                                                        if (!CanUsable(myCharacter.PlayerID, characterData.GetData("Archer").skill02ap))
                                                                        {

                                                                            APChecker();
                                                                            return;
                                                                        }
                                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill02Tiles"), "TileSkillTexture");
                                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill02Tiles")), Types.Tile.Skill);
                                                                        selectedSkill = 2;
                                                                        buttonCount = 0;
                                                                    }
                                                                    break;
                                                                case Types.SkillButton.Skill03:
                                                                    {
                                                                        if (!CanUsable(myCharacter.PlayerID, characterData.GetData("Archer").skill03ap))
                                                                        {

                                                                            APChecker();
                                                                            return;
                                                                        }
                                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill03Tiles"), "TileSkillTexture");
                                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill03Tiles")), Types.Tile.Skill);
                                                                        selectedSkill = 3;
                                                                        buttonCount = 0;
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        break;
                                                    case Types.Character.Magician:
                                                        {
                                                            boardManager.ShowSkillDisplay(boardManager.GetAllTiles(), "TileNomalTexture");
                                                            Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(boardManager.GetAllTiles()), Types.Tile.Nomal);
                                                            classType = "Magician";
                                                            switch (skillID)
                                                            {
                                                                case Types.SkillButton.Skill01:
                                                                    {
                                                                        if (!CanUsable(myCharacter.PlayerID, characterData.GetData("Magician").skill01ap))
                                                                        {

                                                                            APChecker();
                                                                            return;
                                                                        }
                                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill01Tiles"), "TileSkillTexture");
                                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill01Tiles")), Types.Tile.Skill);
                                                                        selectedSkill = 1;
                                                                        buttonCount = 0;
                                                                    }
                                                                    break;
                                                                case Types.SkillButton.Skill02:
                                                                    {
                                                                        if (!CanUsable(myCharacter.PlayerID, characterData.GetData("Magician").skill02ap))
                                                                        {

                                                                            APChecker();
                                                                            return;
                                                                        }
                                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill02Tiles"), "TileSkillTexture");
                                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill02Tiles")), Types.Tile.Skill);
                                                                        selectedSkill = 2;
                                                                        buttonCount = 0;
                                                                    }
                                                                    break;
                                                                case Types.SkillButton.Skill03:
                                                                    {
                                                                        if (!CanUsable(myCharacter.PlayerID, characterData.GetData("Magician").skill03ap))
                                                                        {
                                                                            APChecker();
                                                                            return;
                                                                        }
                                                                        selectedSkill = 3;
                                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill03Tiles"), "TileSkillTexture");
                                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill03Tiles")), Types.Tile.Skill);
                                                                    }
                                                                    break;
                                                            }
                                                        }

                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                }
                                break;
                            case Event.SelectedType.Tile:
                                {
                                    if (!isMyTurn)
                                    {
                                        return;
                                    }

                                    buttonCount = 0;
                                    Board.TileID tileID = (Board.TileID)e.hashTable["select_tile_id"];
                                    TileType.ID OnSelectedTile = (TileType.ID)e.hashTable["select_tile"];
                                    Debug.Log(boardManager.IsSkillRange);
                                    CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
                                    CharacterController targetCharacter = characterManager.GetOther();
                                    bool LeftorRight = tileID.z > myCharacter.CharPos().z;
                                    boardManager.SetDisplay(boardManager.GetAllTiles(), "TileNomalTexture");
                                    Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(boardManager.GetAllTiles()), Types.Tile.Nomal);

                                    //if (myCharacter.isSelected)
                                    //{
                                    //    myCharacter.OnCollider();
                                    //}
                                    switch (OnSelectedTile)
                                    {

                                        case TileType.ID.Nomal:
                                            {
                                                int x;
                                                int z;

                                                x = tileID.x;
                                                z = tileID.z;

                                                //if(boardManager.IsSkillRange == false && boardManager.IsMoveRange == false)
                                                //{
                                                //    return;
                                                //}
                                                if(PhotonNetwork.player.ID != myCharacter.PlayerID) { return; }
                                                if(myCharacter.isSelected == true)
                                                {
                                                    if (!myCharacter.IsMovableTile(x, z))
                                                    {
                                                        myCharacter.OnCollider();
                                                        boardManager.IsMoveRange = false;
                                                    }
                                                }

                                                if (myCharacter.IsMovableTile(x, z) && myCharacter.isSelected && IsEnemyPos(targetCharacter.CharPos(), x, z) && boardManager.IsMoveRange == true)
                                                {
                                                    boardManager.CanMoveDisplay(boardManager.GetAllTiles(), "TileNomalTexture");
                                                    Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(boardManager.GetAllTiles()), Types.Tile.Nomal);
                                                    //  State 수정
                                                    //myCharacter.SetPosition(x, z);
                                                    LookAtTile(z);
                                                    myCharacter.SetMoveTile(x, z);
                                                    myCharacter.ChangeMoveState();
                                                    myCharacter.isSelected = false;

                                                    Debug.Log("이동 타일 선택");
                                                }
                                                #region 스킬 시작
                                                if (boardManager.IsSkillRange == true)
                                                {
                                                    Debug.Log("스킬 타일 선택");
                                                    myCharacter.SetSkillTile(x, z);

                                                    #region 스킬1번 시작
                                                    if (selectedSkill == 1 && myCharacter.IsSkillTile(x, z, "skill01Tiles"))
                                                    {
                                                        LookAtTile(z);
                                                        //무사 - 베기(부분 스킬)
                                                        if (classType == "Musa")
                                                        {
                                                            Debug.Log("무사 스킬!");

                                                            //오른쪽 스킬
                                                            if (LeftorRight)
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill01Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z < targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();

                                                                    Debug.Log("오른쪽 스킬!");
                                                                   
                                                                    targetCharacter.usedShield = false;
                                                                }
                                                                

                                                                //오른쪽 스킬 범위 내에 타겟 없을 시
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }

                                                            //왼쪽 스킬
                                                            else
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill01Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z > targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                    Debug.Log("왼쪽 스킬!");
                                                                }
                                                                //왼쪽 스킬 범위 내에 타겟 없을 시
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }
                                                        }

                                                        //도적 - 마구 할퀴기(전체 스킬)
                                                        else if (classType == "Thief")
                                                        {
                                                            if (myCharacter.IsEnemyInTiles("skill01Tiles", targetCharacter.CharPos()))
                                                            {
                                                                myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                myCharacter.ChangeUsingSkillState();
                                                            }
                                                            else
                                                            {
                                                                myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                myCharacter.ChangeMissState();
                                                            }

                                                        }

                                                        //궁수 - 에로우 레인(부분 스킬)
                                                        else if (classType == "Archer")
                                                        {
                                                            //오른쪽 스킬
                                                            if (LeftorRight)
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill01Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z < targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                    Debug.Log("오른쪽 스킬!");
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }

                                                            //왼쪽 스킬
                                                            else
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill01Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z > targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                    Debug.Log("왼쪽 스킬!");
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }

                                                        }

                                                        //마법사 - 찌리릿!(부분 스킬)
                                                        else if (classType == "Magician")
                                                        {
                                                            //오른쪽 스킬
                                                            if (LeftorRight)
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill01Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z < targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                    Debug.Log("오른쪽 스킬!");
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }

                                                            //왼쪽 스킬
                                                            else
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill01Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z > targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                    Debug.Log("왼족 스킬!");
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }
                                                        }
                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill01Tiles"), "TileNomalTexture");
                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill01Tiles")), Types.Tile.Nomal);
                                                        boardManager.IsSkillRange = false;
                                                    }
                                                    #endregion 스킬1번 끝
                                                    #region 스킬2번 시작
                                                    else if (selectedSkill == 2 && myCharacter.IsSkillTile(x, z, "skill02Tiles"))
                                                    {
                                                        LookAtTile(z);
                                                        //무사 - 일섬(부분 스킬)
                                                        if (classType == "Musa")
                                                        {
                                                            //오른쪽
                                                            if (LeftorRight)
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill02Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z < targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetMoveTile(x, z);
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingMoveSkillState();
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetMoveTile(x, z);
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMoveSkillMissState();
                                                                }
                                                            }

                                                            //왼쪽
                                                            else
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill02Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z > targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetMoveTile(x, z);
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingMoveSkillState();
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetMoveTile(x, z);
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMoveSkillMissState();
                                                                }
                                                            }

                                                        }

                                                        //도적 - 뒤잡기(부분 스킬)
                                                        else if (classType == "Thief")
                                                        {
                                                            //오른쪽 스킬
                                                            if (LeftorRight)
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill02Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z < targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetMoveTile(x, z);
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingMoveSkillState();
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetMoveTile(x, z);
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeMoveSkillMissState();
                                                                }
                                                            }

                                                            //왼쪽 스킬
                                                            else
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill02Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z > targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetMoveTile(x, z);
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingMoveSkillState();
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetMoveTile(x, z);
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeMoveSkillMissState();
                                                                }
                                                            }

                                                        }

                                                        //궁수 - 멀티샷(부분 스킬)
                                                        else if (classType == "Archer")
                                                        {
                                                            //오른쪽 스킬
                                                            if (LeftorRight)
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill02Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z < targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }

                                                            //왼쪽 스킬
                                                            else
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill02Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z > targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }


                                                        }

                                                        //마법사 - 용용이!(부분 스킬)
                                                        else if (classType == "Magician")
                                                        {
                                                            //오른쪽 스킬
                                                            if (LeftorRight)
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill02Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z < targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }

                                                            //왼쪽 스킬
                                                            else
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill02Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z > targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }
                                                        }
                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill02Tiles"), "TileNomalTexture");
                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill02Tiles")), Types.Tile.Nomal);
                                                        boardManager.IsSkillRange = false;
                                                    }
                                                    #endregion 스킬2번 끝
                                                    #region 스킬3번 시작
                                                    else if (selectedSkill == 3 && myCharacter.IsSkillTile(x, z, "skill03Tiles"))
                                                    {
                                                        LookAtTile(z);
                                                        //무사 - 만개(전체 스킬)
                                                        if (classType == "Musa")
                                                        {
                                                            if (myCharacter.IsEnemyInTiles("skill03Tiles", targetCharacter.CharPos()))
                                                            {
                                                                myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                myCharacter.ChangeUsingSkillState();
                                                            }
                                                            else
                                                            {
                                                                myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                myCharacter.ChangeMissState();
                                                            }


                                                        }

                                                        //도적 - 암살(부분 스킬)
                                                        else if (classType == "Thief")
                                                        {
                                                            //오른쪽 스킬
                                                            if (LeftorRight)
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill03Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z < targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                }
                                                                else
                                                                {
                     
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }

                                                            //왼쪽 스킬
                                                            else
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill03Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z > targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }


                                                        }

                                                        //궁수 - 일격필중(부분 스킬)
                                                        else if (classType == "Archer")
                                                        {
                                                            //오른쪽 스킬
                                                            if (LeftorRight)
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill03Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z < targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }

                                                            //왼쪽 스킬
                                                            else
                                                            {
                                                                if (myCharacter.IsEnemyInTiles("skill03Tiles", targetCharacter.CharPos()) && myCharacter.CharPos().z > targetCharacter.CharPos().z)
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                    myCharacter.ChangeUsingSkillState();
                                                                }
                                                                else
                                                                {
                                                                    myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                    myCharacter.ChangeMissState();
                                                                }
                                                            }

                                                        }

                                                        //마법사 - 피~날레!(전체 스킬)
                                                        else if (classType == "Magician")
                                                        {
                                                            if (myCharacter.IsEnemyInTiles("skill03Tiles", targetCharacter.CharPos()))
                                                            {
                                                                myCharacter.SetSelectedSkillIndex(selectedSkill);

                                                                myCharacter.ChangeUsingSkillState();
                                                            }
                                                            else
                                                            {
                                                                myCharacter.SetSelectedSkillIndex(selectedSkill);
                                                                myCharacter.ChangeMissState();
                                                            }

                                                        }
                                                        boardManager.ShowSkillDisplay(myCharacter.GetSkillTiles("skill03Tiles"), "TileNomalTexture");
                                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetSkillTiles("skill03Tiles")), Types.Tile.Nomal);
                                                        boardManager.IsSkillRange = false;
                                                    }
                                                    #endregion 스킬3번 끝

                                                }
                                                boardManager.IsSkillRange = false;
                                                #endregion 스킬 끝
                                            }
                                            break;
                                    }
                                }
                                break;
                            case Event.SelectedType.Character:
                                {
                                    CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
                                    if (myCharacter.isSelected)
                                    {
                                        myCharacter.OffCollider();
                                        if (boardManager.IsSkillRange == true)
                                        {
                                            boardManager.SetDisplay(boardManager.GetAllTiles(), "TileNomalTexture");
                                            Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(boardManager.GetAllTiles()), Types.Tile.Nomal);
                                            boardManager.IsSkillRange = false;
                                        }
                                        boardManager.CanMoveDisplay(myCharacter.GetMovableTiles(), "TileMoveTexture");
                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(myCharacter.GetMovableTiles()), Types.Tile.Move);
                                        Debug.Log("플레이어 캐릭터 눌렸다!");
                                    }
                                    else
                                    {
                                        boardManager.SetDisplay(boardManager.GetAllTiles(), "TileNomalTexture");
                                        Helper.RPC.Send("RPCShowDisplay", PhotonTargets.Others, boardManager.GetTileVector(boardManager.GetAllTiles()), Types.Tile.Nomal);
                                    }
                                }
                                break;
                            case Event.SelectedType.Item:
                                {
                                    UI.ItemID item = (UI.ItemID)e.hashTable["select_item_id"];
                                    string itemName;
                                    switch (item)
                                    {
                                        case UI.ItemID.HpItem:
                                            {
                                                itemName = "HpItem";
                                                CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
                                                CharacterController targetCharacter = characterManager.GetOther();
                                                if(myCharacter.HP + 1200 > CharacterData.instance.GetData(myCharacter.CharacterType.ToString()).hp)
                                                {
                                                    uiManager.HPFull();
                                                    return;
                                                }
                                                if (myCharacter.skeletonAnimation.AnimationName != "idle") { return; }
                                                myCharacter.GiveHeal(1200);

                                                staticEffectManager.instance.SetItemEffect(itemName, myCharacter, targetCharacter);
                                                Helper.RPC.Send("RpcSetItemEffect", PhotonTargets.Others, itemName, targetCharacter.PlayerID, myCharacter.PlayerID); //ctn = MyPlayer

                                                myCharacter.FloatingTextOnEnter3(1200);//플로팅 텍스트 체력편
                                                photonView.RPC("RPCHPItem", PhotonTargets.Others);
                                                uiManager.UsingItem();
                                                
                                            }
                                            break;
                                        case UI.ItemID.ApItem:
                                            {
                                                itemName = "ApItem";
                                                CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
                                                CharacterController targetCharacter = characterManager.GetOther();
                                                if (myCharacter.AP + 30 > CharacterData.instance.GetData(myCharacter.CharacterType.ToString()).maxap)
                                                {
                                                    uiManager.APFull();
                                                    return;
                                                }
                                                if (myCharacter.skeletonAnimation.AnimationName != "idle") { return; }

                                                myCharacter.GiveAP(30);

                                                staticEffectManager.instance.SetItemEffect(itemName, myCharacter, targetCharacter);
                                                Helper.RPC.Send("RpcSetItemEffect", PhotonTargets.Others, itemName, targetCharacter.PlayerID, myCharacter.PlayerID); //ctn = MyPlayer

                                                myCharacter.FloatingTextOnEnter2(30); //플로팅 텍스트 행동력편
                                                uiManager.UpdatePlayerSlider(myCharacter);
                                                photonView.RPC("RPCAPItem", PhotonTargets.Others);
                                                uiManager.UsingItem();
                                                
                                            }
                                            break;
                                        case UI.ItemID.ShieldItem:
                                            {
                                                CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
                                                CharacterController targetCharacter = characterManager.GetOther();

                                                itemName = "ShieldItem";
                                                if (myCharacter.skeletonAnimation.AnimationName != "idle") { return; }
                                                myCharacter.usedShield = true;
                                                staticEffectManager.instance.SetItemEffect(itemName, myCharacter, targetCharacter);

                                                photonView.RPC("RPCShieldItem", PhotonTargets.Others);
                                                uiManager.UsingItem();
                                                
                                            }
                                            break;
                                    }
                                    break;
                                }
                       
                        }
                    }
                    break;
                case (int)Event.ID.PageModify:
                    {
                        Event.PageModify pageModify = evt as Event.PageModify;
                        Event.PageState page = (Event.PageState)pageModify.hashtable["state"];
                        Page.ID id = (Page.ID)pageModify.hashtable["page"];

                        switch (page)
                        {
                            case Event.PageState.OnEnter:
                                {
                                    switch (id)
                                    {
                                        case Page.ID.Title:
                                            {

                                                uiManager.Hide((int)UI.ID.LodingUIController);
                                                SoundManager.instance.PlaySE("Sound/BGM/Intro&MainMenu");
                                                uiManager.Show((int)UI.ID.TitleUIController);
                                            }
                                            break;
                                        case Page.ID.Lobby:
                                            {
                                                uiManager.Show((int)UI.ID.LobbyUIController);
                                                uiManager.Hide((int)UI.ID.LodingUIController);
                                                SoundManager.instance.StopSE("Intro&MainMenu(Clone)");
                                                SoundManager.instance.PlaySE("Sound/BGM/Lobby");
                                                uiManager.StartLobby();
                                            }
                                            break;
                                        case Page.ID.Loding:
                                            {
                                            }
                                            break;
                                        case Page.ID.InGame:
                                            {
                                                SoundManager.instance.PlaySE("Sound/BGM/Ingame");
                                                isConnected = true;
                                                StartCoroutine(EnterInGamePage());
                                                if (PhotonNetwork.isMasterClient)
                                                {
                                                    StartCoroutine(ServerLoop());
                                                }
                                            }
                                            break;
                                        case Page.ID.Credit:
                                            {

                                                uiManager.Hide((int)UI.ID.LodingUIController);
                                                uiManager.Show((int)UI.ID.CreditUIController);
                                                SoundManager.instance.StopSE("Intro&MainMenu(Clone)");
                                                SoundManager.instance.PlaySE("Sound/BGM/Credit");
                                            }
                                            break;
                                        case Page.ID.MainMenu:
                                            {
                                                uiManager.Hide((int)UI.ID.LodingUIController);
                                                uiManager.Show((int)UI.ID.MainMenuUIController);
                                                SoundManager.instance.StopSE("Intro&MainMenu(Clone)");
                                                SoundManager.instance.PlaySE("Sound/BGM/Intro&MainMenu");
                                            }
                                            break;

                                        case Page.ID.Book:
                                            {
                                                uiManager.Hide((int)UI.ID.LodingUIController);
                                                uiManager.Show((int)UI.ID.BookUIController);
                                            }
                                            break;
                                        case Page.ID.RoomList:
                                            {
                                                uiManager.HideLoding();
                                                uiManager.Hide((int)UI.ID.LodingUIController);
                                                uiManager.Show((int)UI.ID.RoomListController);
                                            }
                                            break;
                                    }
                                }
                                break;
                            case Event.PageState.OnExit:
                                {
                                    switch (id)
                                    {
                                        case Page.ID.Title:
                                            {

                                                uiManager.Show((int)UI.ID.LodingUIController);
                                                uiManager.Hide((int)UI.ID.TitleUIController);
                                            }
                                            break;
                                        case Page.ID.Lobby:
                                            {

                                                //  !! 로비에서 룸 Join 동기화 문제와 코루틴 문제로 
                                                //  EnterInGamePage  함수에서 Hide 함
                                                uiManager.Hide((int)UI.ID.LobbyUIController);
                                            
                                            }
                                            break;

                                        case Page.ID.Loding:
                                            {
                                            }
                                            break;

                                        case Page.ID.InGame:
                                            {
                                                isStartGame = false;
                                                IsCreateCharacter = false;
                                                boardManager.AllTileDel();
                                                PhotonNetwork.Disconnect();
                                                uiManager.Show((int)UI.ID.LodingUIController);
                                                SoundManager.instance.StopSE("Victory(Clone)");
                                                SoundManager.instance.StopSE("Lose(Clone)");
                                                Character.Manager.Single.Clear();
                                                uiManager.EndGame();
                                                uiManager.Hide((int)UI.ID.IngameUIController);
                                                uiManager.StartGame();
                                            }
                                            break;
                                        case Page.ID.Credit:
                                            {

                                                uiManager.Show((int)UI.ID.LodingUIController);
                                                uiManager.Hide((int)UI.ID.CreditUIController);
                                                SoundManager.instance.StopSE("Credit(Clone)");
                                                SoundManager.instance.PlaySE("Sound/BGM/Intro&MainMenu");
                                            }
                                            break;
                                        case Page.ID.MainMenu:
                                            {

                                                uiManager.Show((int)UI.ID.LodingUIController);
                                                uiManager.Hide((int)UI.ID.MainMenuUIController);
                                             
                                            }
                                            break;
                                        case Page.ID.Book:
                                            {

                                                uiManager.Show((int)UI.ID.LodingUIController);
                                                uiManager.Hide((int)UI.ID.BookUIController);
                                            }
                                            break;
                                        case Page.ID.RoomList:
                                            {
                                                uiManager.Hide((int)UI.ID.RoomListController);
                                                uiManager.Show((int)UI.ID.LodingUIController);
                                            }
                                            break;
                                    }
                                }
                                break;
                            case Event.PageState.OnExecute:
                                {
                                    switch (id)
                                    {
                                        case Page.ID.InGame:
                                            {
                                                inputManager.Click();
                                                CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
                                                CharacterController targetCharacter = characterManager.GetOther();
                                                if (myCharacter == null && targetCharacter == null) { return; }


                                                if (IsCreateCharacter)
                                                {
                                                    if (PhotonNetwork.isMasterClient)
                                                    {
                                                        if (isSetStats == true)
                                                        {
                                                            Debug.Log("myCharacter" + myCharacter);
                                                            Debug.Log("targetCharacter" + targetCharacter);
                                                            uiManager.SetFirstCharacterStat(myCharacter);
                                                            uiManager.SetSecondCharacterStat(targetCharacter);
                                                            isSetStats = false;
                                                        }
                                                        uiManager.UpdatePlayerSlider(myCharacter);
                                                        uiManager.UpdateOtherSlider(targetCharacter);
                                                        uiManager.FirstSkillOnUpdate(myCharacter);
                                                    }
                                                    else
                                                    {
                                                        if (isSetStats == true)
                                                        {
                                                            Debug.Log("myCharacter" + myCharacter);
                                                            Debug.Log("targetCharacter" + targetCharacter);
                                                            uiManager.SetFirstCharacterStat(myCharacter);
                                                            uiManager.SetSecondCharacterStat(targetCharacter);
                                                            isSetStats = false;
                                                        }
                                                        uiManager.UpdatePlayerSlider(targetCharacter);
                                                        uiManager.UpdateOtherSlider(myCharacter);
                                                        uiManager.SecondSkillOnUpdate(myCharacter);
                                                    }
                                                    UnearnedWin();

                                                }
                                                if (targetCharacter != null)
                                                {
                                                    LookAtTarget();
                                                }

                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case (int)Event.ID.TurnModify:
                    {
                        Event.TurnModify e = evt as Event.TurnModify;
                        int playerID = (int)e.hashtable["player_id"];

                        photonView.RPC("RpcChangeTurn", PhotonTargets.All, playerID);
                    }
                    break;
                case (int)Event.ID.Item:
                    {
                        Event.Item e = evt as Event.Item;
                        UI.ItemID item = (UI.ItemID)e.hashTable["select_item_id"];
                        CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
                        CharacterController targetCharacter = characterManager.GetOther();

                        switch (item)
                        {
                            case UI.ItemID.HpItem:
                                {
                                    StartCoroutine(Hp());
                                }
                                break;
                            case UI.ItemID.ApItem:
                                {
                                    StartCoroutine(Ap());
                                }
                                break;
                            case UI.ItemID.ShieldItem:
                                {
                                    StartCoroutine(Shield());
                                }
                                break;
                        }
                    }
                    break;
                case (int)Event.ID.Room:
                    {
                        Event.Room room = evt as Event.Room;
                        uiManager.SetRoomName(room.hashTable["selected_room"].ToString());
                    }
                    break;
                case (int)Event.ID.GameResult:
                    {
                        Event.GameResult e = evt as Event.GameResult;
                        Event.Result result = (Event.Result)e.hashTable["result_type"];
                        int playerID = (int)e.hashTable["character_id"];
                        CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
                        CharacterController targetCharacter = characterManager.GetOther();
                        switch (result)
                        {
                            case Event.Result.Alive:
                                {
                                }
                                break;
                            case Event.Result.Die:
                                {
                                    if(PhotonNetwork.isMasterClient)
                                    {
                                        if(PhotonNetwork.player.ID == playerID)
                                        {
                                            uiManager.LoseSend();
                                            photonView.RPC("RpcWinSend", PhotonTargets.Others);
                                        }
                                        else
                                        {
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if(PhotonNetwork.player.ID == playerID)
                                        {
                                            uiManager.LoseSend();
                                            photonView.RPC("RpcWinSend", PhotonTargets.Others);
                                        }
                                        else
                                        {
                                            return;
                                        }
                                    }
                                }
                                break;
                        }

                    }
                    break;
                case (int)Event.ID.Event:
                    {
                        Event.OnEvent e = evt as Event.OnEvent;
                        Event.EventType eventType = (Event.EventType)e.hashTable["event_type"];

                        CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
                        CharacterController targetCharacter = characterManager.GetOther();

                        switch (eventType)
                        {
                            case Event.EventType.AllUsedAP:
                                {
                                    if (myCharacter.AP == 0)
                                    {
                                        myCharacter.NextTurnAP();
                                        myCharacter.OffCollider();
                                    }
                                    else
                                    {
                                        targetCharacter.NextTurnAP();
                                        targetCharacter.OffCollider();
                                    }
                                    if (PhotonNetwork.isMasterClient)
                                    {
                                        turnManager.ChangeTurn();
                                    }
                                }
                                break;
                        }
                    }
                    break;



            }
        }
        public IEnumerator Hp()
        {
            while (true)
            {
                yield return null;
                if (PhotonNetwork.inRoom)
                {
                    uiManager.SetItemImage(UI.ItemID.HpItem);
                    break;
                }

            }
        }
        public IEnumerator Ap()
        {
            while (true)
            {
                yield return null;
                if (PhotonNetwork.inRoom)
                {
                    uiManager.SetItemImage(UI.ItemID.ApItem);
                    break;
                }

            }
        }
        public IEnumerator Shield()
        {
            while (true)
            {
                yield return null;
                if (PhotonNetwork.inRoom)
                {
                    uiManager.SetItemImage(UI.ItemID.ShieldItem);
                    break;
                }

            }
        }

        [PunRPC]
        public void RPCHPItem()
        {
            CharacterController ctn = Character.Manager.Single.GetOther();

            ctn.FloatingTextOnEnter3(1200);
            ctn.GiveHeal(1200);
            uiManager.UpdateOtherSlider(ctn);

        }

        [PunRPC]
        public void RPCAPItem()
        {
            CharacterController ctn = Character.Manager.Single.GetOther();

            ctn.FloatingTextOnEnter2(30);
            ctn.GiveAP(30);
            uiManager.UpdateOtherSlider(ctn);

        }

        [PunRPC]
        public void RPCShieldItem()
        {
            CharacterController ctn = Character.Manager.Single.GetOther();
            ctn.usedShield = true;
        }

        protected IEnumerator EnterInGamePage()
        {
            boardManager.Prepare();

            int playerID = PhotonNetwork.player.ID;
            Types.Character type = (Types.Character)PhotonNetwork.player.CustomProperties["character_id"];

            Vector3 pos = Vector3.zero;

            if (PhotonNetwork.isMasterClient)
            {
                pos = new Vector3(2, 0, 1);
                uiManager.SetFirstCharacter(type, true);
            }
            else
            {
                pos = new Vector3(2, 0, 6);
                uiManager.SetSecondCharacter(type, true);
            }
            GameObject obj = PhotonNetwork.Instantiate(characters[(int)type], pos, Quaternion.identity, 0);
            ctn = obj.GetComponent<CharacterController>();
            CharacterController ctn2 = characterManager.GetOther();

            while (true)
            {
                yield return null;

                if (PhotonNetwork.room.PlayerCount == 2)
                {
                    Debug.Log("log ! : PhotonNetwork.room.PlayerCount == 2");
                    break;
                }

            }

            //ctn.OwnerID
            photonView.RPC("RpcCreateCharacter", PhotonTargets.Others, playerID, type);

            turnManager.AddPlayerID(playerID);
            isStartGame = true;

            while (true)
            {
                yield return null;

                if (IsCreateCharacter)
                {
                    isSetStats = true;
                    break;
                }
            }
        }
        public void EscapeYes()
        {

            isStartGame = false;
            PhotonNetwork.Disconnect();
            uiManager.Show((int)UI.ID.MainMenuUIController);

        }
        [PunRPC]
        protected void RpcStartGame()
        {
            if (PhotonNetwork.isMasterClient)
            {
                turnManager.StartGame();
            }
            uiManager.Show((int)UI.ID.IngameUIController);
            uiManager.Hide((int)UI.ID.RoomListController);
            uiManager.Hide((int)UI.ID.LodingUIController);
        }

        [PunRPC]
        protected void RpcCreateTargetCharacter()
        {
            IsCreateCharacter = true;
        }

        [PunRPC]
        protected void RpcCreateCharacter(int playerID, Types.Character type)
        {
            Debug.LogFormat("전달 받았음 {0}", type);
            turnManager.AddPlayerID(playerID);
            CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
            CharacterController targetCharacter = characterManager.GetOther();

            if (PhotonNetwork.isMasterClient)
            {
                uiManager.SetSecondCharacter(type, false);
            }
            else
            {
                uiManager.SetFirstCharacter(type, false);
                photonView.RPC("RpcCreateTargetCharacter", PhotonTargets.All);
            }
        }

        [PunRPC]
        public void RpcResultWindowSend()
        {
            uiManager.WinSend();
        }

        [PunRPC]
        public void RpcWinSend()
        {
            uiManager.WinSend();
        }

        [PunRPC]
        protected void RpcChangeButton()
        {
            turnManager.ChangeTurn();
        }
        #region 스킬관련RPC 시작
        [PunRPC]
        public void RpcAction(int playerID, int targetID, int x, int z)
        {
            CharacterController myCharacter = characterManager.Get(playerID);
            CharacterController targetCharacter = characterManager.Get(targetID);
            if (PhotonNetwork.isMasterClient)
            {
                myCharacter.SetPosition(x, z);
            }
            else
            {
                targetCharacter.SetPosition(x, z);
            }
        }
        [PunRPC]
        public void RpcTakenDamage(int playerID, int targetID, int damage)
        {
            CharacterController myCharacter = characterManager.Get(playerID);
            CharacterController targetCharacter = characterManager.Get(targetID);
            if (PhotonNetwork.isMasterClient && PhotonNetwork.player.IsLocal)
            {
                targetCharacter.GiveDamage(damage);
            }
            else
            {
                myCharacter.GiveDamage(damage);
            }

        }

        public bool CanUsable(int PlayerID, int consumption)
        {
            CharacterController myCharacter = characterManager.Get(PlayerID);
            if (myCharacter.RemainAP() >= consumption)
            {
                return true;
            }
            return false;
        }

        [PunRPC]
        protected void RpcSetSkill(int playerID, int Consumption)
        {
            CharacterController myCharacter = characterManager.Get(playerID); // 내턴엔 나 // 상대턴엔 상대 // 
            myCharacter.ConsumeAP(Consumption);
        }
        #endregion 스킬관련 RPC 끝

        public bool IsEnemyPos(Vector3 charpos, int x, int z)
        {
            Vector3 tile = new Vector3(x, 0, z);
            if (charpos != tile)
            {
                return true;
            }
            return false;
        }
        void MyTurn() // 내컴에선 마이턴은 내턴  상대꺼에서 마이턴은 자기턴
        {
            turnCount += 1;
            Debug.Log(turnCount);
            debugText.text = "My";
            CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID); // 호스트 01
            CharacterController targetCharacter = characterManager.GetOther(); // 클라이언트 02
            myCharacter.OnMyturn();
            targetCharacter.OfftargetTurn();
            uiManager.TurnEnableTrue();
            if (PhotonNetwork.isMasterClient)
            {
                uiManager.FirstPlayerTurn();
            }
            else
            {
                uiManager.SecondPlayerTurn();
            }
               // photonView.RPC("RpcNextTurnAP", PhotonTargets.AllBuffered, myCharacter.PlayerID);
            myCharacter.OnMovableTrigger();
            myCharacter.OnCollider();
            targetCharacter.OffCollider();
            isMyTurn = true;
            isChangeTurn = false;
            DebugLog += string.Format("\n log ! : My 턴 {0}", myCharacter.PlayerID);
            Debug.Log(DebugLog);
        }

        void TargetTurn()
        {
            debugText.text = "Target";
            CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
            CharacterController targetCharacter = characterManager.GetOther(); // 클라이언트 02
            myCharacter.OffMyturn();
            myCharacter.OffCollider();
            targetCharacter.OffCollider();
            targetCharacter.OntargetTurn();

            //myCharacter.NextTurnAP();
            //photonView.RPC("RpcNextTurnAP", PhotonTargets.Others, myCharacter.PlayerID);

            uiManager.TurnEnableFalse();
            if (PhotonNetwork.isMasterClient)
            {
                uiManager.SecondPlayerTurn();
            }
            else
            {
                uiManager.FirstPlayerTurn();
            }
            myCharacter.isSelected = false;
            isMyTurn = false;
            boardManager.CanMoveDisplay(boardManager.GetAllTiles(), "TileNomalTexture");
            DebugLog += string.Format("\n log ! : Target 턴 {0}", myCharacter.PlayerID);
            Debug.Log(DebugLog);
            isChangeTurn = true;
        }

        protected void ResultWindowSend(CharacterController myCharacter, CharacterController targetCharacter)        //업데이트 되면서 해준다.
        {
            uiManager.SetResultPlayerCheck(myCharacter, targetCharacter);
        }

        [PunRPC]
        void RPCFloatingTextEntry(int damage, int PlayerID)
        {
            CharacterController myCharacter = characterManager.Get(PlayerID);

            myCharacter.FloatingTextOnEnter(damage);
        }

    }
}