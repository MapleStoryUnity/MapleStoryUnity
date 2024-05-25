/**
 * $File: npc2100.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Example dialogue script.
    /// 
    /// Here show the basic usage of the dialogue script system.
    /// </summary>
    public class npc2100 : JCS_DialogueScript
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override void Action(int mode, int type, int selection)
        {
            // if quest complete
            if (mode == -1)
                ds.Dispose();

            if (Status == -1)
            {
                ds.SendNext("Hello World");
                ds.SendNameTag("Name 01");

                ds.SendLeftImage(LeftSprite);
            }
            else if (Status == 0)
            {
                /* For Mouse. */
                {
                    ds.SendNextPrev("Make a selection...");
                }
                /* For Gamepad/Controller/Joystick. */
                {
                    //ds.SendEmpty("Make a selection...");
                }

                //ds.SendChoice(0, "Selection 01");
                ds.SendChoice(1, "Selection 02");
                ds.SendChoice(2, "Selection 03");
                ds.SendChoice(3, "Selection 04");
                ds.SendChoice(4, "Selection 05");
                ds.SendChoice(5, "Selection 06");

                ds.SendNameTag("Name 02");

                ds.SendRightImage(RightSprite);
            }
            else if (Status == 1)
            {
                string msg = "Error text...";
                switch (selection)
                {
                    case 1:
                        msg = "You made selection 02.";
                        break;
                    case 2:
                        msg = "You made selection 03.";
                        break;
                    case 3:
                        msg = "You made selection 04.";
                        break;
                    case 4:
                        msg = "You made selection 05.";
                        break;
                    case 5:
                        msg = "You made selection 06.";
                        break;

                    default:
                        // make selection 0 default
                        msg = "You made selection 01.";
                        break;
                }
                ds.SendNextPrev(msg);
            }
            else if (Status == 2)
            {
                ds.SendYesNo("...");
            }
            else if (Status == 3)
            {
                string msg = "Default text";
                switch (selection)
                {
                    // Yes button clicked!
                    case 1:
                        msg = "You press Yes button!!! n(^O^)n";
                        break;
                    // No button clicked~
                    case 0:
                        msg = "You press No button!!! n(_ _)n";
                        break;
                }
                ds.SendOk(msg);
            }
        }
    }
}
