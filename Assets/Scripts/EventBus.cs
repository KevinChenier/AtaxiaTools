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
        private ConcurrentQueue<(Model.Types.EventType type, dynamic value)> queue;
        private Dictionary<Model.Types.EventType, EventHandler<dynamic>> handlers;

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
            handlers = new Dictionary<Model.Types.EventType, EventHandler<dynamic>>();
            foreach (Model.Types.EventType t in Enum.GetValues(typeof(Model.Types.EventType)))
            {
                handlers.Add(t, Base);
            }
            queue = new ConcurrentQueue<(Model.Types.EventType type, dynamic value)>();
        }

        private void Base(object sender, dynamic e)
        {
            //Debug.Log(e.ToString());
        }

        public void On(Model.Types.EventType type, EventHandler<dynamic> func)
        {
            if (type == Model.Types.EventType.All)
            {
                foreach (Model.Types.EventType t in Enum.GetValues(typeof(Model.Types.EventType)))
                {
                    handlers[t] += func;
                }
            } else
            {
                handlers[type] += func;
            }
        }

        public void Push(Model.Types.EventType type, dynamic value)
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
