/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mx
{
    public abstract class Mx
    {
        /* Variables */

        public const string NAME = "Mx";

        public static readonly Version VERSION = new Version("0.1.2");

        /* Setter & Getter */

        /* Functions */

        public virtual bool Enable() { return true; }

        public static void CompletingRead(
            string prompt,
            Dictionary<string, MxItem> collection,
            CompletingReadCallback callback,
            CompletingReadCallback hover = null,
            bool requiredMatch = true)
        {
            MxCompletionWindow.OverrideIt(prompt, collection, callback, hover, requiredMatch);
        }

        public static void CompletingRead(
            string prompt,
            Dictionary<string, string> collection,
            CompletingReadCallback callback,
            CompletingReadCallback hover = null,
            bool requiredMatch = true)
        {
            var newCollection = collection.ToDictionary(x => x.Key, x => new MxItem(summary: x.Value));

            MxCompletionWindow.OverrideIt(prompt, newCollection, callback, hover, requiredMatch);
        }

        private static void CompletingRead(
            string prompt,
            List<string> candidates,
            List<string> summaries,
            CompletingReadCallback callback,
            CompletingReadCallback hover = null,
            bool requiredMatch = true)
        {
            Dictionary<string, string> collection;

            if (candidates != null)
            {
                if (summaries == null)
                    summaries = new List<string>(new string[candidates.Count]);

                collection = candidates
                    .Zip(summaries, (k, v) => new { k, v })
                    .ToDictionary(x => x.k, x => x.v);
            }
            else
            {
                collection = new Dictionary<string, string>();
            }

            CompletingRead(prompt, collection, callback, hover, requiredMatch);
        }

        public static void CompletingRead(
            string prompt, 
            List<string> candidates, 
            CompletingReadCallback callback,
            CompletingReadCallback hover = null,
            bool requiredMatch = true)
        {
            CompletingRead(prompt, candidates, null, callback, hover, requiredMatch);
        }

        public static void CompletingRead<T>(
            string prompt,
            List<T> candidates,
            CompletingReadCallback callback,
            CompletingReadCallback hover = null,
            bool requiredMatch = true)
        {
            var _candidates = MxUtil.ToListString(candidates);
            CompletingRead(prompt, _candidates, null, callback, hover, requiredMatch);
        }

        public static void CompletingRead(
            string prompt,
            (List<string>, List<string>) collection,
            CompletingReadCallback callback,
            CompletingReadCallback hover = null,
            bool requiredMatch = true)
        {
            CompletingRead(prompt, collection.Item1, collection.Item2, callback, hover, requiredMatch);
        }

        public static void ReadString(
            string prompt, CompletingReadCallback callback)
        {
            CompletingRead(prompt, null, null, callback, null, false);
        }

        public static void ReadNumber(
            string prompt, CompletingReadCallback callback)
        {
            CompletingRead(prompt, null, null, (answer, summary) =>
                {
                    float number;

                    if (!float.TryParse(answer, out number))
                    {
                        MxCompletionWindow.INHIBIT_CLOSE = true;
                        Debug.LogError("Invalid number: " + answer);
                        return;
                    }

                    callback.Invoke(answer, null);
                }, null, false);
        }

        public static void YesOrNo(string prompt, CompletingReadCallback callback)
        {
            CompletingRead(prompt, new List<string>() { "Yes", "No"}, callback);
        }
    }
}
