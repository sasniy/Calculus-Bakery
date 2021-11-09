using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Model.Transporter
{
    internal sealed class Transporter : MonoBehaviour
    {
        [SerializeField] private float _timeTransitionInSeconds = 1;
        private bool _isMoving = true;

        private Platform _platform;
        private IReadOnlyDictionary<PipePosition, Pipe> _pipes;

        private Pipe _servedPipe;

        private void Start()
        {
            _platform = FindObjectOfType<Platform>();

            _pipes = FindObjectsOfType<Pipe>()
                .OrderBy(pipe => pipe.Location)
                .ToDictionary(pipe => pipe.Location);

            _servedPipe = _pipes[PipePosition.Left];
            StartCoroutine(MoveTo(PipePosition.Left));
        }

        public void TryMoveToward(Direction direction)
        {
            if (_isMoving)
                return;

            var target = (int)_servedPipe.Location + (int)direction;

            if (target < (int)Direction.Left || target > (int)Direction.Right)
                return;

            _isMoving = true;
            StartCoroutine(MoveTo((PipePosition)target));
        }

        private IEnumerator MoveTo(PipePosition location)
        {
            var nextPipe = _pipes[location];
            
            var start = _platform.transform.localPosition;
            var end = new Vector3(nextPipe.transform.localPosition.x, start.y, start.z);
            for (var t = 0f; t < 1; t += Time.deltaTime / _timeTransitionInSeconds)
            {
                var easingTime = t < 0.5 ? t * t * 2 : 1 - (1 - t) * (1 - t) * 2;

                _platform.transform.localPosition = Vector3.Lerp(start, end, easingTime);

                yield return null;
            }

            _platform.transform.localPosition = end;
            _servedPipe = nextPipe;

            _isMoving = false;
        }
    }
}
