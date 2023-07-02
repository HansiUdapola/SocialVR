using Ubiq.Avatars;
using UnityEngine;

namespace Ubiq.FloatingAvatar{

//This script manage how float animal Avatars copy of FloatAvatar.
[RequireComponent(typeof(Avatars.Avatar))]
[RequireComponent(typeof(ThreePointTrackedAvatar))]

    public class FloatingAnimalAvatar : MonoBehaviour
    {
        //Seven Animal body objects to refer avatar body (No arms)
        public Transform head;
        public Transform torso;
        public Transform leftHand;
        public Transform rightHand;
        public Transform ears;
        public Transform tail;
        public Transform mouth;

        //Renderer objects for above 7 body parts of avatar
        public Renderer headRenderer;
        public Renderer torsoRenderer;
        public Renderer leftHandRenderer;
        public Renderer rightHandRenderer;
        public Renderer earsRenderer;
        public Renderer tailRenderer;
        public Renderer mouthRenderer;

        //Offset gameobjects
        public Transform baseOfNeckOffset;
        public Transform earsOffset;
        public Transform tailOffset;
        public Transform mouthOffset;


        //Body Movement Animation
        public AnimationCurve torsoFootCurve;
        public AnimationCurve torsoFacingCurve;

        private Vector3 footPosition;
        private Quaternion torsoFacing;

        //To catch Avatar
        private Avatars.Avatar avatar;

        //Track VR HMD, Left,Right Hands
        private ThreePointTrackedAvatar trackedAvatar;

        public TexuredAvatar texturedAvatar;

        private void Awake()
        {
            avatar = GetComponent<Avatars.Avatar>();
            trackedAvatar = GetComponent<ThreePointTrackedAvatar>();
        }
        private void OnEnable()
        {
            trackedAvatar.OnHeadUpdate.AddListener(ThreePointTrackedAvatar_OnHeadUpdate);
            trackedAvatar.OnLeftHandUpdate.AddListener(ThreePointTrackedAvatar_OnLeftHandUpdate);
            trackedAvatar.OnRightHandUpdate.AddListener(ThreePointTrackedAvatar_OnRightHandUpdate);

            if (texturedAvatar)
            {
                texturedAvatar.OnTextureChanged.AddListener(TexturedAvatar_OnTextureChanged);
            }  
        }

        private void TexturedAvatar_OnTextureChanged(Texture2D tex)
        {
            headRenderer.material.mainTexture = tex;
            torsoRenderer.material = headRenderer.material;
            leftHandRenderer.material = headRenderer.material;
            rightHandRenderer.material = headRenderer.material;
            earsRenderer.material = headRenderer.material;
            tailRenderer.material = headRenderer.material;
            mouthRenderer.material = headRenderer.material;
        }

        private void OnDisable()
        {
            if (trackedAvatar && trackedAvatar != null)
            {
                trackedAvatar.OnHeadUpdate.RemoveListener(ThreePointTrackedAvatar_OnHeadUpdate);
                trackedAvatar.OnLeftHandUpdate.RemoveListener(ThreePointTrackedAvatar_OnLeftHandUpdate);
                trackedAvatar.OnRightHandUpdate.RemoveListener(ThreePointTrackedAvatar_OnRightHandUpdate);
            }

            if (texturedAvatar && texturedAvatar != null)
            {
                texturedAvatar.OnTextureChanged.RemoveListener(TexturedAvatar_OnTextureChanged);
            }
        }

        private void ThreePointTrackedAvatar_OnHeadUpdate(Vector3 pos, Quaternion rot)
        {
            head.position = pos;
            head.rotation = rot;
            UpdateTorso();
        }

        private void ThreePointTrackedAvatar_OnLeftHandUpdate(Vector3 pos, Quaternion rot)
        {
            leftHand.position = pos;
            leftHand.rotation = rot;
        }

        private void ThreePointTrackedAvatar_OnRightHandUpdate(Vector3 pos, Quaternion rot)
        {
            rightHand.position = pos;
            rightHand.rotation = rot;
        }

        private void Update()
        {
            
            UpdateAnimalBody();
        }

        private void UpdateTorso()
        {
            // Animate through coding to add movement the body(Torso) related to 
            //HMD (head)

            // Update virtual 'foot' position, just for animation, wildly inaccurate :)
            var neckPosition = baseOfNeckOffset.position;
            footPosition.x += (neckPosition.x - footPosition.x) * Time.deltaTime * torsoFootCurve.Evaluate(Mathf.Abs(neckPosition.x - footPosition.x));
            footPosition.z += (neckPosition.z - footPosition.z) * Time.deltaTime * torsoFootCurve.Evaluate(Mathf.Abs(neckPosition.z - footPosition.z));
            footPosition.y = 0;

            // Forward direction of torso is vector in the transverse plane
            // Determined by head direction primarily, hint provided by hands
            var torsoRotation = Quaternion.identity;

            // Head: Just use head direction
            var headFwd = head.forward;
            headFwd.y = 0;

            var rot = Quaternion.LookRotation(headFwd, Vector3.up);
            var angle = Quaternion.Angle(torsoFacing, rot);
            var rotateAngle = Mathf.Clamp(Time.deltaTime * torsoFacingCurve.Evaluate(Mathf.Abs(angle)), 0, angle);
            torsoFacing = Quaternion.RotateTowards(torsoFacing, rot, rotateAngle);

            // Place torso so it makes a straight line between neck and feet
            torso.position = neckPosition;
            torso.rotation = Quaternion.FromToRotation(Vector3.down, footPosition - neckPosition) * torsoFacing;
        }
    
        private void UpdateAnimalBody(){
            var headTopPosition = earsOffset.position;
            var TorsoBackPosition = tailOffset.position;
            var MouthPosition = mouthOffset.position;

            //ears
            ears.position = headTopPosition;
            ears.rotation = head.rotation;

            //tail
            tail.position = TorsoBackPosition;
            tail.rotation = Quaternion.FromToRotation(Vector3.down, footPosition - TorsoBackPosition) * torsoFacing;

            //mouth
            //mouth.position = MouthPosition;
            mouth.position = MouthPosition;
            mouth.rotation = head.rotation;
            //mouth.rotation = Quaternion.FromToRotation(Vector3.down, footPosition - MouthPosition) * torsoFacing;
        }
        
    }

}
