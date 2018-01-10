﻿using Monkeyspeak;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// Legacy Furcadia channel processing
/// <para>
/// This lib handles the basic channels, Emote, Say (Speech and Spoken Furcadia commands), Whispers
/// </para>
/// <pra>Bot Testers: Be aware this class needs to be tested any way possible!</pra>
/// <para>
/// TODO: Upgrade to AngelCat style Channels and Reintegrate into the engine. These channels may
///       still work with the existing system. <see
///       href="http://bugtraq.tsprojects.org/view.php?id=107">[BUG: 107]</see>
/// </para>
/// </summary>
/// <remarks>
/// This Lib contains the following unnamed delegates:
/// <para>(0:1) When the bot logs into Furcadia,</para>
/// <para>(0:2) When the bot logs out of Furcadia,</para>
/// <para>(0:3) When the Furcadia client disconnects or closes,"</para>
/// <para>
/// (0:5) When anyone says something,
/// <para>Ignores Bots speech</para>
/// </para>
/// <para>
/// (0:8) When anyone shouts something,
/// <para>Ignores Bots speech</para>
/// </para>
/// <para>
/// (0:11) When anyone emotes something,
/// <para>Ignores Bots speech</para>
/// </para>
/// <para>
/// (0:15) When anyone whispers something,
/// <para>Ignores Bots speech</para>
/// </para>
/// <para>
/// (0:18) When anyone says or emotes something,
/// <para>Ignores Bots speech</para>
/// </para>
/// <para>
/// (0:21) When anyone emits something,
/// <para>Ignores Bots speech</para>
/// </para>
/// <para>(0:24) When anyone enters the Dream,</para>
/// <para>(0:26) When anyone leaves the Dream,</para>
/// <para>
/// (0:32) When anyone requests to summon the bot,
/// <para>Ignores Bots speech</para>
/// </para>
/// <para>
/// (0:34) When anyone requests to join the bot,
/// <para>Ignores Bots speech</para>
/// </para>
/// <para>
/// (0:36) When anyone requests to follow the bot,
/// <para>Ignores Bots speech</para>
/// </para>
/// <para>
/// (0:38) When anyone requests to lead the bot,
/// <para>Ignores Bots speech</para>
/// </para>
/// <para>
/// (0:40) When anyone requests to cuddle with the bot,
/// <para>Ignores Bots speech</para>
/// (0:92) When the bot detects the "Your throat is tired. Please wait a few seconds"message,
/// </para>
/// <para>(0:93) When the bot resumes processing after seeing ""Your throat is tired""message,</para>
/// <para>(5:0) say {..}. (Normal Furcadia Text commands)</para>
/// <para>(5:1) emote {..}.</para>
/// <para>(5:2) shout {..}.</para>
/// <para>(5:3) Emit {..}.</para>
/// <para>(5:4) Emitloud {..}.</para>
/// <para>(5:5) whisper {..} to the triggering furre.</para>
/// <para>(5:6) whisper {..} to furre named {..}.</para>
/// <para>(5:7) whisper {..} to furre named {..} even if they're off-line.</para>
/// </remarks>
public sealed class MsSayLibrary : MonkeySpeakLibrary
{
    #region Public Methods

    /// <summary>
    /// (0:29) When a furre named {..} enters the bots view,"
    /// </summary>
    /// <param name="reader"><see cref="TriggerReader"/></param>
    /// <returns>true on success</returns>
    public bool FurreNamedEnterView(TriggerReader reader)
    {
        ReadTriggeringFurreParams(reader);
        var tPlayer = DreamInfo.Furres.GerFurreByName(reader.ReadString());
        return tPlayer.Visible == tPlayer.WasVisible;
    }

    /// <summary>
    /// (0:31) When a furre named {..} leaves the bots view,
    /// </summary>
    /// <param name="reader"><see cref="TriggerReader"/></param>
    /// <returns>true on success</returns>
    public bool FurreNamedLeaveView(TriggerReader reader)
    {
        ReadTriggeringFurreParams(reader);
        var tPlayer = DreamInfo.Furres.GerFurreByName(reader.ReadString());
        return tPlayer.Visible == tPlayer.WasVisible;
    }

