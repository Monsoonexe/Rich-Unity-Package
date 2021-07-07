using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

//clarifications
using Debug = UnityEngine.Debug;

/*  developer notes:
 *  consider Dictionary if lots of remove/add (slow iteration!).
 *  consider LinkedList if size is a problem.
 *  
 *  Specific calls are faster. Prefer: 
 *  ManagedBehaviourEngine.AddManagedListener((IManagedUpdate)this);
 *  
 *  You would do well to place this in the UnityExecutionOrder before 'defaultTime'.
 *  
 *  TODO - can an IManagedBehaviour be cast to multiple?????
 */

namespace RichPackage.Managed
{
    /// <summary>
    /// Managed callbacks are faster than UnityCallbacks (by about 3x!).
    /// Subscribe to these events instead to speed things up.
    /// </summary>
    public class ManagedBehaviourEngine : RichMonoBehaviour
    {
        private static ManagedBehaviourEngine instance;
        private const int STARTING_SIZE = 10;
        private static bool isQuitting = false;

        #region Listener List Fields

        [SerializeField]
        [Tooltip("These can be set at Edit time and will be properly configured.")]
        private List<AManagedBehaviour> staticBehaviours
            = new List<AManagedBehaviour>();

        private List<IManagedPreAwake> preAwakeListeners
            = new List<IManagedPreAwake>();

        private List<IManagedAwake> awakeListeners
            = new List<IManagedAwake>();

        private List<IManagedStart> startListeners
            = new List<IManagedStart>();

        private static readonly List<IManagedEarlyUpdate> earlyUpdateListeners
            = new List<IManagedEarlyUpdate>(STARTING_SIZE);

        private static readonly List<IManagedUpdate> updateListeners
            = new List<IManagedUpdate>(STARTING_SIZE);

        private static readonly List<IManagedFixedUpdate> fixedUpdateListeners
            = new List<IManagedFixedUpdate>(STARTING_SIZE);

        private static readonly List<IManagedLateUpdate> lateUpdateListeners
            = new List<IManagedLateUpdate>(STARTING_SIZE);

        private static readonly List<IManagedOnApplicationPause> pauseListeners
            = new List<IManagedOnApplicationPause>(STARTING_SIZE);

        private static readonly List<IManagedOnApplicationQuit> quitListeners
            = new List<IManagedOnApplicationQuit>(STARTING_SIZE);

        #endregion

        #region Time Fields

        /// <summary>
        /// Same as Time.deltaTime except cached, so no marshalling.
        /// </summary>
        public static float DeltaTime;

        /// <summary>
        /// Same as Time.fixedDeltaTime except cached, so no marshalling.
        /// </summary>
        public static float FixedDeltaTime;

        #endregion  

        #region Unity Callbacks

        protected override void Awake()
        {
            //do my inits
            base.Awake();

            if (!InitSingleton(this, ref instance, false))
            {
                Debug.Log("[ManagedBehaviourEngine] Singleton violation. " +
                    "Disabling this duplicate.", this);

                this.enabled = false;
                return;
            }

            var i = 0;//cache

            //init static listeners
            var count = staticBehaviours.Count;
            for (i = 0; i < count; ++i)
                AddManagedListener(staticBehaviours[i]);

            //PreAwake()
            count = preAwakeListeners.Count;
            for (i = 0; i < count; ++i)
                preAwakeListeners[i].ManagedPreAwake();

            //Awake()
            count = awakeListeners.Count;
            for (i = 0; i < count; ++i)
                awakeListeners[i].ManagedAwake();
        }

        private void Start()
        {
            //Start()
            var count = startListeners.Count;
            for (var i = 0; i < count; ++i)
                startListeners[i].ManagedStart();

            //clear initializer lists because they'll never be fired again
            preAwakeListeners.Clear();
            preAwakeListeners = null;
            awakeListeners.Clear();
            awakeListeners = null;
            startListeners.Clear();
            startListeners = null;
        }

        private void OnDestroy()
        {
            RemoveAllManagedListeners();
        }

        private void Update()
        {
            DeltaTime = Time.deltaTime; //update time step

            //EarlyUpdate()
            var count = earlyUpdateListeners.Count;
            for (var i = 0; i < count; ++i)
                earlyUpdateListeners[i].ManagedEarlyUpdate();

            //Update()
            count = updateListeners.Count;
            for (var i = 0; i < count; ++i)
                updateListeners[i].ManagedUpdate();
            
            //late update here?
        }

