
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PhotonView))]
public class PhotonTutorial : MonoBehaviour
{
    protected int selectedCharacterType;
    protected string[] characters = new string[] { "Characters/Archer/Archer", "Characters/Warrior/Warrior" };
    protected Dictionary<int, BattleInArea.Game.CharacterController> players = new Dictionary<int, BattleInArea.Game.CharacterController>();
    protected PhotonView photonView;
    protected bool selectedCharacter;
    protected bool createRoomAndJoin;

    public UnityEngine.UI.Text debugText;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();

        //  포톤 네트워크 접속
        PhotonNetwork.ConnectUsingSettings("0.1");

        //  게임 루프
        StartCoroutine(GameLoop());
    }

    private void Update()
    {

    }

   /* void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        GUILayout.Label("접속한 플레이어" + PhotonNetwork.playerList.Length.ToString());
        if (PhotonNetwork.isMasterClient) GUILayout.Label("마스트 클라이언트");


        

        if (PhotonNetwork.connectedAndReady && !createRoomAndJoin)
        {
            // 아처 선택
            if (GUI.Button(new Rect(65, Screen.height - 70, 60, 60), "아처\n선택"))
            {
                selectedCharacter = true;

                ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
                table["selected_character_type"] = 0;

                PhotonNetwork.player.SetCustomProperties(table);
            }

            //  전사 선택
            if (GUI.Button(new Rect(130, Screen.height - 70, 60, 60), "전사\n선택"))
            {
                selectedCharacter = true;

                ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
                table["selected_character_type"] = 1;

                PhotonNetwork.player.SetCustomProperties(table);
            }

            if (selectedCharacter)
            {
                //  게임 시작
                if (GUI.Button(new Rect(0, Screen.height - 70, 60, 60), "Start\nGame"))
                {
                    if (PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default))
                    {
                        createRoomAndJoin = true;
                    }
                }

            }

            if (GUI.Button(new Rect(195, Screen.height - 70, 60, 60), "연결해제"))
            {
                PhotonNetwork.Disconnect();
            }
        }

        //if (GUI.Button(new Rect(260, Screen.height - 70, 60, 60), "연결"))
        //{
        //    PhotonNetwork.ConnectUsingSettings("0.1");
        //}


        // 
    }*/

    IEnumerator GameLoop()
    {
        //  마스터 클라이언트만 게임 루프
        while (true)
        {
            yield return null;

            if (PhotonNetwork.isMasterClient)
                break;
        }

        //  플레이어 대기
        while (true)
        {
            yield return null;

            Debug.Log("플레이어 대기");

            if (PhotonNetwork.room.PlayerCount == 2)
                break;
        }

        /*
        //  플레이어 생성
        for (int i = 0, ii = PhotonNetwork.playerList.Length; ii > i; ++i)
        {

            Debug.Log("플레이어 생성");
            int type = (int)PhotonNetwork.playerList[i].CustomProperties["selected_character_type"];

            Vector3[] pos = new Vector3[] { new Vector3(-2, 0, 0), new Vector3(2, 0, 0) };

            GameObject obj = PhotonNetwork.Instantiate(characters[type], pos[i], Quaternion.identity, 0);

            BattleInArea.Game.CharacterController ctn = obj.GetComponent<BattleInArea.Game.CharacterController>();
            ctn.SetupPlayer(PhotonNetwork.playerList[i]);

            players.Add(PhotonNetwork.playerList[i].ID, ctn);
        }
        */

        float time = 0f;
        //  게임 루프
        while (true)
        {
            yield return null;

            time += Time.deltaTime;

            int t = Mathf.FloorToInt(time);

            photonView.RPC("Test", PhotonTargets.All, t);
            //localPlayer.photonView.RPC("Test", PhotonTargets.Others, t);
            //Test(t);
        }
    }

    [PunRPC]
    void Test(int time)
    {
        debugText.text = time.ToString();
    }
}