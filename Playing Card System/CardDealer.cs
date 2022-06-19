using UnityEngine;
using DG.Tweening;
using RichPackage.Audio;
using RichPackage.Decks;
using RichPackage.Pooling;
using Sirenix.OdinInspector;
using RichPackage.Animation;

namespace RichPackage.PlayingCards
{
    /// <summary>
    /// 
    /// </summary>
    public class CardDealer : RichMonoBehaviour
    {
        [Header("---Settings---")]
        public float dealTileDuration = 2.0f;
        public float flipDuration = 1.0f;
        public Vector3 punchScale = new Vector3(0.25f, 0.25f, 0);
        public int punchVibrato = 5;

        [Header("---Easings---")]
        public Ease moveEase = Ease.OutCirc;
        public Ease flipEase = Ease.OutQuart;
        //public Ease punchEase = Ease.;

        [Header("---Prefab Refs---")]
        public Transform deckOriginPoint;

        [Header("---Resources---")]
        public ADeck cardDeck;

        [SerializeField]
        private GameObjectPool gameObjectPool;

        [Header("---Audio---")]
        [SerializeField]
        private RichAudioClipReference dealCardAudio;

        [SerializeField]
        private RichAudioClipReference flipCardAudio;

        private void Start()
        {
            //configure pool
            gameObjectPool.OnEnpoolMethod += (item) => item.GetComponent<ACardBehaviour>().Hide(); //hide show
            gameObjectPool.OnDepoolMethod += (item) => item.GetComponent<ACardBehaviour>().Show(); //hide show

            if (!gameObjectPool.initOnAwake)
                gameObjectPool.InitPool();
        }

        private CardSO DrawCardInternal()
            => cardDeck.DrawAs<CardSO>();

        public Tween Deal(Transform targetPoint, out CardSO cardData)
        {
            //draw data from deck
            cardData = DrawCardInternal();

            //draw mono from pool
            var mono = gameObjectPool.Depool<ACardBehaviour>();
            var cardXform = mono.transform;

            //merge
            mono.UpdateUI(cardData);

            //start at origin (deck) face down
            cardXform.SetPositionAndRotation(deckOriginPoint.position,
                deckOriginPoint.rotation);

            //move card from deck to point
            var moveTween = cardXform.DOMove(
                targetPoint.position, dealTileDuration)
                .OnStart(dealCardAudio.DoPlaySFX)
                .SetEase(moveEase);

            //flip to show face
            var flipTween = cardXform.DOLocalRotateBy(
                new Vector3(0, 180, 0), flipDuration)
                .OnStart(flipCardAudio.DoPlaySFX)
                .SetEase(flipEase);

            //punch for added effect
            var punchTween = cardXform.DOPunchScale(punchScale,
                flipDuration, punchVibrato);

            //Create and rig sequence
            var dealSequence = DOTween.Sequence();

            //flip and punch in parallel
            dealSequence.Append(moveTween)
                .Append(flipTween)
                .Join(punchTween);

            return dealSequence;
        }

        /// <summary>
        /// Reshuffle entire deck.
        /// </summary>
        public void ReshuffleDeck() => cardDeck.Shuffle();

        /// <summary>
        /// Discard the top card.
        /// </summary>
        public void Discard() => DrawCardInternal();

        /// <summary>
        /// Hide all the cards dealt.
        /// </summary>
        public void ResetCards()
            => gameObjectPool.ReturnAllToPool();
    }
}
