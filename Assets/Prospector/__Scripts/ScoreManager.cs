using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An enum to handle all the possible scoring events
public enum eScoreEvent
{
    draw,
    mine,
    mineGold, gameWin,
    gameLoss
}

public class ScoreManager : MonoBehaviour
{
    static private ScoreManager S;

    static public int SCORE_FROM_PREV_ROUND = 0;
    static public int HIGH_SCORE = 0;
    [Header("Set Dynamically")]
    // Fields to track score info
    public int score = 0;
    void Awake()
    {
        if (S == null)
        {
            S = this;
        } else
        {
            Debug.LogError("ERROR: ScoreManager.Awake(): S is already set!");
        }

        // Check for a high score in PlayerPrefs
        if (PlayerPrefs.HasKey("ProspectorHighScore"))
        {
            HIGH_SCORE = PlayerPrefs.GetInt("ProspectorHighScore");
        }

        // Add the score from last round, which will be >0 if it was a win
        score += SCORE_FROM_PREV_ROUND;
        // And reset the SCORE_FROM_PREV_ROUND
        SCORE_FROM_PREV_ROUND = 0;
    }

    static public void EVENT(eScoreEvent evt)
    {
        // try-catch stops an error from breaking the progam
        try
        {
            S.Event(evt);
        } catch (System .NullReferenceException nre) {
            Debug.LogError ("ScoreManager:EVENT() called while S=null. \n" + nre) ;
        }
    }

    void Event(eScoreEvent evt)
    {
        switch (evt)
        {
            // Same things need to happen whether it's a draw, a win, or a loss
            case eScoreEvent.draw: // Drawing a card
            case eScoreEvent.gameWin: // Won the round
            case eScoreEvent.gameLoss: // Lost the round
            score = 0; // reset scoreRun
            break;
            case eScoreEvent.mine: // Remove a mine card
            score = score +1; // increase the score 
            break;
        }
        switch (evt)
        {
            case eScoreEvent.gameWin:
                SCORE_FROM_PREV_ROUND = score;
                print("You won this round! Round score: " + score);
                break;

            case eScoreEvent.gameLoss:
                if (HIGH_SCORE <= score)
                {
                    print("You got the high score! High score: " + score);
                    HIGH_SCORE = score;
                    PlayerPrefs.SetInt("ProspectorHighScore", score);
                } else
                {
                    print("Your final score for the game was: " + score);
                }
                break;

            default:
                print("score: " + score );
                break;
        }
    }

    static public int SCORE { get { return S.score; } }
}
