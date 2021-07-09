using Assets.Scripts.Model;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Assets.Scripts
{
    public class BusLogger : MonoBehaviour
    {
        private static BusLogger _instance;
        private IMongoCollection<EventLog> collection;

        public static BusLogger Instance
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
            DontDestroyOnLoad(gameObject);
        }

        private void Init()
        {
            var client = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            collection = client.GetDatabase("AtaxieTools").GetCollection<EventLog>("events");

            EventBus.Instance.On(Model.EventType.All, LogToDb);
        }

        private void LogToDb(object sender, dynamic e)
        {
            collection.InsertOneAsync(new EventLog { Value = e });
        }
    }
}
