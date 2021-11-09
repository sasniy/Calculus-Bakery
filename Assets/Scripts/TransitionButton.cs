using Model.Transporter;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    [RequireComponent(typeof(Button))]
    internal class TransitionButton : MonoBehaviour
    {
        [SerializeField] private Direction _direction;

        private Transporter _transporter;
        private Button _button;

        private void OnEnable()
        {
            if (_transporter != null)
                _button.onClick.AddListener(() => _transporter.TryMoveToward(_direction));
        }

        private void Start()
        {
            _transporter = FindObjectOfType<Transporter>();
            _button = GetComponent<Button>();

            _button.onClick.AddListener(() => _transporter.TryMoveToward(_direction));
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
