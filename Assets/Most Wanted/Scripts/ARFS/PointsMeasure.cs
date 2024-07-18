using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ARFS.Tools.Measure
{
    public class PointsMeasure : MonoBehaviour
    {
        [SerializeField]bool enable;
        [SerializeField]Transform _pointRU;
        [SerializeField]Transform _pointRD;
        [SerializeField]Transform _pointFU;
        [SerializeField]Transform _pointFD;

        [SerializeField] Vector3 distance;
        [SerializeField] Vector3 steps = new(1, 1, 1);
        [SerializeField] Vector3 unitDistance;

        Transform CheckIfChange(Transform oldValue, Transform newValue)
        {
            FindTransforms();
            if (newValue.position != oldValue.position)
            {
                OnPositionChange += OnPositionChangeAction;
                OnPositionChange();
                return newValue;
            }else
            {
                return oldValue;
            }
        }

        // Delegado para el evento
        public delegate void ValueHandler();

        // Evento basado en el delegado
        public event ValueHandler OnPositionChange;

        void OnPositionChangeAction()
        {
            if (_pointFU == null || _pointRU == null || _pointFD == null || _pointFU == null) FindTransforms();

            if (_pointFU == null || _pointRU == null || _pointFD == null) return;
            Vector3 up = _pointFU.position - _pointRU.position;
            up = up.magnitude * Vector3.right;
            Vector3 Left = _pointFU.position - _pointFD.position;
            Left = Left.magnitude * Vector3.forward;

            distance = new Vector3(
                (up.magnitude), (Left.magnitude), 0);
            unitDistance = new Vector3(
                (up.magnitude / steps.x), (Left.magnitude / steps.y), 0);

        }
        void FindTransforms()
        {
            if (!(_pointFU == null || _pointRU == null || _pointFD == null || _pointFU == null)) return;
            _pointRU = gameObject.transform.Find("pointRU").transform;
            _pointRD = gameObject.transform.Find("pointRD").transform;
            _pointFU = gameObject.transform.Find("pointFU").transform;
            _pointFD = gameObject.transform.Find("pointFD").transform;
        }
        private void OnDrawGizmos()
        {
            if (!enable) return;
            FindTransforms();
            OnPositionChangeAction();
        }

    }
}
