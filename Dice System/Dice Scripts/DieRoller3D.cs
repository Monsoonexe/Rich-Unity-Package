using RichPackage.Pooling;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RichPackage.DiceSystem
{
    /*
    * TODO: reroll die
    * Does this even need to exist along with DieRoller3DEngine?
    * I feel like the Engine does everything this does anyways.
    */

    /// <summary>
    /// An n-sided die pool.
    /// </summary>
    /// <seealso cref="DieBehaviour"/>
    public class DieRoller3D : RichMonoBehaviour, IDieRoller
    {
        [Header("---Settings---")]
        public bool randomizeDieSpawnPoints = false;
        public bool useSpawnPointDirection = false;

        [Header("---Prefab Refs---")]
        [SerializeField, Required]
        [Tooltip("Parent object of all dice elements (except this).")]
        private GameObject objectsHandle;

        [SerializeField, Required]
        private GameObjectPool diePool;

        [Tooltip("Point where each dice will spawn when thrown.")]
        [SerializeField]
        private Transform[] spawnPoints;

        //events
        public event Action<IList<int>> OnResultReadyEvent;

        //runtime data
        /// <summary>
        /// Results are valid after OnDiceResultsReady has been called.
        /// </summary>
        public IList<int> Results { get; private set; } //working collection provided externally
        private List<DieBehaviour> workingDice;
        private int diceStillRolling;//waits until this is 0 before raising event.

        protected override void Awake()
        {
            base.Awake();
            workingDice = new List<DieBehaviour>(diePool.maxAmount);
        }

        private void Start()
        {
            //subscribe to 'read result' event
            diePool.OnDepoolMethod = OnDepool;

            //unsubscribe from 'read result' event
            diePool.OnEnpoolMethod = OnEnpool;
        }

        #region Pooling Methods

        private void OnDepool(GameObject die)
        {
            die.GetComponent<DieBehaviour>()
                .OnResultReadyEvent += DieResultIsIn;
            die.SetActive(true);
        }

        private void OnEnpool(GameObject die)
        {
            die.GetComponent<DieBehaviour>()
                .OnResultReadyEvent -= DieResultIsIn;
            die.SetActive(false);
        }

        #endregion Pooling Methods

        [Button, DisableInEditorMode]
        public void TestDiceRoll() => RollDice(3);

        /// <summary>
        /// Physically rolls dice and returns an int[] of results.
        /// </summary>
        /// <param name="diceCount"></param>
        /// <returns>int[] of results.</returns>
        public IList<int> RollDice(int diceCount = 1)
        {
            IList<int> resultsArray = new int[diceCount];//one result per die
            RollDice(diceCount, resultsArray);
            return resultsArray;
        }

        /// <summary>
        /// Physically rolls dice and returns the results.
        /// </summary>
        /// <param name="diceCount"></param>
        /// <returns></returns>
        public void RollDice(int diceCount, IList<int> results)
        {
            Results = results;//save and return
            diceStillRolling = diceCount;//to know which dice are still rolling, not Ready.

            Debug.Assert(results != null,
                "[DieRoller3D] results IList is null! " +
                "Expected not null.",
                this);

            //validate
            Debug.AssertFormat(diceCount <= results.Count,
                "[DieRoller3D] resultsArray <{0}> is not " +
                "big enough to hold diceCount <{1}> on {2}.",
                results.Count, diceCount, this.name);

            ReclaimDice();//clear any leftover visible dice

            //add variety by changing die start points
            if (randomizeDieSpawnPoints)
                spawnPoints.Shuffle();

            // mark list as invalid until results are ready
            for (int i = 0; i < diceCount; ++i)
                results[i] = -1;//mark invalid result b.c. not ready until dice finish animating.

            // init and roll dice pool
            for (int i = 0; i < diceCount; ++i)
            {   //get di from pool
                DieBehaviour workingDie = diePool.Depool<DieBehaviour>();
                Transform spawnPoint = spawnPoints[i];

                //move dice to throw points and randomize starting rotation
                workingDie.MoveDie(spawnPoint); //move
                //workingDie.RandomizeRotation(); //seed rotation
                //already subscribed to dice' OnResultsReadyEvent.
                if (useSpawnPointDirection)
                    workingDie.RollDie(spawnPoint.forward);
                else
                    workingDie.RollDie();

                // track
                workingDice.Add(workingDie);
            }
        }

        private void DieResultIsIn(int result)
        {
            //count dice results
            --diceStillRolling;
            Results[diceStillRolling] = result; //load output
            //when all are in, report.
            if (diceStillRolling <= 0)
            {
                PrintPendingResults(); //debug.
                OnResultReadyEvent?.Invoke(Results);
            }
        }

        /// <summary>
        /// Get results without physically rolling the dice.
        /// </summary>
        /// <param name="results">list where results will be placed.</param>
        /// <returns>The same list as was given.</returns>
        public IList<int> RollDiceOdds(int diceCount)
        {
            IList<int> results = new int[diceCount];
            RollDiceOdds(diceCount, results);
            return results;
        }

        /// <summary>
        /// Get results without physically rolling the dice.
        /// </summary>
        /// <param name="results">list where results will be placed.</param>
        /// <returns>The same list as was given.</returns>
        public void RollDiceOdds(int diceCount, IList<int> results)
        {
            Results = results;//keep results to be referenced later.

            Debug.Assert(results != null,
                "[DieRoller3D] results IList is null! " +
                "Expected not null, otherwise use " +
                "`IList<int> RollDiceOdds(int)`.",
                this);

            DieBehaviour sampleDie = diePool.Depool<DieBehaviour>();
            sampleDie.gameObject.SetActive(false);//just need the data off a die

            for (int i = 0; i < diceCount; ++i)
            {
                int result = sampleDie.GetResultRandom();//random odds result
                results[i] = result;
            }
            diePool.Enpool(sampleDie);//job's done.
        }

        /// <summary>
        /// Clear dice elements.
        /// </summary>
        [Button, DisableInEditorMode]
        public void ReclaimDice()
        {
            //remove all dice and Enpool them
            workingDice.RemoveWhile(diePool.Enpool);//hooray functional programming!
        }

        private void PrintPendingResults()
        {
            int count = workingDice.Count;
            StringBuilder outLog = StringBuilderCache.Rent();

            outLog.Append("Roll results: | ");

            //copy over only numbers we need
            for (int i = 0; i < count; ++i)
                outLog.Append(Results[i].ToString() + " | ");

            Debug.Log(StringBuilderCache.ToStringAndReturn(outLog));
        }

        /// <summary>
        /// Prefer to subscribe to events.
        /// </summary>
        [Button, DisableInEditorMode]
        private void ExamineRollResults()
        {
            int dieCount = workingDice.Count;

            //possibly need to do all these raycasts in a Coroutine (one a frame)
            //get the result of each die
            for (int i = 0; i < dieCount; ++i)
            {
                DieBehaviour die = workingDice[i];
                int result = die.GetResult();
                Results[i] = result; //load output
            }
            PrintPendingResults(); //debug.
            //onDiceResultsReady.Invoke();//do this in OnComplete event to get back into Unity runtime
        }

    }
}
