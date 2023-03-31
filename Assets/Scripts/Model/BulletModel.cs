using Model.Components;
using UnityEngine;

namespace SelStrom.Asteroids
{
    public class BulletModel : IGameEntity
    {
        public MoveComponent Move = new();
        private bool _isDead;
        private float _lifeTime;

        public BulletModel(int lifeTimeSeconds, Vector2 position, Vector2 direction, float speed)
        {
            _lifeTime = lifeTimeSeconds;
            Move.Position.Value = position;
            Move.Direction = direction;
            Move.Speed = speed;
        }

        public bool IsDead() => _isDead;
        
        public void Connect(Model model)
        {
            Move.Connect(model);
        }

        public void Update(float deltaTime)
        {
            Move.Update(deltaTime);

            _lifeTime -= deltaTime;
            if (_lifeTime <= 0)
            {
                _isDead = true;
            }
        }
    }
}