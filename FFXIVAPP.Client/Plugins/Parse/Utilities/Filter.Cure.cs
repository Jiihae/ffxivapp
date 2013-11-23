﻿// FFXIVAPP.Client
// Filter.Cure.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Events;
using NLog;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessCure(Event e, Expressions exp)
        {
            var line = new Line
            {
                RawLine = e.RawLine
            };
            var cure = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                    switch (e.Direction)
                    {
                        case EventDirection.You:
                        case EventDirection.Party:
                            cure = exp.pCure;
                            if (cure.Success)
                            {
                                line.Source = _lastNamePlayer;
                                UpdateHealingPlayer(cure, line, exp, false);
                            }
                            break;
                    }
                    break;
                case EventSubject.Party:
                    switch (e.Direction)
                    {
                        case EventDirection.You:
                        case EventDirection.Party:
                            cure = exp.pCure;
                            if (cure.Success)
                            {
                                line.Source = _lastNameParty;
                                UpdateHealingPlayer(cure, line, exp);
                            }
                            break;
                    }
                    break;
            }
            if (cure.Success)
            {
                return;
            }
            ParsingLogHelper.Log(LogManager.GetCurrentClassLogger(), "Cure", e, exp);
        }

        private static void UpdateHealingPlayer(Match cure, Line line, Expressions exp, bool isParty = true)
        {
            _isParty = isParty;
            try
            {
                line.Action = isParty ? _lastActionParty : _lastActionPlayer;
                line.Amount = cure.Groups["amount"].Success ? Convert.ToDecimal(cure.Groups["amount"].Value) : 0m;
                line.Crit = cure.Groups["crit"].Success;
                line.Modifier = cure.Groups["modifier"].Success ? Convert.ToDecimal(cure.Groups["modifier"].Value) / 100 : 0m;
                line.Target = Convert.ToString(cure.Groups["target"].Value);
                line.RecLossType = Convert.ToString(cure.Groups["type"].Value.ToUpper());
                if (line.IsEmpty())
                {
                    return;
                }
                if (line.RecLossType == exp.HealingType)
                {
                    ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                                .SetHealing(line);
                }
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(LogManager.GetCurrentClassLogger(), "Cure", exp.Event, ex);
            }
        }
    }
}