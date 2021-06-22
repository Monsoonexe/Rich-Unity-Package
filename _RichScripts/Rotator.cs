using UnityEngine;

namespace PolygonPilgrimage.BattleRoyaleKit
{
    public class Rotator : RichMonoBehaviour
    {
        public Vector3 rotateVector;
        public Space relativeSpace = Space.World;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotateVector * Time.deltaTime,
                relativeSpace);
        }
    }

}
