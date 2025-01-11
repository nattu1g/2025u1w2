using System.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Scripts
{
    public class CanvasOnOff : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        private CanvasGroup _canvasGroup;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // 起動時にCanvasGroupを取得しておく
            _canvasGroup = _canvas.GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = _canvas.gameObject.AddComponent<CanvasGroup>();
            }
        }

        // Update is called once per frame
        async Task UpdateAsync()
        {
            if (Input.GetMouseButtonDown(0))
            {
                await LMotion.Create(1f, 0f, 0.24f)
                    .BindToAlpha(_canvasGroup);
                _canvas.enabled = false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                _canvas.enabled = true;
                await LMotion.Create(0f, 1f, 0.24f)
                    .BindToAlpha(_canvasGroup);
            }
        }
    }
}
