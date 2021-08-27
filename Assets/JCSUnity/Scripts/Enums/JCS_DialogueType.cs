/**
 * $File: JCS_DialogueType.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// type of dialogue
    /// </summary>
    public enum JCS_DialogueType
    {
        // Special Priority
        GAME_UI,         // default
        UI_EFFECT,

        //*** Low Priority ***//

        // any screen in game, only allow one type
        // Conversation Dialogue
        NPC_DIALOGUE,

        // can have multiple same type at the same time
        // For instance, inventory, equip, use, etc.
        PLAYER_DIALOGUE,

        // application layer
        SYSTEM_DIALOGUE,      // screen from application layer so it have the authroized to break the game


        //*** Hight Priority ***//
    }
}
