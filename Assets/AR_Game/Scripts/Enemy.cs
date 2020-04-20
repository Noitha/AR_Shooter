using UnityEngine;

namespace AR_Game.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public int score;

        public MeshRenderer meshRenderer;
        
        public delegate void EnemyKilled(Enemy enemy);

        public EnemyKilled OnEnemyKilled;
        
        private Rigidbody _rigidbody;

        public void Initialize()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        
        public void Kill()
        {
            OnEnemyKilled.Invoke(this);
        }
    }
}