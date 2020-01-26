
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    public partial class Helper
    {
        public class Sever
        {

            /// <summary>
            /// 포톤 서버에 접속이 되었습니까?
            /// </summary>
            public static bool IsConnected
            {
                get { return PhotonNetwork.connectedAndReady; }
            }

            /// <summary>
            /// 
            /// </summary>
            public static bool IsJoinedRoom
            {
                get
                {
                    if (PhotonNetwork.connectionStateDetailed == ClientState.Joined)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }
    }
}