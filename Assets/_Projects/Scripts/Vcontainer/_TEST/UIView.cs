using MessagePipe;
using R3;
using Scripts.Custom;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class UIView : MonoBehaviour
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private Text text;

    [Inject]
    private readonly IPublisher<Unit> _publisher;

    private void Start()
    {
        button.onClick.AddListener(() => _publisher.Publish(Unit.Default));
    }

    public void ChangeText(int num)
    {
        text.text = num.ToString();
    }
}