        private void FixedUpdate()
        {
            FixedDeltaTime = Time.fixedDeltaTime; //update time step

            //FixedUpdate()
            var count = fixedUpdateListeners.Count;
            for (var i = 0; i < count; ++i)
                fixedUpdateListeners[i].ManagedFixedUpdate();
        }

        private void LateUpdate()
        {
            //LateUpdate()
            var count = lateUpdateListeners.Count;
            for (var i = 0; i < count; ++i)
                lateUpdateListeners[i].ManagedLateUpdate();
        }

        private void OnApplicationPause(bool pause)
        {
            //OnApplicationPause()
            var count = pauseListeners.Count;
            for (var i = 0; i < count; ++i)
                pauseListeners[i].ManagedOnApplicationPause(pause);
        }

        private void OnApplicationQuit()
        {
            isQuitting = true;
            //OnApplicationQuit()
            var count = quitListeners.Count;
            for (var i = 0; i < count; ++i)
                quitListeners[i].ManagedOnApplicationQuit();
        }

        #endregion

        #region Add Listeners

        //These should not be called at runtime! only work on static behaviours.
        //public static void AddManagedListener(IManagedPreAwake behaviour)
        //{
        //    AssertSingletonExists();
        //    Debug.Assert(!preAwakeListeners.Contains(behaviour),
        //        "[ManagedBehaviourEngine] Duplicate behaviour being subscribed!" +
        //        " Check your subscription logic, fool!");

        //    preAwakeListeners.Add(behaviour);
        //}
        //public static void AddManagedListener(IManagedAwake behaviour)
        //{
        //    AssertSingletonExists();
        //    Debug.Assert(!awakeListeners.Contains(behaviour),
        //        "[ManagedBehaviourEngine] Duplicate behaviour being subscribed!" +
        //        " Check your subscription logic, fool!");

        //    awakeListeners.Add(behaviour);
        //}
        //public static void AddManagedListener(IManagedStart behaviour)
        //{
        //    AssertSingletonExists();
        //    Debug.Assert(!startListeners.Contains(behaviour),
        //        "[ManagedBehaviourEngine] Duplicate behaviour being subscribed!" +
        //        " Check your subscription logic, fool!");

        //    startListeners.Add(behaviour);
        //}

