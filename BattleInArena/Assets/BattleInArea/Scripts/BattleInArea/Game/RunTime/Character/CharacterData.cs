using UnityEngine;
using UnityEditor;

namespace BattleInArea.Game
{
    public class CharacterData : MonoBehaviour
    {
        public static CharacterData instance;
        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            
        }

        public DataBase.RowData.ChrarcterRowData GetData(string classname) //받아올 캐릭터 지정해줄 상수 만들기
        {
            Core.DataBase.ITable table = Helper.DataBase.GetTable(DataBase.CharacterTableKey);

            switch (classname)
            {
                case "Musa":
                    {
                        return table.GetRowData("10001") as DataBase.RowData.ChrarcterRowData;
                    }
                case "Thief":
                    {
                        return table.GetRowData("10002") as DataBase.RowData.ChrarcterRowData;
                    }
                case "Archer":
                    {
                        return table.GetRowData("10003") as DataBase.RowData.ChrarcterRowData;
                    }
                case "Magician":
                    {
                        return table.GetRowData("10004") as DataBase.RowData.ChrarcterRowData;
                    }
            }

            return null;
        }
        public int GetDamage(int skillNumber, string className)
        {
            Core.DataBase.ITable table = Helper.DataBase.GetTable(DataBase.CharacterTableKey);

            switch (className)
            {
                case "Musa":
                    {
                        DataBase.RowData.ChrarcterRowData warrior = table.GetRowData("10001") as DataBase.RowData.ChrarcterRowData;

                        switch(skillNumber)
                        {
                            case 1:
                                {
                                    return warrior.skill01dmg;
                                }
                            case 2:
                                {
                                    return warrior.skill02dmg;
                                }
                            case 3:
                                {
                                    return warrior.skill03dmg;
                                }
                        }
                    }
                    break;
                case "Thief":
                    {
                        DataBase.RowData.ChrarcterRowData thief = table.GetRowData("10002") as DataBase.RowData.ChrarcterRowData;

                        switch (skillNumber)
                        {
                            case 1:
                                {
                                    return thief.skill01dmg;
                                }
                            case 2:
                                {
                                    return thief.skill02dmg;
                                }
                            case 3:
                                {
                                    return thief.skill03dmg;
                                }
                        }
                    }
                    break;
                case "Archer":
                    {
                        DataBase.RowData.ChrarcterRowData archer = table.GetRowData("10003") as DataBase.RowData.ChrarcterRowData;

                        switch (skillNumber)
                        {
                            case 1:
                                {
                                    return archer.skill01dmg;
                                }
                            case 2:
                                {
                                    return archer.skill02dmg;
                                }
                            case 3:
                                {
                                    return archer.skill03dmg;
                                }
                        }
                    }
                    break;
                case "Magician":
                    {
                        DataBase.RowData.ChrarcterRowData magician = table.GetRowData("10004") as DataBase.RowData.ChrarcterRowData;

                        switch (skillNumber)
                        {
                            case 1:
                                {
                                    return magician.skill01dmg;
                                }
                            case 2:
                                {
                                    return magician.skill02dmg;
                                }
                            case 3:
                                {
                                    return magician.skill03dmg;
                                }
                        }
                    }
                    break;
            }

            return 0;
        }
        public int GetConsumption(int skillNumber, string className)
        {
            Core.DataBase.ITable table = Helper.DataBase.GetTable(DataBase.CharacterTableKey);

            switch (className)
            {
                case "Musa":
                    {
                        DataBase.RowData.ChrarcterRowData warrior = table.GetRowData("10001") as DataBase.RowData.ChrarcterRowData;

                        switch (skillNumber)
                        {
                            case 1:
                                {
                                    return warrior.skill01ap;
                                }
                            case 2:
                                {
                                    return warrior.skill02ap;
                                }
                            case 3:
                                {
                                    return warrior.skill03ap;
                                }
                        }
                    }
                    break;
                case "Thief":
                    {
                        DataBase.RowData.ChrarcterRowData thief = table.GetRowData("10002") as DataBase.RowData.ChrarcterRowData;

                        switch (skillNumber)
                        {
                            case 1:
                                {
                                    return thief.skill01ap;
                                }
                            case 2:
                                {
                                    return thief.skill02ap;
                                }
                            case 3:
                                {
                                    return thief.skill03ap;
                                }
                        }
                    }
                    break;
                case "Archer":
                    {
                        DataBase.RowData.ChrarcterRowData archer = table.GetRowData("10003") as DataBase.RowData.ChrarcterRowData;

                        switch (skillNumber)
                        {
                            case 1:
                                {
                                    return archer.skill01ap;
                                }
                            case 2:
                                {
                                    return archer.skill02ap;
                                }
                            case 3:
                                {
                                    return archer.skill03ap;
                                }
                        }
                    }
                    break;
                case "Magician":
                    {
                        DataBase.RowData.ChrarcterRowData magician = table.GetRowData("10004") as DataBase.RowData.ChrarcterRowData;

                        switch (skillNumber)
                        {
                            case 1:
                                {
                                    return magician.skill01ap;
                                }
                            case 2:
                                {
                                    return magician.skill02ap;
                                }
                            case 3:
                                {
                                    return magician.skill03ap;
                                }
                        }
                    }
                    break;
            }

            return 0;
        }
    }
}