

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    public partial class Character
    {
        public class Manager : Core.Interfaces.IManager
        {
            public static Manager Single;

            public Manager()
            {
                Single = this;
            }

            protected string[] characters = new string[] { "Characters/Warrior/Warrior", "Characters/Thief/Thief", "Characters/Archer/Archer", "Characters/Magicion/Magicion" };
            protected List<CharacterController> controllers = new List<CharacterController>();

            public void Prepare()
            {

            }

            public CharacterController Get(int playerID)
            {
                for (int i = 0, ii = controllers.Count; ii > i; ++i)
                {
                    if (controllers[i].PlayerID == playerID)
                    {
                        return controllers[i];
                    }

                }

                return null;
            }

            public CharacterController GetOther()
            {
                for (int i = 0, ii = controllers.Count; ii > i; ++i)
                {
                    if (controllers[i].PlayerID != PhotonNetwork.player.ID)
                    {
                        return controllers[i];
                    }

                }

                return null;
            }

            public static void Add(CharacterController ctn)
            {
                Single.controllers.Add(ctn);
            }

            public void Clear()
            {
                Single.controllers.Clear();
            }
        }
    }
}