        public static void AddManagedListener(IManagedEarlyUpdate behaviour)
        {
            AssertSingletonExists();
            Debug.Assert(!earlyUpdateListeners.Contains(behaviour),
                "[ManagedBehaviourEngine] Duplicate behaviour being subscribed!" +
                " Check your subscription logic, fool!");
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");

            earlyUpdateListeners.Add(behaviour);
        }
        public static void AddManagedListener(IManagedUpdate behaviour)
        {
            AssertSingletonExists();
            Debug.Assert(!updateListeners.Contains(behaviour),
                "[ManagedBehaviourEngine] Duplicate behaviour being subscribed!" +
                " Check your subscription logic, fool!");
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");

            updateListeners.Add(behaviour);
        }
        public static void AddManagedListener(IManagedFixedUpdate behaviour)
        {
            AssertSingletonExists();
            Debug.Assert(!fixedUpdateListeners.Contains(behaviour),
                "[ManagedBehaviourEngine] Duplicate behaviour being subscribed!" +
                " Check your subscription logic, fool!");
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");

            fixedUpdateListeners.Add(behaviour);
        }
        public static void AddManagedListener(IManagedLateUpdate behaviour)
        {
            AssertSingletonExists();
            Debug.Assert(!lateUpdateListeners.Contains(behaviour),
                "[ManagedBehaviourEngine] Duplicate behaviour being subscribed!" +
                " Check your subscription logic, fool!");
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");

            lateUpdateListeners.Add(behaviour);
        }
        public static void AddManagedListener(IManagedOnApplicationPause behaviour)
        {
            AssertSingletonExists();
            Debug.Assert(!pauseListeners.Contains(behaviour),
                "[ManagedBehaviourEngine] Duplicate behaviour being subscribed!" +
                " Check your subscription logic, fool!");
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");

            pauseListeners.Add(behaviour);
        }
        public static void AddManagedListener(IManagedOnApplicationQuit behaviour)
        {
            AssertSingletonExists();
            Debug.Assert(!quitListeners.Contains(behaviour),
                "[ManagedBehaviourEngine] Duplicate behaviour being subscribed!" +
                " Check your subscription logic, fool!");

            quitListeners.Add(behaviour);
        }
        /// <summary>
        /// This is a bit slower as it has to test against every possible IManagedBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        public static void AddManagedListener(IManagedBehaviour behaviour)
        {
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");
            //add one or multiple behaviours
            //TODO - exception on subscribe Initializer
            //if (behaviour is IManagedPreAwake a)
            //    AddManagedListener(a);
            //else if (behaviour is IManagedAwake b)
            //    AddManagedListener(b);
            //else if (behaviour is IManagedStart c)
            //    AddManagedListener(c);
            if (behaviour is IManagedEarlyUpdate d)
                AddManagedListener(d);
            else if (behaviour is IManagedUpdate e)
                AddManagedListener(e);
            else if (behaviour is IManagedFixedUpdate f)
                AddManagedListener(f);
            else if (behaviour is IManagedLateUpdate g)
                AddManagedListener(g);
            else if (behaviour is IManagedOnApplicationPause h)
                AddManagedListener(h);
            else if (behaviour is IManagedOnApplicationQuit i)
                AddManagedListener(i);
        }
        /// <summary>
        /// This is the slowest as it has to test against every possible IManagedBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        public static void AddManagedListener(AManagedBehaviour behaviour)
        {
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");
            //add one or multiple behaviours
            //TODO - exception on subscribe Initializer
            //if (behaviour is IManagedPreAwake a)
            //    AddManagedListener(a);
            //if (behaviour is IManagedAwake b)
            //    AddManagedListener(b);
            //if (behaviour is IManagedStart c)
            //    AddManagedListener(c);
            if (behaviour is IManagedEarlyUpdate d)
                AddManagedListener(d);
            if (behaviour is IManagedUpdate e)
                AddManagedListener(e);
            if (behaviour is IManagedFixedUpdate f)
                AddManagedListener(f);
            if (behaviour is IManagedLateUpdate g)
                AddManagedListener(g);
            if (behaviour is IManagedOnApplicationPause h)
                AddManagedListener(h);
            if (behaviour is IManagedOnApplicationQuit i)
                AddManagedListener(i);
        }
        /// <summary>
        /// This is the slowest as it has to test against every possible IManagedBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        public static void AddManagedListener(MonoBehaviour behaviour)
        {
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");
            //add one or multiple behaviours
            //TODO - exception on subscribe Initializer
            //if (behaviour is IManagedPreAwake a)
            //    AddManagedListener(a);
            //if (behaviour is IManagedAwake b)
            //    AddManagedListener(b);
            //if (behaviour is IManagedStart c)
            //    AddManagedListener(c);
            if (behaviour is IManagedEarlyUpdate d)
                AddManagedListener(d);
            if (behaviour is IManagedUpdate e)
                AddManagedListener(e);
            if (behaviour is IManagedFixedUpdate f)
                AddManagedListener(f);
            if (behaviour is IManagedLateUpdate g)
                AddManagedListener(g);
            if (behaviour is IManagedOnApplicationPause h)
                AddManagedListener(h);
            if (behaviour is IManagedOnApplicationQuit i)
                AddManagedListener(i);
        }

        #endregion

        #region Remove Listeners

        //These should not be called at runtime! only work on static behaviours.
        //public static void RemoveManagedListener(IManagedPreAwake behaviour)
        //{
        //    AssertSingletonExists();
        //    preAwakeListeners.Remove(behaviour);
        //}
        //public static void RemoveManagedListener(IManagedAwake behaviour)
        //{
        //    AssertSingletonExists();
        //    awakeListeners.Remove(behaviour);
        //}
        //public static void RemoveManagedListener(IManagedStart behaviour)
        //{
        //    AssertSingletonExists();
        //    startListeners.Remove(behaviour);
        //}

