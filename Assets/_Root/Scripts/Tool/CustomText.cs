using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tool
{
    public class CustomText : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private TextMeshProUGUI _textMeshProUgui;

        public string Text
        {
            get => GetText();
            set => SetText(value);
        }


        private void OnValidate() => Initialize();
        private void Start() => Initialize();


        private void Initialize()
        {
            bool hasAnyTextComponent =
                TryAttachTextComponent(ref _text) ||
                TryAttachTextComponent(ref _textMeshProUgui);

            if (!hasAnyTextComponent)
                throw new UnityException("Can't attach any text component!");
        }

        private bool TryAttachTextComponent<TComponent>(ref TComponent component) where TComponent : Component
        {
            if (component != null)
                return true;

            return TryGetComponent(out component);
        }


        public void SetText(string text)
        {
            if (_text != null)
                _text.text = text;

            else if (_textMeshProUgui != null)
                _textMeshProUgui.text = text;
        }

        public string GetText()
        {
            if (_text != null)
                return _text.text;

            if (_textMeshProUgui != null)
                return _textMeshProUgui.text;

            return string.Empty;
        }

    }
}
