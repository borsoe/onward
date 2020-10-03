﻿using System;
using System.Collections.Generic;
using Onward.Character.MonoBehaviours;
using Onward.Game.Enums;
using Onward.Game.ScriptableObjects;
using Onward.Grid.MonoBehaviours;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Onward.Game.MonoBehaviours
{
    public class GameManager : MonoBehaviour
    {
        #region setup

        private EncounterDesign _encounterDesign;
        private Champion.Factory _unitFactory;
        private GraphData _graphData;
        
        public bool isStateCombat;

        [Inject]
        public void Construct(EncounterDesign encounterDesign, GraphData graphData, Champion.Factory factory)
        {
            _encounterDesign = encounterDesign;
            _graphData = graphData;
            _unitFactory = factory;
        }

        #endregion

        #region methods

        public void ToggleGameState()
        {
            isStateCombat = !isStateCombat;
        }

        private void Init()
        {
            foreach (var encounterDesignEnemy in _encounterDesign.enemies)
            {
                var enemy = _unitFactory.Create(encounterDesignEnemy.Value);
                _graphData[encounterDesignEnemy.Key].OccupyingEntity = enemy;
            }
        }

        #endregion

        private void Awake()
        {
            Init();
        }
    }
}