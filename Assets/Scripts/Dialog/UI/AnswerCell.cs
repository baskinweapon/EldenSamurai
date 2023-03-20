using UnityEngine;

namespace Dialog.UI {
    public class AnswerCell : MonoBehaviour {
        public string textButton;

        public void PressButton() {
            ChatSystem.instance.NewPart(textButton);
        }
    }
}