    /// <summary>
    /// Initializes this instance. Add your trigger handlers here.
    /// </summary>
    /// <param name="args">Parametized argument of objects to use to pass runtime objects to a library at initialization</param>
    public override void Initialize(params object[] args)
    {
        base.Initialize(args);

        // says
        Add(TriggerCategory.Cause, 5,
            r => ReadTriggeringFurreParams(r) && !IsConnectedCharacter(Player),
            "When anyone says something,");

        Add(TriggerCategory.Cause, 6,
            r => MsgIs(r),
            "When anyone says {..},");

        // (0:7) When some one says something with {..} in it
        Add(TriggerCategory.Cause, 7,
            r => MsgContains(r),
            "When anyone says something with {..} in it,");

        // Shouts
        Add(TriggerCategory.Cause, 8,
             r => ReadTriggeringFurreParams(r) && !IsConnectedCharacter(Player),
            "When anyone shouts something,");

        Add(TriggerCategory.Cause, 9,
            r => MsgIs(r),
            "When anyone shouts {..},");

        // (0:10) When some one shouts something with {..} in it
        Add(TriggerCategory.Cause, 10,
            r => MsgContains(r),
            "When anyone shouts something with {..} in it,");

        // emotes
        Add(TriggerCategory.Cause, 11,
            r => ReadTriggeringFurreParams(r) && !IsConnectedCharacter(Player),
            "When anyone emotes something,");

        Add(TriggerCategory.Cause, 12,
            r => MsgIs(r),
            "When anyone emotes {..},");

        // (0:13) When some one emotes something with {..} in it
        Add(TriggerCategory.Cause, 13,
            r => MsgContains(r),
            "When anyone emotes something with {..} in it,");

        // Whispers
        Add(TriggerCategory.Cause, 15,
            r => ReadTriggeringFurreParams(r) && !IsConnectedCharacter(Player),
            "When anyone whispers something,");

        Add(TriggerCategory.Cause, 16,
            r => MsgIs(r),
            "When anyone whispers {..},");

        // (0:13) When some one emotes something with {..} in it
        Add(TriggerCategory.Cause, 17,
            r => MsgContains(r),
            "When anyone whispers something with {..} in it,");

        // Says or Emotes
        Add(TriggerCategory.Cause, 18,
            r => ReadTriggeringFurreParams(r),
            "When anyone says or emotes something,");

        Add(TriggerCategory.Cause, 19,
            r => MsgIs(r),
            "When anyone says or emotes {..},");

        // (0:13) When some one emotes something with {..} in it
        Add(TriggerCategory.Cause, 20,
            r => MsgContains(r),
            "When anyone says or emotes something with {..} in it,");

        Add(TriggerCategory.Cause, 21,
               r => ReadTriggeringFurreParams(r) && ReadDreamParams(r),
             "When someone emits something,");

        Add(TriggerCategory.Cause, 22,
            r => MsgIs(r),
            "When someone emits {..},");

        Add(TriggerCategory.Cause, 23,
          r => MsgContains(r),
          "When someone emits something with {..} in it,");

        // Furre In View
        // TODO: Move to Movement?
        // (0:28) When anyone enters the bots view,
        Add(TriggerCategory.Cause, 28,
            r => EnterView(r),
            "When anyone enters the bots view, ");

        // (0:28) When a furre named {..} enters the bots view
        Add(TriggerCategory.Cause, 29,
            r => FurreNamedEnterView(r),
            "When a furre named {..} enters the bots view,");

        // Furre Leave View
        // (0:30) When anyone leaves the bots view,
        Add(TriggerCategory.Cause, 30,
            r => LeaveView(r),
            "When anyone leaves the bots view, ");

        // (0:31) When a furre named {..} leaves the bots view
        Add(TriggerCategory.Cause, 31,
            r => FurreNamedLeaveView(r),
            "When a furre named {..} leaves the bots view,");

        // Summon
        // (0:32) When anyone requests to summon the bot,
        Add(TriggerCategory.Cause, 32, r =>
            ReadTriggeringFurreParams(r) && "summon" == r.GetParameter<string>(),
            "When anyone requests to summon the bot,");

        // (0:33) When a furre named {..} requests to summon the bot,
        Add(TriggerCategory.Cause, 33,
            r => NameIs(r) && "summon" == r.GetParameter<string>(),
            "When a furre named {..} requests to summon the bot,");

        // Join
        // (0:34) When anyone requests to join the bot,
        Add(TriggerCategory.Cause, 34,
           r => ReadTriggeringFurreParams(r) && "join" == r.GetParameter<string>(),
           "When anyone requests to join the bot,");

        // (0:35) When a furre named {..} requests to join the bot,

        Add(TriggerCategory.Cause, 35,
            r => NameIs(r) && "join" == r.GetParameter<string>(),
            "When a furre named {..} requests to join the bot,");
        // Follow
        // (0:36) When anyone requests to follow the bot,
        Add(TriggerCategory.Cause, 36,
           r => ReadTriggeringFurreParams(r) && "follow" == r.GetParameter<string>(),
           "When anyone requests to follow the bot,");

        // (0:37) When a furre named {..} requests to follow the bot,
        Add(TriggerCategory.Cause, 37,
            r => NameIs(r) && "follow" == r.GetParameter<string>(),
            "When a furre named {..} requests to follow the bot,");
        // Lead

        // (0:38) When anyone requests to lead the bot,
        Add(TriggerCategory.Cause, 38,
            r => ReadTriggeringFurreParams(r) && "lead" == r.GetParameter<string>(),
            "When anyone requests to lead the bot,");

        // (0:39) When a furre named {..} requests to lead the bot,
        Add(TriggerCategory.Cause, 39,
            r => NameIs(r) && "lead" == r.GetParameter<string>(),
            "When a furre named {..} requests to lead the bot,");

        // Cuddle
        // (0:40) When anyone requests to cuddle with the bot.
        Add(TriggerCategory.Cause, 40,
            r => ReadTriggeringFurreParams(r) && "cuddle" == r.GetParameter<string>(),
            "When anyone requests to cuddle with the bot,");

        // (0:41) When a furre named {..} requests to cuddle with the bot,
        Add(TriggerCategory.Cause, 41,
            r => NameIs(r) && "cuddle" == r.GetParameter<string>(),
            "When a furre named {..} requests to cuddle with the bot,");

        //  (1:3) and the triggering furre's name is {..},
        Add(TriggerCategory.Condition, 5,
            r => NameIs(r),
            "and the triggering furre\'s name is {..},");

        //  (1:4) and the triggering furre's name is not {..},
        Add(TriggerCategory.Condition, 6,
            r => !NameIs(r),
            "and the triggering furre\'s name is not {..},");

        //  (1:5) and the Triggering Furre's message is {..}, (say, emote,
        //  shot, whisper, or emit Channels)
        Add(TriggerCategory.Condition, 7,
            r => MsgIs(r),
            "and the triggering furre\'s message is {..},");

        //  (1:8) and the triggering furre's message contains {..} in it,
        //  (say, emote, shot, whisper, or emit Channels)
        Add(TriggerCategory.Condition, 8,
            r => MsgContains(r),
            "and the triggering furre\'s message contains {..} in it,");

        // (1:9) and the triggering furre's message does not contain {..} in it,
        // (say, emote, shot, whisper, or emit Channels)
        Add(TriggerCategory.Condition, 9,
            r => !MsgContains(r),
            "and the triggering furre\'s message does not contain {..} in it,");

        // (1:10) and the triggering furre's message is not {..},
        // (say, emote, shot, whisper, or emit Channels)
        Add(TriggerCategory.Condition, 10,
            r => !MsgIs(r),
            "and the triggering furre\'s message is not {..},");

        // (1:11) and triggering furre's message starts with {..},
        Add(TriggerCategory.Condition, 11,
            r => MsgStartsWith(r),
            "and triggering furre\'s message starts with {..},");

        // (1:12) and triggering furre's message doesn't start with {..},
        Add(TriggerCategory.Condition, 12,
            r => MsgNotStartsWith(r),
            "and triggering furre\'s message doesn\'t start with {..},");

        // (1:13) and triggering furre's message  ends with {..},
        Add(TriggerCategory.Condition, 13,
            r => MsgEndsWith(r),
            "and triggering furre\'s message  ends with {..},");

        // (1:14) and triggering furre's message doesn't end with {..},
        Add(TriggerCategory.Condition, 14,
            r => MsgNotEndsWith(r),
            "and triggering furre\'s message doesn\'t end with {..},");

        // (1:904) and the triggering furre is the Bot Controller,
        Add(TriggerCategory.Condition, 15,
            r => TriggeringFurreIsBotController(r),
            "and the triggering furre is the Bot Controller,");

        // (1:905) and the triggering furre is not the Bot Controller,
        Add(TriggerCategory.Condition, 16,
         r => !TriggeringFurreIsBotController(r),
            "and the triggering furre is not the Bot Controller,");

        // Says
        //  (5:0) say {..}.
        Add(TriggerCategory.Effect, 0,
            r => SendSay(r.ReadString()),
            "say {..}.");

        // emotes
        //  (5:1) emote {..}.
        Add(TriggerCategory.Effect, 1,
            r => SendEmote(r.ReadString()),
            "emote  message{..}.");

        // Shouts
        //  (5:2) shout {..}.
        Add(TriggerCategory.Effect, 2,
            r => SndShout(r.ReadString()),
            "shout message {..}.");

        // Emits
        //  (5:3) emit {..}.
        Add(TriggerCategory.Effect, 3,
            r => SendEmit(r.ReadString()),
            "emit message {..}.");

        //  (5:4) emitloud {..}.
        Add(TriggerCategory.Effect, 4,
            r => SendEmitLoud(r.ReadString()),
            "emit-loud message {..}.");

        // Whispers
        //  (5:5) whisper {..} to the triggering furre.
        Add(TriggerCategory.Effect, 5,
            r => SndWhisper(Player.ShortName, r.ReadString()),
            "whisper {..} to the triggering furre.");

        //  (5:6) whisper {..} to {..}.
        Add(TriggerCategory.Effect, 6, r =>
        {
            string msg = r.ReadString();
            string tname = r.ReadString();
            return SndWhisper(tname, msg);
        }, "whisper {..} to furre named {..}.");

        //  (5:7) whisper {..} to {..} even if they're off-line.
        Add(TriggerCategory.Effect, 7, r =>
        {
            string msg = r.ReadString();
            string tname = r.ReadString();
            return SendOffLineWhisper(tname, msg);
        }, "whisper {..} to furre named {..} even if they\'re off-line.");
    }

