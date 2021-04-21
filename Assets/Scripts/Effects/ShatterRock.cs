using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NeanderthalTools.Util;
using UnityEngine;

namespace NeanderthalTools.Effects
{
    public class ShatterRock : MonoBehaviour
    {
        #region Helpers

        private class RockPiece
        {
            public Rigidbody Rigidbody { get; }

            public Vector3 InitialScale { get; }

            public RockPiece(Rigidbody rigidbody, Vector3 initialScale)
            {
                Rigidbody = rigidbody;
                InitialScale = initialScale;
            }
        }

        #endregion

        #region Editor

        [SerializeField]
        private GameObject complete;

        [SerializeField]
        private GameObject shattered;

        [SerializeField]
        private float scaleDownDuration = 5f;

        [SerializeField]
        private float scaleDownDelay = 2f;

        #endregion

        #region Fields

        private List<RockPiece> pieces;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            pieces = GetPieces();
        }

        #endregion

        #region Methods

        public void Shatter()
        {
            complete.SetActive(false);
            shattered.SetActive(true);

            StartCoroutine(ScaleDown());
        }

        private List<RockPiece> GetPieces()
        {
            return shattered
                .GetComponentsInChildren<Rigidbody>(true)
                .Select(body => new RockPiece(body, body.transform.localScale))
                .ToList();
        }

        private IEnumerator ScaleDown()
        {
            yield return new WaitForSeconds(scaleDownDelay);
            yield return Coroutines.Progress(1f, 0f, scaleDownDuration, SetScale);

            ClearPieces();
        }

        private void SetScale(float scaleProgress)
        {
            for (var index = pieces.Count - 1; index >= 0; index--)
            {
                var piece = pieces[index];
                var pieceTransform = piece.Rigidbody.transform;

                pieceTransform.localScale = piece.InitialScale * scaleProgress;
            }
        }

        private void ClearPieces()
        {
            foreach (var piece in pieces)
            {
                Destroy(piece.Rigidbody.gameObject);
            }

            pieces.Clear();
        }

        #endregion
    }
}
