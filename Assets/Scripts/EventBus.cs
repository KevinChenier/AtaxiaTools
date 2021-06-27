using Assets.Scripts.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class EventBus : MonoBehaviour
    {
        private static EventBus _instance;
        private Queue<(Model.EventType, dynamic)> queue;
        public Dictionary<Model.EventType, EventHandler<dynamic>> handlers;

        public static EventBus Instance
        {
            get { return _instance; }
        }


        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void On(Model.EventType type, EventHandler<dynamic> func)
        {
            handlers[type] += func;
        }

        public void Push(Model.EventType type, dynamic value)
        {
            queue.Enqueue((type, value));
        }
    }
}