        public static void RemoveAllManagedListeners()
        {
            instance?.staticBehaviours.Clear();
            instance?.preAwakeListeners?.Clear();
            instance?.awakeListeners?.Clear();
            instance?.startListeners?.Clear();
            earlyUpdateListeners.Clear();
            updateListeners.Clear();
            fixedUpdateListeners.Clear();
            lateUpdateListeners.Clear();
            pauseListeners.Clear();
            quitListeners.Clear();
        }
        public static void RemoveManagedListener(IManagedEarlyUpdate behaviour)
        {
            AssertSingletonExists();
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");
            earlyUpdateListeners.Remove(behaviour);
        }
        public static void RemoveManagedListener(IManagedUpdate behaviour)
        {
            AssertSingletonExists();
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");
            updateListeners.Remove(behaviour);
        }
        public static void RemoveManagedListener(IManagedFixedUpdate behaviour)
        {
            AssertSingletonExists();
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");
            fixedUpdateListeners.Remove(behaviour);
        }
        public static void RemoveManagedListener(IManagedLateUpdate behaviour)
        {
            AssertSingletonExists();
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");
            lateUpdateListeners.Remove(behaviour);
        }
        public static void RemoveManagedListener(IManagedOnApplicationPause behaviour)
        {
            AssertSingletonExists();
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");
            pauseListeners.Remove(behaviour);
        }
        public static void RemoveManagedListener(IManagedOnApplicationQuit behaviour)
        {
            AssertSingletonExists();
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");
            quitListeners.Remove(behaviour);
        }
        /// <summary>
        /// This is a bit slower as it has to test against every possible IManagedBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        public static void RemoveManagedListener(IManagedBehaviour behaviour)
        {
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");
            //add one or multiple behaviours
            //if (behaviour is IManagedPreAwake a)
            //    RemoveManagedListener(a);
            //else if (behaviour is IManagedAwake b)
            //    RemoveManagedListener(b);
            //else if (behaviour is IManagedStart c)
            //    RemoveManagedListener(c);

            if (behaviour is IManagedEarlyUpdate d)
                RemoveManagedListener(d);
            else if (behaviour is IManagedUpdate e)
                RemoveManagedListener(e);
            else if (behaviour is IManagedFixedUpdate f)
                RemoveManagedListener(f);
            else if (behaviour is IManagedLateUpdate g)
                RemoveManagedListener(g);
            else if (behaviour is IManagedOnApplicationPause h)
                RemoveManagedListener(h);
            else if (behaviour is IManagedOnApplicationQuit i)
                RemoveManagedListener(i);
        }
        /// <summary>
        /// This is the slowest as it has to test against every possible IManagedBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        public static void RemoveManagedListener(AManagedBehaviour behaviour)
        {
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");
            //add one or multiple behaviours
            //if (behaviour is IManagedPreAwake a)
            //    RemoveManagedListener(a);
            //if (behaviour is IManagedAwake b)
            //    RemoveManagedListener(b);
            //if (behaviour is IManagedStart c)
            //    RemoveManagedListener(c);
            if (behaviour is IManagedEarlyUpdate d)
                RemoveManagedListener(d);
            if (behaviour is IManagedUpdate e)
                RemoveManagedListener(e);
            if (behaviour is IManagedFixedUpdate f)
                RemoveManagedListener(f);
            if (behaviour is IManagedLateUpdate g)
                RemoveManagedListener(g);
            if (behaviour is IManagedOnApplicationPause h)
                RemoveManagedListener(h);
            if (behaviour is IManagedOnApplicationQuit i)
                RemoveManagedListener(i);
        }
        /// <summary>
        /// This is the slowest as it has to test against every possible IManagedBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        public static void RemoveManagedListener(MonoBehaviour behaviour)
        {
            Debug.Assert(behaviour != null,
                "[ManagedBehaviourEngine] behaviour is null");
            //add one or multiple behaviours
            //if (behaviour is IManagedPreAwake a)
            //    RemoveManagedListener(a);
            //if (behaviour is IManagedAwake b)
            //    RemoveManagedListener(b);
            //if (behaviour is IManagedStart c)
            //    RemoveManagedListener(c);
            if (behaviour is IManagedEarlyUpdate d)
                RemoveManagedListener(d);
            if (behaviour is IManagedUpdate e)
                RemoveManagedListener(e);
            if (behaviour is IManagedFixedUpdate f)
                RemoveManagedListener(f);
            if (behaviour is IManagedLateUpdate g)
                RemoveManagedListener(g);
            if (behaviour is IManagedOnApplicationPause h)
                RemoveManagedListener(h);
            if (behaviour is IManagedOnApplicationQuit i)
                RemoveManagedListener(i);
        }
        #endregion

        [Conditional("UNITY_EDITOR")]
        private static void AssertSingletonExists()
        {
            if (isQuitting) return;
            Debug.Assert(instance,
                "[ManagedBehaviourEngine] No instance in scene! " +
                "One is being created for you now, but it won't be done during build.");

            Construct();
        }

        /// <summary>
        /// Creates a new instance in the scene if one doesn't already exist.
        /// </summary>
        /// <returns></returns>
        [ContextMenu("Tools/Create ManagedBehaviourEngine Instance")]
        public static void Construct()
        {
            if (isQuitting) return;
            if (!instance)
                instance = new GameObject("ManagedBehaviourEngine")
                    .AddComponent<ManagedBehaviourEngine>();
            
        }
    }
}
