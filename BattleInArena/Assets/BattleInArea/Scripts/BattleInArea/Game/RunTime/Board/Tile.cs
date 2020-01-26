using UnityEngine;
using UnityEditor;

namespace BattleInArea.Game
{
    [RequireComponent(typeof(Selectable))]
    public class Tile : MonoBehaviour
    {
        private Event.Selected evt;

        public Board.TileID tileID;

        public int X { get { return tileID.x; } }
        public int Z { get { return tileID.z; } }

        void Start()
        {
            Selectable selectable = gameObject.GetComponent<Selectable>();
            selectable.fun = OnSelected;
        }
        private void Awake()
        {
            evt = new Event.Selected();
        }

        public void OnSelected()
        {
            evt.hashTable["select_type"] = Event.SelectedType.Tile;
            evt.hashTable["select_tile_id"] = tileID;
            evt.hashTable["select_tile"] = TileType.ID.Nomal;
            Core.Event.Dispatcher.Dispatch(evt);
        }

        public void SetTileTexture(string Texture_Name)
        {
            MeshRenderer renderer = gameObject.GetComponentInChildren<MeshRenderer>();
            renderer.material.mainTexture = Resources.Load<Texture>(Texture_Name);
        }
    }
}
