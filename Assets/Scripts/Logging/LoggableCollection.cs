﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Logging
{
    [CreateAssetMenu(fileName = "LoggableCollection", menuName = "Game/Loggable Collection")]
    public class LoggableCollection : ScriptableObject, IEnumerable<ILoggable>
    {
        #region Editor

        [NonSerialized]
        private readonly List<ILoggable> loggables = new List<ILoggable>();

        #endregion

        #region Methods

        public void Add(ILoggable loggable)
        {
            if (loggables.Contains(loggable))
            {
                return;
            }

            loggables.Add(loggable);
        }

        public void Remove(ILoggable loggable)
        {
            loggables.Remove(loggable);
        }

        #endregion

        #region Overrides

        public IEnumerator<ILoggable> GetEnumerator()
        {
            return loggables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
