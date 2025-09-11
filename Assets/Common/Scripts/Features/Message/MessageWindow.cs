using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alchemy.Inspector;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
// using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Message
{
    public class MessageWindow : MonoBehaviour
    {
        [LabelText("行間")]
        [SerializeField] private float _lineHeight = 25f;
        [LabelText("スライド時間")]
        [SerializeField] private float _slideDuration = 0.5f;
        [LabelText("メッセージ")]
        [SerializeField] private List<GameObject> _messageObject;
        [LabelText("メッセージウィンドウ")]
        [SerializeField] private GameObject _messageWindow;
        private Queue<GameObject> _messageObjectQueue = new Queue<GameObject>(); // メッセージオブジェクトキュー
        private Queue<string> _messages = new Queue<string>(); // メッセージキュー
        private float _activeTimer = 0f; // メッセージウィンドウを表示している時間のカウント
        private const int MAX_VISIBLE_MESSAGES = 4; // 表示可能な最大メッセージ数
        private const float TARGET_TIME = 5f; // メッセージウィンドウを表示する目標時間

        private List<Vector2> _initialPositions = new List<Vector2>();


        // TEST
        private int messageCount = 0; // テスト用のカウンター
        public Button showButton;
        public Button closeButton;
        void Start()
        {



        }

        // Update is called once per frame
        void Update()
        {
            _activeTimer += Time.deltaTime; // メッセージウィンドウを表示している時間をカウント
            if (_activeTimer >= TARGET_TIME) // メッセージウィンドウを表示する目標時間を超えたら
            {
                CloseMessageWindow();
            }
            // if (Input.GetMouseButtonDown(0))
            // {
            //     // Debug.Log("[Update] Before LMotion _messageWindow.activeSelf = " + _messageWindow.activeSelf);

            //     // AddMessage("MESSEAGE " + messageCount++);
            // }
            // if (Input.GetMouseButtonDown(1))
            // {
            //     var t = _test.transform.localPosition;
            //     _test.transform.localPosition = new Vector3(t.x - 20f, t.y - 10f, 0);
            // }
        }

        // void OnEnable()
        // {
        //     Debug.Log("[MessageWindow] OnEnable");

        // }

        // void OnDisable()
        // {
        //     Debug.Log("[MessageWindow] OnDisable");
        // }

        public void AddMessage(string newMessage)
        {
            // メッセージが出たらウィンドウを継続して表示
            _activeTimer = 0f;
            // テスト用：カウント
            newMessage = newMessage + " " + messageCount++;
            // ウィンドウが非表示の場合
            if (!_messageWindow.activeSelf)
            {
                MessageWindowEnable();
            }

            _messages.Enqueue(newMessage);


            // Debug.Log("AddMessage que count" + _messages.Count);
            // UpdateMessages(_messages.Count);
            ShowMessage(_messages.Count);

            if (_messages.Count > 4)
            {
                _messages.Dequeue(); // 古いメッセージを削除
            }

        }
        /// <summary>
        /// 枠外に移動した一番上のメッセージを初期化して、一番下に移動
        /// </summary>
        private void MoveMessage()
        {
            var obj = _messageObjectQueue.ElementAt(0);
            var lt = _messageObjectQueue.ElementAt(0).transform.localPosition;
            _messageObjectQueue.ElementAt(0).transform.localPosition = new Vector3(lt.x, lt.y - _lineHeight * 5, 0);
            obj.SetActive(false);
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            _messageObjectQueue.Dequeue();
            _messageObjectQueue.Enqueue(obj);
        }
        private void ShowMessage(int index)
        {
            var targetMessage = _messageObjectQueue.ElementAt(index - 1);
            targetMessage.SetActive(true);
            targetMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _messages.ElementAt(index - 1);

            if (index > MAX_VISIBLE_MESSAGES)
            {
                var totalAnimations = index;  // 新しいメッセージ(1) + 既存メッセージ(index-1)
                var completedAnimations = 0; // 完了したアニメーションの数

                void CheckAndExecuteMove() // すべてのアニメーションが完了したら実行
                {
                    completedAnimations++;
                    if (completedAnimations == totalAnimations)
                    {
                        MoveMessage();
                    }
                }

                // 新しいメッセージのアニメーション
                LMotion.Create(
                    targetMessage.transform.localPosition.y,
                    targetMessage.transform.localPosition.y + _lineHeight,
                    _slideDuration)
                    .WithOnComplete(CheckAndExecuteMove)
                    .BindToLocalPositionY(targetMessage.transform);

                // 既存メッセージの押し出しアニメーション
                for (int i = 0; i < index - 1; i++)
                {
                    var messageObj = _messageObjectQueue.ElementAt(i);
                    LMotion.Create(
                        messageObj.transform.localPosition.y,
                        messageObj.transform.localPosition.y + _lineHeight,
                        _slideDuration)
                        .WithOnComplete(CheckAndExecuteMove)
                        .BindToLocalPositionY(messageObj.transform);
                }
            }
            else
            {
                // 4行以下の場合の処理
                LMotion.Create(
                    targetMessage.transform.localPosition.y,
                    targetMessage.transform.localPosition.y + _lineHeight,
                    _slideDuration)
                    .BindToLocalPositionY(targetMessage.transform);
            }
        }

        public void CloseMessageWindow()
        {
            if (!_messageWindow.activeSelf) return;

            _messageWindow.SetActive(false);

            // メッセージウィンドウを非表示にする
            for (int i = 0; i < _messageObject.Count; i++)
            {
                _messageObject[i].transform.localPosition = _initialPositions[i]; // 初期位置に戻す
                _messageObject[i].SetActive(false);
            }

            _messageObjectQueue.Clear();
            _messages.Clear();
        }
        /// <summary>
        /// メッセージウィンドウを表示した後の処理
        /// </summary>
        private async void MessageWindowEnable()
        {
            foreach (var message in _messageObject)
            {
                _messageObjectQueue.Enqueue(message);
                _initialPositions.Add(message.transform.localPosition);
            }
            _messageWindow.SetActive(true);
            await LMotion.Create(new Vector2(1f, 1f), new Vector2(2f, 2f), 0.2f)
                .BindToLocalScaleXY(_messageWindow.transform);
        }
    }
}