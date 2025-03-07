using MessagePipe;
using R3;
using UnityEngine;
using VContainer;

namespace Scripts
{
    public class ObjectPublisher : MonoBehaviour
    {
        [Inject]
        private readonly IPublisher<int> _publisher;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }
        private float _timer = 0f;
        private const float INTERVAL = 5f;

        // Update is called once per frame
        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= INTERVAL)
            {
                _timer = 0f;
                _publisher.Publish(124);
            }
        }
    }
}
