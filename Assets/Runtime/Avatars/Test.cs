using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ubiq.Avatars
{
    public class Test : MonoBehaviour
    {

        GameObject _localAvatarObj;

        public void SetLocalAvatar(GameObject localAvatarObj){
            this._localAvatarObj = localAvatarObj;
            Debug.Log("Get the current local avatar object");
        }
    }

}
