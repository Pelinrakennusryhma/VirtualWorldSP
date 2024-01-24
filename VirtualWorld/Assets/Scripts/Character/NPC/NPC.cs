using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace Characters
{
    public class NPC : NetworkBehaviour
    {
        [field: SerializeField] public NPCData Data { get; private set; }
        [SerializeField] TMP_Text nameplate;
        [SerializeField] TMP_Text titleplate;

        [HideInInspector]
        [SyncVar]
        private string npcName;
        [HideInInspector]
        [SyncVar]
        private string npcTitle;

        public override void OnStartServer()
        {
            base.OnStartServer();

            InitData();

            SetNameplates();
        }

        void InitData()
        {
            npcName = Data.fullName;
            npcTitle = Data.title;
        }

        void SetNameplates()
        {
            nameplate.text = npcName;
            titleplate.text = npcTitle;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            SetNameplates();
        }
    }
}

