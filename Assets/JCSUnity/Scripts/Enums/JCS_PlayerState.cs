/**
 * $File: JCS_PlayerState.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// List of the attack state here.
    /// </summary>
    public enum JCS_AttackState
    {
        NONE = 0,

        // multiple attack states
        ATTACK_01 = 20,
        ATTACK_02 = 21,
        ATTACK_03 = 22,
        ATTACK_04 = 23,
        ATTACK_05 = 24,
    }

    /// <summary>
    /// List of all the possible state here.
    /// </summary>
    public enum JCS_LiveObjectState
    {
        STAND = 0,
        WALK = 1,
        RAND_ATTACK = 2,     // master
        JUMP = 3,
        PRONE = 4,
        ALERT = 5,
        FLY = 6,
        LADDER = 7,
        ROPE = 8,
        SIT = 9,
        HIT = 10,        // when player get hit
        DANCE = 11,
        SWIM = 12,
        DIE = 13,       // when player is dead. (墓碑!)
        GHOST = 14,     // 當玩家死掉了之後
    }
}
