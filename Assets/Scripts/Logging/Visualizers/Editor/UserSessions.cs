using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    [CreateAssetMenu(fileName = "UserSessions", menuName = "Game/User Sessions")]
    public class UserSessions : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private List<UserData> users = new List<UserData>();

        #endregion

        #region Properties

        public List<UserData> Users => users;

        #endregion
    }
}
