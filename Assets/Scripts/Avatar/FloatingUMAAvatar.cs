using Ubiq.Avatars;
using UnityEngine;

namespace Ubiq.FloatingAvatar{

//This script manage how float avatar
[RequireComponent(typeof(Avatars.Avatar))]
[RequireComponent(typeof(ThreePointTrackedAvatar))]

[System.Serializable]
public class VRMap{
    public Transform rigTarget;
    public Vector3 posOffset;
    public Vector3 rotOffset;

    public void Map(Vector3 pos, Quaternion rot){
        rigTarget.position = pos + posOffset;
        rigTarget.rotation = rot * Quaternion.Euler(rotOffset);
        
    }
}
    public class FloatingUMAAvatar : MonoBehaviour
    {
        public VRMap headRig; //to map HMD
        public VRMap leftRig; //to map left controller
        public VRMap rightRig; //to map right controller
            
        public Transform headIK;
       
        //To catch Avatar
        private Avatars.Avatar avatar;

        //Track VR HMD, Left,Right Hands
        private ThreePointTrackedAvatar trackedAvatar;
        private float turnsmoothness = 7;

        private void Awake()
        {
            avatar = GetComponent<Avatars.Avatar>();
            trackedAvatar = GetComponent<ThreePointTrackedAvatar>();
        }

        void Start(){
            //headBodyOffset = transform.position - headContraint.position;
        }

        private void OnEnable()
        {
            trackedAvatar.OnHeadUpdate.AddListener(ThreePointTrackedAvatar_OnHeadUpdate);
            trackedAvatar.OnLeftHandUpdate.AddListener(ThreePointTrackedAvatar_OnLeftHandUpdate);
            trackedAvatar.OnRightHandUpdate.AddListener(ThreePointTrackedAvatar_OnRightHandUpdate);
        }

        private void OnDisable()
        {
            if (trackedAvatar && trackedAvatar != null)
            {
                trackedAvatar.OnHeadUpdate.RemoveListener(ThreePointTrackedAvatar_OnHeadUpdate);
                trackedAvatar.OnLeftHandUpdate.RemoveListener(ThreePointTrackedAvatar_OnLeftHandUpdate);
                trackedAvatar.OnRightHandUpdate.RemoveListener(ThreePointTrackedAvatar_OnRightHandUpdate);
            }
        }

        private void ThreePointTrackedAvatar_OnHeadUpdate(Vector3 pos, Quaternion rot)
        {            
            headRig.Map(pos, rot);
            this.transform.position = pos;
            
            /* rot.x=0;
            rot.z=0;            
            this.transform.rotation = rot; */

            //To rotate avatar body in more realistic
            this.transform.forward = Vector3.Lerp(transform.forward, 
            Vector3.ProjectOnPlane(headIK.forward, Vector3.up).normalized, Time.deltaTime * turnsmoothness);
            
        }

        private void ThreePointTrackedAvatar_OnLeftHandUpdate(Vector3 pos, Quaternion rot)
        {
            leftRig.Map(pos, rot);
        }

        private void ThreePointTrackedAvatar_OnRightHandUpdate(Vector3 pos, Quaternion rot)
        {
            rightRig.Map(pos, rot);            
        }

        private void LateUpdate()
        {
                   
        }     
    
    }

}
