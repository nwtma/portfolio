using UnityEngine;
using UnityEditor;

namespace BattleInArea.Game
{
    public class MovableTrigger : MonoBehaviour
    {
        CharacterController character;
        private void Awake()
        {
            character = gameObject.GetComponentInParent<CharacterController>();
            gameObject.GetComponent<BoxCollider>().enabled = false;

        }

        public void On()
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        public void Off()
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Tile tile = other.GetComponent<Tile>();
            if (tile == null) return;

            character.Add(tile);

            //Debug.LogFormat("add {0}{1}", tile.x, tile.z);
        }

        private void OnTriggerExit(Collider other)
        {
            Tile tile = other.GetComponent<Tile>();
            if (tile == null) return;
            character.Remove(tile);

            //Debug.LogFormat("remove {0}{1}", tile.x, tile.z);
        }
    }
}