
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    /// <summary>
    /// Server Side
    /// </summary>
    public partial class BattleInAreaClient : Core.Client
    {
        [SerializeField]
        protected UnityEngine.UI.Text debugText;
        protected string DebugLog;

        protected PhotonView photonView;
        protected TurnManager turnManager;

        protected bool IsCreateCharacter;
        protected bool isConnected;
        protected bool isStartGame;

        public void AwakeServer()
        {
            photonView = gameObject.GetComponent<PhotonView>();
            turnManager = new TurnManager();

            Helper.RPC.Single = photonView;
        }

        public void UpdateCharacterInfo(Types.Character selectedCharacter)
        {
            ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
            table["character_id"] = selectedCharacter;

            PhotonNetwork.player.SetCustomProperties(table);
        }


        public IEnumerator Process()
        {
            yield return StartCoroutine(OnConnect());
        }

        public IEnumerator OnConnect()
        {
            //  포톤 네트워크 접속
            PhotonNetwork.ConnectUsingSettings("0.1");
            //PhotonNetwork.
            //PhotonNetwork.ConnectToBestCloudServer("0.1818");
            while (true)
            {
                yield return null;

                //  플레이어 카운드 동기화 문제로 수정
                //if (PhotonNetwork.connected)
                if (PhotonNetwork.connectionStateDetailed == ClientState.JoinedLobby)
                    break;
            }

            //  햐..포톤....나중에 면접볼때 네트워크때 힘들일 머 있었냐고하면 썰풀면 될듯...
            yield return new WaitForSeconds(Random.Range(0f, 2f));

            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet["index"] = PhotonNetwork.countOfPlayers + 1;

            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            yield return null;
        }

        public IEnumerator ServerLoop()
        {
            Debug.Log("마스터클라이언트 Loop 들어옴");

            while (true)
            {
                yield return null;

                if (turnManager.PlayerCount == 2)
                    break;
            }
            //  게임 시작
            photonView.RPC("RpcStartGame", PhotonTargets.All);
            
        }

        [PunRPC]
        void RpcChangeTurn(int playerID)
        {
            DebugLog = string.Empty;

            DebugLog += string.Format("log ! : {0}턴입니다.. ", playerID);

            if (PhotonNetwork.player.ID == playerID)
            {
                MyTurn();
            }
            else
            {
                TargetTurn();
            }
        }

        [PunRPC]
        void RpcNextTurnAP(int playerID)
        {
            CharacterController player = Character.Manager.Single.Get(playerID);
            player.NextTurnAP();
        }

        [PunRPC]
        void RpcPlayAnimation(int id, string animationName, bool loop)
        {
            CharacterController ctn = Character.Manager.Single.Get(id);
            ctn.PlayAnimation(animationName, loop);

        }

        [PunRPC]
        void RpcTakedamage(int damage, int PlayerID)
        {
            CharacterController targetCharacter = characterManager.Get(PlayerID);
            targetCharacter.GiveDamage(damage);
        }
        

        [PunRPC]
        protected void RpcConsumeAP(int playerID)
        {
            CharacterController myCharacter = characterManager.Get(playerID); // 내턴엔 나 // 상대턴엔 상대 // 
            myCharacter.ConsumeAP(10);

            //if (myCharacter.AP == 0)
            //{
            //    //Debug.Log("마스터 클라이언트의 AP가 모두 떨어졌습니다." + "마스터 클라이언트는" + myCharacter);
            //    turnManager.ChangeTurn();
            //}
        }
        [PunRPC]
        public void RpcPlayOneShotSE(string Sound_Name)
        {
            SoundManager.instance.PlayOneShotSE(Sound_Name);
        }
        [PunRPC]
        public void RpcPlaySE(string Sound_Name)
        {
            SoundManager.instance.PlaySE(Sound_Name);
        }
        [PunRPC]
        public void RpcStopALLSE()
        {
            SoundManager.instance.StopALLSE();
        }

        [PunRPC]
        public void RpcStopSE(string Sound_Name)
        {
            SoundManager.instance.StopSE2(Sound_Name);
        }
        [PunRPC]
        public void RpcLookAtTarget(int playerID)
        {
            CharacterController myCharacter = characterManager.Get(playerID);
            CharacterController targetCharacter = characterManager.GetOther();

            if (targetCharacter != null)
            {
                if (myCharacter.CharPos().z <= targetCharacter.CharPos().z)
                {
                    myCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
                    targetCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);

                }
                else if (myCharacter.CharPos().z >= targetCharacter.CharPos().z)
                {
                    myCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);
                    targetCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        [PunRPC]
        public void RpcSetEffect(Types.Character c, int idx, string whenPlay, int myID, int targetID, int targetTileZ)
        {
            CharacterController myCharacter = characterManager.Get(myID);
            CharacterController targetCharacter = characterManager.Get(targetID);

            targetCharacter.SkillTileZ = targetTileZ;

            DebugLog += string.Format("\n log ! : My 턴 {0}, Target 턴 {1}", myID, targetID); //나의 ID 2, 타겟 ID 1
            Debug.Log(DebugLog);
            staticEffectManager.instance.SetEffectRPC(c, idx, whenPlay, targetCharacter, myCharacter);
        }

        public void LookAtTarget()
        {
            CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);
            CharacterController targetCharacter = characterManager.GetOther();
            if (myCharacter.skeletonAnimation.AnimationName == "idle" && targetCharacter.skeletonAnimation.AnimationName == "idle")
            {
                if (myCharacter.Position.z <= targetCharacter.Position.z)
                {
                    myCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
                    targetCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);

                }
                else if (myCharacter.Position.z >= targetCharacter.Position.z)
                {
                    myCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);
                    targetCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        [PunRPC]
        public void RpcLookAtTile(Quaternion rotation)
        {
            CharacterController targetCharacter = characterManager.GetOther();
            targetCharacter.transform.rotation = rotation;
            Debug.Log(targetCharacter);
            Debug.Log(rotation);
        }
        public void LookAtTile(int z)
        {
            CharacterController myCharacter = characterManager.Get(PhotonNetwork.player.ID);

            if (myCharacter.CharPos().z <= z)
            {
                myCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
                photonView.RPC("RpcLookAtTile", PhotonTargets.Others, myCharacter.transform.rotation);
            }
            else if (myCharacter.CharPos().z >= z)
            {
                myCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);
                photonView.RPC("RpcLookAtTile", PhotonTargets.Others, myCharacter.transform.rotation);
            }
        }
        protected void UnearnedWin()//서버에 캐릭터가 0i이면 실행 ㅣ
        {
            if(!isStartGame)
            {
                return;
            }
            if(PhotonNetwork.room.PlayerCount == 1)//
            {
                if (!uiManager.ResultUIActive()) { return; }
                uiManager.ResultInGameUIConnect();
                //pageManager.Chage((int)Page.ID.MainMenu);
                isStartGame = false;
            }

        }
        protected void APChecker()
        {
            uiManager.APChecker();
        }


        [PunRPC]
        public void RpcSetItemEffect(string itemName, int myID, int targetID)
        {
            CharacterController myCharacter = characterManager.Get(myID);
            CharacterController targetCharacter = characterManager.Get(targetID);

            staticEffectManager.instance.SetItemEffect(itemName, targetCharacter, myCharacter);
        }


        [PunRPC]
        public void RPCShowDisplay(Vector3[] tileVector, Types.Tile type)
        {
            boardManager.ShowDisplayOther(tileVector, type);
        }

        void OnGUI()
        {
            //GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

            //if (PhotonNetwork.connectedAndReady)
            //{
            //    if (PhotonNetwork.player.CustomProperties.ContainsKey("character_id"))
            //    {
            //        Types.Character character_id = (Types.Character)PhotonNetwork.player.CustomProperties["character_id"];
            //        GUILayout.Label("선택한 캐릭터 : " + character_id.ToString());
            //        GUILayout.Label("PhotonNetwork.countOfPlayers : " + PhotonNetwork.countOfPlayers.ToString());
            //        GUILayout.Label("PhotonNetwork.countOfPlayersInRooms : " + PhotonNetwork.countOfPlayersInRooms.ToString());
            //        GUILayout.Label("PhotonNetwork.countOfRooms : " + PhotonNetwork.countOfRooms.ToString());

            //        if (PhotonNetwork.player.CustomProperties.ContainsKey ("index"))
            //        {
            //            GUILayout.Label("PhotonNetwork.MyIndex : " + PhotonNetwork.player.CustomProperties["index"].ToString());
            //        }
                    
            //        if (PhotonNetwork.room != null)
            //        {
            //            GUILayout.Label("PhotonNetwork.room.IsLocalClientInside : " + PhotonNetwork.room.IsLocalClientInside);
            //            GUILayout.Label("PhotonNetwork.IsMasterCli" + PhotonNetwork.isMasterClient);
            //            GUILayout.Label("PhotonNetwork.room.IsVisible : " + PhotonNetwork.room.IsVisible);
            //            GUILayout.Label("PhotonNetwork.room.IsOpen : " + PhotonNetwork.room.IsOpen);
            //        }
            //    }
            //}
            //GUILayout.Label("접속한 플레이어" + PhotonNetwork.playerList.Length.ToString());
        }
    }
}
