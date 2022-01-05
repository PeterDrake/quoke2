using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class QuokeTestUtils
{

    /// <summary>
    /// Presses each of the indicated keys, with a pause after each one, if needed, to allow time for the game to
    /// respond.
    /// </summary>
    public static IEnumerator Press(string keys, PlayerKeyboardManager playerKeyboard, 
        CheatKeyboardController cheatKeyboard = null, StrategicMapKeyboardController strategicMapKeyboard = null)
    {
        foreach (char c in keys)
        {
            KeyCode k;
            if (" ~<>".IndexOf(Char.ToUpper(c)) == -1)
            {
                k = (KeyCode) Enum.Parse(typeof(KeyCode), Char.ToUpper(c).ToString());
            }
            else
            {
                switch (c)
                {
                    case ' ':
                        k = KeyCode.Space;
                        break;
                    case '<':
                        k = KeyCode.Comma;
                        break;
                    case '>':
                        k = KeyCode.Period;
                        break;
                    case '~':
                        k = KeyCode.Return;
                        break;
                    default:
                        k = KeyCode.Space;
                        break;
                }
            }
            // There are two different objects receiving keypresses, but only one will react to a given key
            playerKeyboard.SetKeyDown(k);
            if (cheatKeyboard)
            {
                cheatKeyboard.SetKeyDown(k);
            }
            else if (strategicMapKeyboard)
            {
                strategicMapKeyboard.SetKeyDown(k);
            }

            if ("WASDL <>".IndexOf(Char.ToUpper(c)) != -1)
            {
                // This key take some time for the game to complete its response (e.g., movement)
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                yield return null;
            }
        }
    }
}
