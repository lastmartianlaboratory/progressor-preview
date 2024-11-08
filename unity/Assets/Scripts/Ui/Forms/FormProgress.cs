using System;
using UnityEngine;
using UXT;
using UXT.Attr;
using UXT.UI;
using Progressor.Ui;
using Sf = UnityEngine.SerializeField;

namespace Progressor.Ui
{
    public class FormProgress : Dialog
    {
        // ⚡ Events

        public event Action OnPauseToggle;

        // ⌘ Auto

        [RqSc] Ticker   Ticker;

        // ≡ Sf

        [ConstructHeader]
        [Sf, Construct] WidgetProgressBar _ProgressBar;

        // ↑ Dialog

        protected override void OnOpen ()
        {
            _ProgressBar.SetProgress(0);
        }

        // ↑ Widget

        protected override void ConstructWidget ()
        {
            _ProgressBar.ClickEvent.AddListener((_) => { OnPauseToggle?.Invoke(); });
        }

        // ↑ MonoBehaviour

        private void Update ()
        {
            _ProgressBar.SetProgress((Ticker.ActivePhase != null) ? Ticker.ActivePhase.ProgressNormalized : 0f);
        }
    }
}
