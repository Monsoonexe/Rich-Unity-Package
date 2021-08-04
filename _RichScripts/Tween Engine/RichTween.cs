using System;
using System.Collections;
using UnityEngine;

namespace RichPackage.Tweening
{
    /// <summary>
    /// Nah, got wayy too complicated for such a small project.
    /// Don't re-create DOTween.
    /// </summary>
    public class RichTween
    {
        private Coroutine myRoutine;
        private Transform myTransform;
        public Action onComplete;
        public Action onStart;
    }
}
