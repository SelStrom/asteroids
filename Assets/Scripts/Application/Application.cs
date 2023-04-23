using System;
using SelStrom.Asteroids.Configs;
using UnityEngine;

namespace SelStrom.Asteroids
{
    public class Application
    {
        private GameObjectPool _gameObjectPool;
        private IApplicationComponent _appComponent;
        private Game _game;
        private Model _model;
        private GameData _configs;
        private Transform _gameContainer;
        private PlayerInput _playerInput;
        private EntitiesCatalog _catalog;
        private Hud _hud;

        public void Connect(IApplicationComponent appComponent, GameData configs,
            Transform poolContainer, Transform gameContainer, Hud hud)
        {
            _hud = hud;
            _configs = configs;
            _playerInput = new PlayerInput();
            _gameContainer = gameContainer;
            _appComponent = appComponent;

            _gameObjectPool = new GameObjectPool();
            _gameObjectPool.Connect(poolContainer);
            
            _catalog = new EntitiesCatalog();
        }

        public void Start()
        {
            var mainCamera = Camera.main;
            var orthographicSize = mainCamera!.orthographicSize;
            var sceneWidth = mainCamera.aspect * orthographicSize * 2;
            var sceneHeight = orthographicSize * 2;
            Debug.Log("Scene size: " + sceneWidth + " x " + sceneHeight);

            _model = new Model { GameArea = new Vector2(sceneWidth, sceneHeight) };
            
            _catalog.Connect(_configs, new ModelFactory(_model), new ViewFactory(_gameObjectPool, _gameContainer));
            
            _game = new Game(_catalog, _model, _configs, _playerInput, _hud);
            _game.Start();

            _appComponent.OnUpdate += OnUpdate;
            _appComponent.OnPause += OnPause;
            _appComponent.OnResume += OnResume;

            _playerInput.OnBackAction += OnBack;
        }

        private void OnResume()
        {
            _appComponent.OnUpdate += OnUpdate;
        }

        private void OnPause()
        {
            _appComponent.OnUpdate -= OnUpdate;
        }

        private void OnUpdate(float deltaTime)
        {
            //todo? _schedule.Update(deltaTime);
            _model.Update(deltaTime);
            //todo? _game.Update(deltaTime);
        }

        private void OnBack()
        {
            UnityEngine.Application.Quit(0);
        }

        private void CleanUp()
        {
            _catalog.CleanUp();
            _gameObjectPool.CleanUp();
        }
        
        private void Dispose()
        {
            _catalog.Dispose();
            _gameObjectPool.Dispose();
            
            _catalog = null;
            _gameObjectPool = null;
            _appComponent = null;
            _game = null;
            _model = null;
            _configs = null;
            _gameContainer = null;
            _playerInput = null;
            _hud = null;
        }

        public void Quit()
        {
            _appComponent.OnUpdate -= OnUpdate;
            _appComponent.OnPause -= OnPause;
            _appComponent.OnResume -= OnResume;
            _playerInput.OnBackAction -= OnBack;

            Dispose();
        }
    }
}