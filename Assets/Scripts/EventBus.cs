using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Scripts
{
    public class EventBus : MonoBehaviour
    {
        private static EventBus _instance;
        private ConcurrentQueue<(Model.EventType type, dynamic value)> queue;
        private Dictionary<Model.EventType, EventHandler<dynamic>> handlers;

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
            Init();
            var bgw = new BackgroundWorker();
            bgw.DoWork += Work;
            bgw.RunWorkerAsync();
            DontDestroyOnLoad(gameObject);
        }

        private void Init()
        {
            handlers = new Dictionary<Model.EventType, EventHandler<dynamic>>();
            foreach (Model.EventType t in Enum.GetValues(typeof(Model.EventType)))
            {
                handlers.Add(t, Base);
            }
            queue = new ConcurrentQueue<(Model.EventType type, dynamic value)>();
        }

        private void Base(object sender, dynamic e)
        {
            //Debug.Log(e.ToString());
        }

        public void On(Model.EventType type, EventHandler<dynamic> func)
        {
            if (type == Model.EventType.All)
            {
                foreach (Model.EventType t in Enum.GetValues(typeof(Model.EventType)))
                {
                    handlers[t] += func;
                }
            } else
            {
                handlers[type] += func;
            }
        }

        public void Push(Model.EventType type, dynamic value)
        {
            queue.Enqueue((type, value));
        }

        private void Work(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                if (queue.TryDequeue(out var value))
                {
                    handlers[value.type](sender, value.value);
                }
            }
        }
    }
}
