

namespace BattleInArea.Game
{
    public partial class Helper
    {
        public class RPC
        {
            public static PhotonView Single;

            public static void Send(string methodName, PhotonTargets target, params object[] parameters)
            {
                Single.RPC(methodName, target, parameters);
            }

            public static void RpcSetPosition(int myPlayerID, int targetPlayerID)
            {                
                Single.RPC("RpcSetPosition", PhotonTargets.AllBuffered, myPlayerID, targetPlayerID);
            }
        }
    }
}
