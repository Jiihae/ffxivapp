﻿// FFXIVAPP
// SpeechHelper.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Speech.Synthesis;
using NLog;

namespace FFXIVAPP.Classes.Helpers
{
    internal static class SpeechHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        /// <param name="message"> </param>
        public static void Speak(string message)
        {
            using (var synthesizer = new SpeechSynthesizer())
            {
                synthesizer.SetOutputToDefaultAudioDevice();
                var builder = new PromptBuilder();
                builder.AppendText(message);
                synthesizer.Speak(builder);
            }
        }
    }
}