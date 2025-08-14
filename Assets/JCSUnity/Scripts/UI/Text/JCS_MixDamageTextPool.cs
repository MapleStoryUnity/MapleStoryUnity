/**
 * $File: JCS_MixDamageTextPool.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Mix dmaage text pool object.
    /// </summary>
    public class JCS_MixDamageTextPool : MonoBehaviour
    {
        public delegate int[] DamageTextSpawnerFP(int minDamage, int maxDamage, Vector3 pos, int hit, int percentOfCritical);

        /* Variables */

        /// <summary>
        /// List of type of damage text.
        /// </summary>
        public enum DamageTextType
        {
            NORMAL,
            CRITICAL,
            GET_DAMAGE,
            HEAL
        };

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_MixDamageTextPool)")]

        [Tooltip("Test this component with key event.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key code to test.")]
        [SerializeField]
        private KeyCode mTestKey = KeyCode.N;
#endif

        [Separator("Initialize Variables (JCS_MixDamageTextPool)")]

        [Tooltip("Critical damage text pool.")]
        [SerializeField]
        private JCS_DamageTextPool mCritDamageTextPool = null;

        [Tooltip("Normal damage text pool.")]
        [SerializeField]
        private JCS_DamageTextPool mNormalDamageTextPool = null;

        [Tooltip("Get damage damage text pool.")]
        [SerializeField]
        private JCS_DamageTextPool mGetDamageDamageTextPool = null;

        [Tooltip("Head damage text pool.")]
        [SerializeField]
        private JCS_DamageTextPool mHealDamageTextPool = null;

        [Separator("Runtime Variables (JCS_MixDamageTextPool)")]

        [Tooltip("Spacing per damage text.")]
        [SerializeField]
        private float mSpacingPerText = 1.0f;

        [Tooltip("Time per spawns.")]
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float mTimePerSpawn = 0.1f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("- Zigge Right Left")]

        [Tooltip("Do the zigge effect?")]
        [SerializeField]
        private bool mZiggeEffect = true;

        [Tooltip("Right align value.")]
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mRightAlign = 0.0f;

        [Tooltip("Left align value.")]
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mLeftAlign = 0.0f;

        private List<int> mSequenceThread = null;
        // Data we need to let Sequence Thread process!
        private List<int[]> mSequenceDamageData = null;
        private List<Vector3[]> mSequencePosData = null;
        private List<DamageTextType[]> mSequenceTypeData = null;
        private List<AudioClip> mSequenceHitSoundData = null;

        // Utililty
        private List<float> mSequenceSpanwTimer = null;
        // IMPORTANT: index of number we want to call to spawn the damage text!
        private List<int> mSequenceSpawnCount = null;

        /* Setter & Getter */

        public JCS_TimeType timeType { get { return this.mTimeType; } set { this.mTimeType = value; } }
        public JCS_DamageTextPool criticalDamageTextPool { get { return this.mCritDamageTextPool; } }
        public JCS_DamageTextPool normralDamageTextPool { get { return this.mNormalDamageTextPool; } }
        public JCS_DamageTextPool getDamageDamageTextPool { get { return this.mGetDamageDamageTextPool; } }
        public JCS_DamageTextPool healDamageTextPoll { get { return this.mHealDamageTextPool; } }

        /* Functions */

        private void Awake()
        {
            // spawn all the sequence
            mSequenceThread = new List<int>();
            mSequenceDamageData = new List<int[]>();
            mSequencePosData = new List<Vector3[]>();
            mSequenceTypeData = new List<DamageTextType[]>();
            mSequenceSpanwTimer = new List<float>();
            mSequenceSpawnCount = new List<int>();
            mSequenceHitSoundData = new List<AudioClip>();
        }

        private void Start()
        {
            // set to global!
            JCS_UtilManager.FirstInstance().SetMixDamageTextPool(this);
        }

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif

            ProccessSequences();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mTestKey))
            {
                // Testing helper function so spawn sequence of damage
                // Showing u can get the damage from ite
                const int x_count = 10;
                const float x_distance = 3f;
                const float y_randDistance = 0.8f;

                for (int count = 0; count < x_count; ++count)
                {
                    DamageTextSpawnerSimple(
                        0,
                        9999,
                        new Vector3(
                            x_distance * count,
                            JCS_Random.Range(-y_randDistance, y_randDistance),
                            10.0f),
                        6,
                        30,
                        0);
                }
            }
        }
