/**
 * $File: JCS_DamageTextPool.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Reusable damage text pool.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_DamageTextPool : MonoBehaviour
    {
        public delegate void SpawnSequenceAction(int[] damage, Vector3[] pos, float timePerSpawn, int totalSpawn);

        /* Variables */

        [Header("** Initialize Variables (JCS_DamageTextPool) **")]

        [Tooltip("Number to handle and spawn at the initialize time.")]
        [SerializeField]
        [Range(1, 500)]
        private int mNumberOfHandle = 10;

        // optimize
        private int mLastSpawnPos = 0;

        // type of the damage text the object going to handle!
        [Header("NOTE: Plz put the whole set of Damage Text here!")]

        [Tooltip("Damage text clone.")]
        [SerializeField]
        private JCS_DamageText mDamageText = null;

        private JCS_Vector<JCS_DamageText> mDamageTexts = null;

        [Header("** Runtime Variables (JCS_DamageTextPool) **")]

        [Tooltip("Spacing per damage text.")]
        [SerializeField]
        [Range(0.0f, 3000.0f)]
        private float mSpacingPerText = 1.0f;

        [Tooltip("Time per spawns.")]
        [SerializeField]
        [Range(0.001f, 5.0f)]
        private float mTimePerSpawn = 0.1f;

        [Tooltip("Face the camera when called damage text.")]
        [SerializeField]
        private bool mFaceCamera = true;

        [Header("- Sound")]

        [Tooltip("Sound when spawns.")]
        [SerializeField]
        private AudioClip mHitSound = null;

        private JCS_SoundPlayer mSoundPlayer = null;

        [Header("- Zigge Right Left (In Sequence)")]

        [Tooltip("Do the zigge effect?")]
        [SerializeField]
        private bool mZiggeEffect = true;

        [Tooltip("Right align value.")]
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mRightAlign = 0.2f;

        [Tooltip("Left align value.")]
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mLeftAlign = 0.2f;

        private List<int> mSequenceThread = null;
        // Data we need to let Sequence Thread process!
        private List<int[]> mSequenceDamageData = null;
        private List<Vector3[]> mSequencePosData = null;
        private List<AudioClip> mSequenceHitSoundData = null;
        private List<float> mSequenceSpanwTimer = null;
        // IMPORTANT: index of number we want to call to spawn the damage text!
        private List<int> mSequenceSpawnCount = null;

        /* Setter & Getter */

        public int GetNumberOfHandle() { return this.mNumberOfHandle; }

        public float SpacingPerText { get { return this.mSpacingPerText; } set { this.mSpacingPerText = value; } }
        public float TimePerSpawn { get { return this.mTimePerSpawn; } set { this.mTimePerSpawn = value; } }
        public bool FaceCamera { get { return this.mFaceCamera; } set { this.mFaceCamera = value; } }

        public bool ZiggeEffect { get { return this.mZiggeEffect; } set { this.mZiggeEffect = value; } }
        public float RightAlign { get { return this.mRightAlign; } set { this.mRightAlign = value; } }
        public float LeftAlign { get { return this.mLeftAlign; } set { this.mLeftAlign = value; } }

        public void SetHitSound(AudioClip hitSound) { this.mHitSound = hitSound; }

        /* Functions */

        private void Awake()
        {
            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            // spawn the pool
            InitDamageTextToArray();

            // spawn all the sequence
            mSequenceThread = new List<int>();
            mSequenceDamageData = new List<int[]>();
            mSequencePosData = new List<Vector3[]>();
            mSequenceSpanwTimer = new List<float>();
            mSequenceHitSoundData = new List<AudioClip>();
            mSequenceSpawnCount = new List<int>();
        }

        private void Update()
        {
            ProccessSequences();
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
        /// <returns> data we produced </returns>
        public int[] DamageTextSpawnerSimple(
            int minDamage,
            int maxDamage,
            Vector3 pos,
            int hit,
            AudioClip hitSound = null)
        {
            if (minDamage > maxDamage)
            {
                JCS_Debug.LogError("Min damage cannot be higher or equal to the max damage!");
                return null;
            }

            if (minDamage < 0 || maxDamage < 0)
            {
                JCS_Debug.LogError("Min or Max damage cannot be lower than 0!");
                return null;
            }

            if (hit <= 0)
            {
                JCS_Debug.LogError("Hit count should not be equal or lower than 0!");
                return null;
            }

            int[] damages = new int[hit];

            for (int index = 0; index < hit; ++index)
            {
                int dm = Random.Range(minDamage, maxDamage);
                damages[index] = dm;
            }

            SpawnDamagetTexts(damages, pos, hitSound);

            // return the damages we just create!
            return damages;
        }

        /// <summary>
        /// Spawn multiple damage texts at once.
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="pos"></param>
        /// <param name="hitSound"></param>
        public void SpawnDamagetTexts(
            int[] damage,
            Vector3 pos,
            AudioClip hitSound = null)
        {
            Vector3[] poses = new Vector3[damage.Length];

            for (int index = 0; index < poses.Length; ++index)
            {
                poses[index] = pos;
            }

            SpawnDamagetTexts(damage, poses, hitSound);
        }
        public void SpawnDamagetTexts(
            int[] damage,
            Vector3[] pos,
            AudioClip hitSound = null)
        {
            if (damage.Length != pos.Length)
            {
                JCS_Debug.LogError("Wrong pair size!");
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
            mSequenceHitSoundData.Add(hitSound);

            // simply add a timer!
            mSequenceSpanwTimer.Add(0);

            // always start with the first index
            mSequenceSpawnCount.Add(0);
        }

        /// <summary>
        /// Spawn one damage text.
        /// </summary>
        /// <param name="damage"> damage number </param>
        /// <param name="pos"> spawn position </param>
        public void SpawnDamageTextFromPool(
            int damage,
            Vector3 pos,
            AudioClip hitSound,
            bool secondSearch = false)
        {
            if (mNumberOfHandle == 0)
                return;

            JCS_DamageText dt = null;

            for (int index = mLastSpawnPos; index < mNumberOfHandle; ++index)
            {
                dt = mDamageTexts.at(index);
                // if not active, meaning we can spawn the text
                if (!dt.isActive())
                {
                    dt.SpawnDamageText(damage, pos);

                    // Hit Sound is the part of SFX sound
                    PlayHitSound(hitSound);

                    // set the last spawn count
                    mLastSpawnPos = index;

                    // Look at the camera once!
                    if (mFaceCamera)
                    {
                        dt.transform.LookAt(Camera.main.transform.position);

                        dt.transform.Rotate(0.0f, 180.0f, 0.0f);
                    }

                    return;
                }
            }

            // if we get here mean we cycle once but we
            // did not spawn a text!
            // so reset the spawn pos and 
            // try to search agian until we find one!
            mLastSpawnPos = 0;

            // if function call the second time, 
            // and try to call the third time, 
            // exit the function call.
            // so prevent "stack overflow 
            // search/infinite function call".
            // IMPORTANT(JenChieh): it wont spawn damage text this time, 
            // if this happens.
            if (secondSearch)
            {
#if (UNITY_EDITOR)
                if (JCS_GameSettings.instance.DEBUG_MODE)
                {
                    JCS_Debug.LogWarning("Prevent, stack overflow function call.");
                }
#endif
                return;
            }

            // dangerious, use carefully!
            // make sure u have enough number of handle
            // or else the program might crash? (too many delay?)
            SpawnDamageTextFromPool(damage, pos, hitSound, true);
        }

        /// <summary>
        /// Initialize damage text to the array.
        /// </summary>
        private void InitDamageTextToArray()
        {
            mDamageTexts = new JCS_Vector<JCS_DamageText>(mNumberOfHandle);

            if (mDamageText == null)
                return;

            for (int count = 0; count < mNumberOfHandle; ++count)
            {
                // spawn a new game object, 
                // and get the component
                JCS_DamageText dt = JCS_Util.SpawnGameObject(
                    mDamageText,
                    mDamageText.transform.position,
                    mDamageText.transform.rotation) as JCS_DamageText;

                // add to array
                mDamageTexts.set(count, dt);

                // set parent
                dt.transform.SetParent(this.transform);
            }
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
            float timer,
            AudioClip hitSound)
        {
            float newTimer = timer;

            newTimer += Time.deltaTime;

            if (mTimePerSpawn < newTimer)
            {
                int count = mSequenceSpawnCount[processIndex];
                if (count == damage.Length)
                {
                    // done the sequence, do delete the process(thread)
                    EndProcessSequence(processIndex);
                    return;
                }

                SpawnDamageTextFromPool(damage[count], pos[count], hitSound);

                ++count;
                // update new count, in order 
                // to spawn next damage text
                mSequenceSpawnCount[processIndex] = count;
                newTimer = 0.0f;
            }

            // update timer
            mSequenceSpanwTimer[processIndex] = newTimer;
        }
        /// <summary>
        /// Loop through the thread and process each thread per frame, 
        /// in addition there is no priority of this thread system.
        /// 
        /// So it will calculate the data with O(n) time complexity in average.
        /// with no first and last!
        /// </summary>
        private void ProccessSequences()
        {
            for (int process = 0; process < mSequenceThread.Count; ++process)
            {
                // pass in all the data wee need in order to process the data
                Sequence(process,
                    mSequenceDamageData[process],
                    mSequencePosData[process],
                    mSequenceSpanwTimer[process],
                    mSequenceHitSoundData[process]);
            }
        }

        /// <summary>
        /// When end the process.
        /// </summary>
        /// <param name="processIndex"></param>
        private void EndProcessSequence(int processIndex)
        {
            mSequenceThread.RemoveAt(processIndex);
            mSequenceDamageData.RemoveAt(processIndex);
            mSequencePosData.RemoveAt(processIndex);
            mSequenceSpanwTimer.RemoveAt(processIndex);
            mSequenceSpawnCount.RemoveAt(processIndex);
            mSequenceHitSoundData.RemoveAt(processIndex);
        }

        /// <summary>
        /// Hit Sound is the part of sfx sound
        /// </summary>
        private void PlayHitSound(AudioClip hitSound)
        {
            if (hitSound != null)
            {
                // play the hit sound provide by passing in.
                mSoundPlayer.PlayOneShot(hitSound, JCS_SoundSettings.instance.GetSFXSound_Volume());
            }
            else
            {
                if (mHitSound != null)
                {
                    // play the regular assigned by variable's hit sound.
                    mSoundPlayer.PlayOneShot(mHitSound, JCS_SoundSettings.instance.GetSFXSound_Volume());
                }
            }
        }
    }
}
