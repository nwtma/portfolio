using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


namespace BattleInArea.Game
{
    public partial class Board : MonoBehaviour
    {
        public class TileID
        {
            public TileID(int x, int z)
            {
                this.x = x;
                this.z = z;
            }
            public int x { private set; get; }
            public int z { private set; get; }

            public bool IsSameTile(TileID other)
            {
                if (this.x == other.x && this.z == other.z)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class Manager : Core.Interfaces.IManager
        {
            public static Manager Single { private set; get; }
            public List<Tile> AllTiles = new List<Tile>();
            public GameObject root;

            public bool IsSkillRange = false;
            public bool IsMoveRange = false;
            public Manager()
            {
                Single = this;
            }

            public void Prepare()
            {
                Create(5, 8);
            }

            public List<Tile> GetAllTiles()
            {
                return AllTiles;
            }

            public Tile tilePrefab;

            public float TileSize = 1;

            public void Create(int inX, int inZ)
            {
                int sizeX, sizeZ;
                sizeX = inX;
                sizeZ = inZ;

                root = new GameObject("Board Root");

                for (int x = 0, xx = sizeX; xx > x; ++x)
                {
                    for (int z = 0, zz = sizeZ; zz > z; ++z)
                    {
                        string name = string.Format("Board{0}{1}", x, z);
                        Vector3 pos = new Vector3(x, 0f, z * TileSize);

                        Tile tile = Instantiate<Tile>(Resources.Load<Tile>("TilePrefabs"), pos, Quaternion.Euler(90,0,0));
                        tile.transform.parent = root.transform;

                        tile.name = name;
                        tile.tileID = new TileID(x, z);
                        AllTiles.Add(tile);
                    }
                }
            }

            public void CanMoveDisplay(List<Tile> movableTiles, string Texture_Name)
            {
                for (int i = 0, ii = movableTiles.Count; ii > i; ++i)
                {
                    movableTiles[i].SetTileTexture(Texture_Name);
                }
                IsMoveRange = true;
            }
            public void ShowSkillDisplay(List<Tile> skillTiles, string Texture_Name)
            {
                for (int i = 0; skillTiles.Count > i; ++i)
                {
                    skillTiles[i].SetTileTexture(Texture_Name);
                }
                IsSkillRange = true;
            }
            public void SetDisplay(List<Tile> allTiles, string Texture_Name)
            {
                for (int i = 0; allTiles.Count > i; ++i)
                {
                    allTiles[i].SetTileTexture(Texture_Name);
                }
            }
            
            public void AllTileDel()
            {
                for(int i = 0; i <AllTiles.Count; ++i)
                {
                    Destroy(AllTiles[i].gameObject);
                }
                AllTiles.Clear();
                Destroy(root);
            }

            private Vector3[] vectors; 
            public Vector3[] GetTileVector(List<Tile> tile)
            {
                vectors = new Vector3[tile.Count];
                for(int i = 0, ii = tile.Count; i < ii; ++i)
                {
                    vectors[i] = tile[i].transform.position;
                }

                return vectors;
            }

            public void ShowDisplayOther(Vector3[] vector, Types.Tile type)
            {
                List<Tile> showTile = new List<Tile>();
                for (int i = 0, ii = vector.Length; i < ii; ++i)
                {
                    for(int n = 0, nn = AllTiles.Count; n < nn; ++n)
                    {
                        if(vector[i] == AllTiles[n].transform.position)
                        {
                            showTile.Add(AllTiles[n]);
                        }
                    }

                    switch (type)
                    {
                        case Types.Tile.Nomal:
                            {
                                SetDisplay(showTile, "TileNomalTexture");
                            }
                            break;

                        case Types.Tile.Move:
                            {
                                SetDisplay(showTile, "TileMoveTexture");
                            }
                            break;

                        case Types.Tile.Skill:
                            {
                                SetDisplay(showTile, "TileSkillTexture");
                            }
                            break;
                    }

                    showTile.Clear();
                }
            }
        }
    }
}