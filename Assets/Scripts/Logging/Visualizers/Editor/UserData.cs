using System.Collections.Generic;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    public class UserData
    {
        #region Properties

        public string LoggingId { get; }

        public List<AggregatedSessionData> Sessions { get; }

        #endregion

        #region Methods

        public UserData(AggregatedSessionData session)
        {
            LoggingId = session.LoggingId;
            Sessions = new List<AggregatedSessionData> {session};
        }

        #endregion
    }
}