    /// <summary>
    /// Called when page is disposing or resetting.
    /// </summary>
    /// <param name="page">The page.</param>
    public override void Unload(Page page)
    {
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// (0:28) When anyone enters the bots view,
    /// </summary>
    /// <param name="reader">
    /// <see cref="TriggerReader"/>
    /// </param>
    /// <returns>
    /// true on success
    /// </returns>
    private bool EnterView(TriggerReader reader)
    {
        ReadTriggeringFurreParams(reader);
        return Player.Visible == Player.WasVisible;
    }

    /// <summary>
    /// (0:30) When anyone leaves the bots view,
    /// </summary>
    /// <param name="reader">
    /// <see cref="TriggerReader"/>
    /// </param>
    /// <returns>
    /// true on success
    /// </returns>
    private bool LeaveView(TriggerReader reader)
    {
        ReadTriggeringFurreParams(reader);
        return Player.Visible == Player.WasVisible;
    }

    private bool MsgNotStartsWith(TriggerReader reader)
    {
        return !MsgStartsWith(reader);
    }

    /// <summary>
    /// send a local emit to the server queue
    /// </summary>
    /// <param name="msg">
    /// message to send
    /// </param>
    private bool SendEmit(string msg)
    {
        if (!string.IsNullOrWhiteSpace(msg))
        {
            return SendServer($"emit {msg}");
        }

        return false;
    }

    /// <summary>
    /// send an emitloud command to the server queue
    /// </summary>
    /// <param name="msg">
    /// message to send
    /// </param>
    private bool SendEmitLoud(string msg)
    {
        if (!string.IsNullOrWhiteSpace(msg))
        {
            return SendServer($"emitloud {msg}");
        }

        return false;
    }

    /// <summary>
    /// Send an emote to the server queue
    /// </summary>
    /// <param name="msg">
    /// message to send
    /// </param>
    private bool SendEmote(string msg)
    {
        if (!string.IsNullOrWhiteSpace(msg))
        {
            return SendServer($":{ msg}");
        }

        return false;
    }

    /// <summary>
    /// Send an off line whisper to the server queue
    /// </summary>
    /// <param name="name">
    /// recipients name
    /// </param>
    /// <param name="msg">
    /// Message to send
    /// </param>
    private bool SendOffLineWhisper(string name, string msg)
    {
        if (!string.IsNullOrWhiteSpace(msg))
        {
            return SendServer($"/%%{name.ToFurcadiaShortName()} {msg}");
        }

        return false;
    }

    /// <summary>
    /// send a speech command to the server queue
    /// </summary>
    /// <param name="msg">
    /// message to send
    /// </param>
    private bool SendSay(string msg)
    {
        if (!string.IsNullOrWhiteSpace(msg))
        {
            return SendServer(msg);
        }

        return false;
    }

    /// <summary>
    /// Send a shout to the server queue
    /// </summary>
    /// <param name="msg">
    /// Message to send
    /// </param>
    private bool SndShout(string msg)
    {
        if (!string.IsNullOrWhiteSpace(msg))
        {
            return SendServer($"-{ msg}");
        }

        return false;
    }

    /// <summary>
    /// Send a whisper to the server queue
    /// </summary>
    /// <param name="name">
    /// recipients name
    /// </param>
    /// <param name="msg">
    /// Message to send
    /// </param>
    private bool SndWhisper(string name, string msg)
    {
        if (!string.IsNullOrWhiteSpace(msg))
        {
            return SendServer($"/%{name.ToFurcadiaShortName()} {msg}");
        }

        return false;
    }

    #endregion Private Methods
}