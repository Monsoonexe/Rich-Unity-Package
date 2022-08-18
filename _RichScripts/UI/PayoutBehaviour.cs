using UnityEngine;
using DG.Tweening;
using TMPro;
using ScriptableObjectArchitecture;
using RichPackage.Audio;

namespace RichPackage.UI
{
    /// <summary>
    /// This thing makes the numbers go up over time.
    /// </summary>
    /// <remarks>Kind of gangster since it steps over the owner
    /// of the TMPText, but at least it only changes color.</remarks>
    [SelectionBase]
    public class PayoutBehaviour : RichMonoBehaviour
    {
        [Header("---Settings---")]
        public float payoutCreditsPerSecond = 100.0f;
        public float maxDuration = 3.0f;

        [SerializeField]
        [Tooltip("Color of text while credits are being paid out.")]
        private Color creditPayoutTMPColor = Color.yellow;

        [Header("---Scene Refs---")]
        [SerializeField]
        private TextMeshProUGUI creditsTMP;

        [Header("---Resources---")]
        [SerializeField]
        private IntVariable playerCredits;

        [SerializeField]
        private IntVariable playerWinnings;

        [Header("---Audio---")]
        [SerializeField]
        private AudioClipReference pingPingPingAudio;

        //DOTween.To() only works on float values, 
        //so these convert int properties to floats.

        private float GetWinAmount() 
            => playerWinnings.Value;

        private void SetWinAmount(float newAmount) 
            => playerWinnings.Value = (int)newAmount;

        private float GetCurrentValue() 
            => playerCredits.Value;

        private void SetCurrentValue(float newValue) 
            => playerCredits.Value = (int)newValue;

        public Tween PayPlayer()
        {   //collect data
            var currentCreditValue = playerCredits.Value; // collect values
            var winAmountValue = (float)playerWinnings.Value; //cached
            var newtotal = currentCreditValue + winAmountValue;
            var duration = CalculatePayoutDuration(
                currentCreditValue, (int)winAmountValue);
            var cachedColor = creditsTMP.color;//cache to remember

            //effects to play while money is paying out
            void DoStartEffects()
            {
                pingPingPingAudio.Value.PlayOneShot(
                    loop: true, pitchShift: false, duration: duration);

                creditsTMP.color = creditPayoutTMPColor; // make color while paying out
            }

            void DoEndEffects()
            {
                creditsTMP.color = cachedColor;//restore color
            }

            //gambler money increase
            var makeItRainTween = DOTween.To(GetCurrentValue, SetCurrentValue, newtotal,
                duration)
                .SetEase(Ease.Linear)//straight line
                .OnStart(DoStartEffects)
                .OnComplete(DoEndEffects);

            return makeItRainTween; //arbitrarily return this tween
        }

        /// <summary>
        /// As a function so it's easy to find and change when I want to make it better.
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="winAmount"></param>
        /// <returns></returns>
        private float CalculatePayoutDuration(int currentValue, int winAmount)
        {
            Debug.Assert(payoutCreditsPerSecond != 0, 
                "[PayoutBehaviour] Divide by zero!");

            var seconds = winAmount / payoutCreditsPerSecond;
            seconds = seconds >= maxDuration ? maxDuration : seconds;//ceiling
            return seconds;
        }
    }
}
