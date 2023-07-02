using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.XR;
using Ubiq.Messaging;

public class NetGrabItem : MonoBehaviour, IGraspable, INetworkComponent, INetworkObject, IUseable
{
    Hand grasped;
    Hand used;
    public NetworkId Id => new NetworkId();
    private NetworkContext context;

    struct Message{
        public Vector3 position;
    }

    void Start()
    {
        context = NetworkScene.Register(this);
    }

    public void Grasp(Hand controller)
    {
        grasped = controller;
    }

    public void Release(Hand controller)
    {
        grasped = null;
    }

    void INetworkComponent.ProcessMessage(ReferenceCountedSceneGraphMessage message){
        var msg = message.FromJson<Message>();
        transform.localPosition = msg.position;
    }

    void Update()
    {
        if(grasped){
            transform.localPosition = grasped.transform.position;
            Message message;
            message.position = transform.localPosition;
            context.SendJson(message);
        }

        if(used){
            transform.localPosition = used.transform.position;
            Message message;
            message.position = transform.localPosition;
            context.SendJson(message);
        }
    }

    public void Use(Hand controller)
    {
        used = controller;
    }

    public void UnUse(Hand controller)
    {
        used = null;
    }
}