#endif

        /// <summary>
        /// Default Damaget Text Spawner with the defualt random algorithm!
        /// </summary>
        public int[] DamageTextSpawnerSimple(
            int minDamage,
            int maxDamage,
            Vector3 pos,
            int hit,
            int percentOfCritical,
            int defenseValue,
            bool isEnemy = false,
            AudioClip hitSound = null)
        {
            return DamageTextSpawnerSimple(
                minDamage,
                maxDamage,
                pos,
                hit,
                percentOfCritical,
                JCS_Random.Range,
                defenseValue,
                isEnemy,
                hitSound);
        }

        /// <summary>
        /// Helper function to spawn the damaget text
        /// so scripter will not have to do all the drug code over and
        /// over agian!
        /// </summary>
        /// <param name="minDamage"> minimum of damage we produce </param>
        /// <param name="maxDamage"> maximum of damage we produce </param>
        /// <param name="pos"> position of damage text we spawn </param>
        /// <param name="hit"> how many damage text we spawn </param>
        /// <param name="percentOfCritical"> how many percentage will be critical instead of normal damage text </param>
        /// <param name="isEnemy"> 
        /// true: use enemy's specific damage text set, 
        /// false: use player's specific damage text set.
        /// </param>
        /// <returns> data we produced </returns>
        public int[] DamageTextSpawnerSimple(
            int minDamage,
            int maxDamage,
            Vector3 pos,
            int hit,
            int percentOfCritical,
            JCS_Range algorithm,
            int defenseValue,
            bool isEnemy = false,
            AudioClip hitSound = null)
        {
            if (minDamage > maxDamage)
            {
                Debug.LogError("Min damage cannot be higher or equal to the max damage!");
                return null;
            }

            if (minDamage < 0 || maxDamage < 0)
            {
                Debug.LogError("Min or Max damage cannot be lower than 0!");
                return null;
            }

            if (percentOfCritical < 0 || percentOfCritical > 100)
            {
                Debug.LogError("Percent Of Critical should within range of 0 ~ 100...");
                return null;
            }

            if (hit <= 0)
            {
                Debug.LogError("Hit count should not be equal or lower than 0!");
                return null;
            }

            int[] damages = new int[hit];
            DamageTextType[] types = new DamageTextType[hit];

            // get the game setting first
            var gs = JCS_GameSettings.FirstInstance();

            for (int index = 0; index < hit; ++index)
            {
                int dm = Random.Range(minDamage, maxDamage);

                // 受到的傷害 = 傷害 - 防禦力
                damages[index] = dm - defenseValue;

                // Check min max
                {
                    // 如果小於最下限得值, 就設定為最下限的值
                    if (damages[index] < gs.MIN_DAMAGE)
                        damages[index] = gs.MIN_DAMAGE;

                    // 如果大於最上限得值, 就設定為最上限的值
                    if (damages[index] > gs.MAX_DAMAGE)
                        damages[index] = gs.MAX_DAMAGE;
                }

                // see if this damage text a critical damage text?
                bool isCritical = (algorithm(0, 100) < percentOfCritical);

                // Set the type of the damage text 
                // base on the tribe!
                if (!isEnemy)
                {
                    if (isCritical)
                        types[index] = DamageTextType.CRITICAL;
                    else
                        types[index] = DamageTextType.NORMAL;
                }
                else
                {
                    types[index] = DamageTextType.GET_DAMAGE;
                }
            }

            SpawnDamageTextsFromPoolByType(damages, pos, types, hitSound);

            // return the damages we just create!
            return damages;
        }

        /// <summary>
        /// 使用於 如果已經計算好公式了!
        /// </summary>
        /// <param name="damages"></param>
        /// <param name="pos"></param>
        /// <param name="cirticalChance"></param>
        /// <param name="damageSound"></param>
        /// <param name="isEnemy"></param>
        /// <returns> damage array </returns>
        public int[] DamageTextSpawnerSimple(
            int[] damages,
            Vector3 pos,
            int cirticalChance,
            bool isEnemy = false,
            AudioClip hitSound = null)
        {
            return DamageTextSpawnerSimple(
                damages,
                pos,
                cirticalChance,
                JCS_Random.Range,
                isEnemy,
                hitSound);
        }

        /// <summary>
        /// 使用於 如果已經計算好公式了!
        /// </summary>
        /// <param name="damages"></param>
        /// <param name="pos"></param>
        /// <param name="cirticalChance"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public int[] DamageTextSpawnerSimple(
            int[] damages,
            Vector3 pos,
            int cirticalChance,
            JCS_Range algorithm,
            bool isEnemy = false,
            AudioClip hitSound = null)
        {
            int hit = damages.Length;

            DamageTextType[] types = new DamageTextType[hit];

            for (int index = 0; index < hit; ++index)
            {
                bool isCritical = (algorithm(0, 100) < cirticalChance);

                if (!isEnemy)
                {
                    if (isCritical)
                        types[index] = DamageTextType.CRITICAL;
                    else
                        types[index] = DamageTextType.NORMAL;
                }
                else
                {
                    types[index] = DamageTextType.GET_DAMAGE;
                }
            }

            SpawnDamageTextsFromPoolByType(damages, pos, types, hitSound);

            // return original damages
            return damages;
        }

        /// <summary>
        /// Spawn a damaget text form Pool by the corresponding type.
        /// </summary>
        /// <param name="damage"> value of the damage text </param>
        /// <param name="pos"> position damage text </param>
        /// <param name="type"> type of the damage text (Default: Normal Damage Text) </param>
        public void SpawnDamageTextFromPoolByType(
            int damage,
            Vector3 pos,
            DamageTextType type = DamageTextType.NORMAL,
            AudioClip hitSound = null)
        {
            JCS_DamageTextPool dtp = GetDamageTextPoolByType(type);

            if (dtp != null)
                dtp.SpawnDamageTextFromPool(damage, pos, hitSound);
        }
        public void SpawnDamageTextsFromPoolByType(
            int[] damage,
            Vector3 pos,
            DamageTextType type,
            AudioClip hitSound = null)
        {
            DamageTextType[] types = new DamageTextType[damage.Length];

            for (int index = 0; index < types.Length; ++index)
                types[index] = type;

            SpawnDamageTextsFromPoolByType(damage, pos, types, hitSound);
        }
        public void SpawnDamageTextsFromPoolByType(
            int[] damage,
            Vector3 pos,
            DamageTextType[] types,
            AudioClip hitSound = null)
        {
            Vector3[] poses = new Vector3[damage.Length];

            for (int index = 0; index < poses.Length; ++index)
                poses[index] = pos;

            SpawnDamageTextsFromPoolByType(damage, poses, types, hitSound);
        }
        public void SpawnDamageTextsFromPoolByType(
            int[] damage,
            Vector3[] pos,
            DamageTextType[] types,
            AudioClip hitSound = null)
        {
            if (damage.Length != pos.Length || damage.Length != types.Length)
            {
                Debug.LogError("Wrong triple pair size");
                return;
            }

            if (mZiggeEffect)
            {
                for (int count = 0; count < pos.Length; ++count)
                {
                    if ((count % 2) == 0)
                        pos[count].x += mRightAlign;
                    else
                        pos[count].x -= mLeftAlign;

                    pos[count].y += mSpacingPerText * count;
                }
            }

            // add thread!
            mSequenceThread.Add(mSequenceThread.Count);

            // update data to memory
            mSequenceDamageData.Add(damage);
            mSequencePosData.Add(pos);
            mSequenceTypeData.Add(types);
            mSequenceHitSoundData.Add(hitSound);

            // simply add a timer!
            mSequenceSpanwTimer.Add(0);

            // always start with the first index
            mSequenceSpawnCount.Add(0);
        }

        /// <summary>
        /// Return the damage text pool by the enum type.
        /// </summary>
        /// <param name="type"> enum type </param>
        /// <returns> damage text pool </returns>
        private JCS_DamageTextPool GetDamageTextPoolByType(DamageTextType type)
        {
            switch (type)
            {
                case DamageTextType.NORMAL:
                    return normralDamageTextPool;
                case DamageTextType.CRITICAL:
                    return criticalDamageTextPool;
                case DamageTextType.GET_DAMAGE:
                    return getDamageDamageTextPool;
                case DamageTextType.HEAL:
                    return healDamageTextPoll;
            }
            return null;
        }

        /// <summary>
        /// The actual thread is doing here.
        /// </summary>
        /// <param name="processIndex"> the unique id of the thread </param>
        /// <param name="damage"> memory data thread needed </param>
        /// <param name="pos"> memory data thread needed </param>
        /// <param name="timer"> memory data thread needed </param>
        private void Sequence(
            int processIndex,
            int[] damage,
            Vector3[] pos,
            DamageTextType[] types,
            float timer,
            AudioClip hitSound)
        {
            float newTimer = timer;

            newTimer += JCS_Time.ItTime(mTimeType);

            if (mTimePerSpawn < newTimer)
            {
                int count = mSequenceSpawnCount[processIndex];
                if (count == damage.Length)
                {
                    // done the sequence, do delete the process(thread)
                    EndProcessSequence(processIndex);
                    return;
                }

                // spawn that specific damage text!
                SpawnDamageTextFromPoolByType(damage[count], pos[count], types[count], hitSound);

                ++count;
                // Update new count, in order to spawn next damage text.
                mSequenceSpawnCount[processIndex] = count;
                newTimer = 0.0f;
            }

            // update timer
            mSequenceSpanwTimer[processIndex] = newTimer;
        }

        private void ProccessSequences()
        {
            for (int process = 0; process < mSequenceThread.Count; ++process)
            {
                // pass in all the data wee need in order to process the data
                Sequence(process,
                    mSequenceDamageData[process],
                    mSequencePosData[process],
                    mSequenceTypeData[process],
                    mSequenceSpanwTimer[process],
                    mSequenceHitSoundData[process]);
            }
        }

        private void EndProcessSequence(int processIndex)
        {
            mSequenceThread.RemoveAt(processIndex);
            mSequenceDamageData.RemoveAt(processIndex);
            mSequencePosData.RemoveAt(processIndex);
            mSequenceTypeData.RemoveAt(processIndex);
            mSequenceSpanwTimer.RemoveAt(processIndex);
            mSequenceSpawnCount.RemoveAt(processIndex);
            mSequenceHitSoundData.RemoveAt(processIndex);
        }
    }
}
