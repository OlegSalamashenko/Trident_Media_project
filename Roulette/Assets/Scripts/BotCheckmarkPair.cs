using UnityEngine;

public class BotCheckmarkPair
{
    public Bot Bot { get; private set; }
    public RectTransform Checkmark { get; private set; }

    public BotCheckmarkPair(Bot bot, RectTransform checkmark)
    {
        Bot = bot;
        Checkmark = checkmark;
    }
}