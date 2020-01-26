using UnityEngine;
using UnityEditor;

namespace BattleInArea.Game
{
    public class SkillTrigger : MonoBehaviour
    {
        CharacterController character;

        private void Awake()
        {
            character = gameObject.GetComponentInParent<CharacterController>();
            //gameObject.GetComponent<BoxCollider>().enabled = false;
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

            string name = gameObject.transform.parent.name;
            character.SkillAdd(name, tile);
        }
    }
}