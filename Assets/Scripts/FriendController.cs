using System.Collections;
using System.Collections.Generic;
using LivelyChatBubbles;
using UnityEngine;

public class FriendController : MonoBehaviour
{
    ChatMouthpiece mouthpiece;
    bool spoken;

    // Start is called before the first frame update
    void Start()
    {
        mouthpiece = GetComponent<ChatMouthpiece>();
        spoken = false;
        // mouthpiece.Speak("Hey...");
    }

    // Update is called once per frame
    void Update()
    {
        if (!spoken)
        {
            mouthpiece.Speak("Hey...");
            spoken = true;
        }
    }

    public void GiveInstructions()
    {
        mouthpiece.Speak("The vending machines ran out of money! LEFT CLICK to throw a coin...");
    }
}
