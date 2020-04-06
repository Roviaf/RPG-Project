using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CoreFeatures
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        void LateUpdate()
        {
            transform.position = target.transform.position;
        }
    }
}

