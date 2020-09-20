using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.CombatSystem;
using System;

namespace FloppyDiscProjectGreen
{
namespace Abilites
{
    public class BurnWiringAbility : AbilityBaseGenerics<BurnWiringAbility>
    {
        private const int DIAGONAL_MOVE_COST = 14;
        private const int STRAIGH_MOVE_COST = 10;
        private float _coolDown = 10;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private AudioClip _sound;  
        private int _level = 0;
        private int _damage = 20;
        private int _radious = 3;
        private int _radiousCost = 30;
        private int _actionPointCost = 20;
        private VisualiationHandler visualiationHandler;


        public override AbilityType type => AbilityType.active;

        public override string abilityName => "Burn Wiring";

        public override float coolDown { get => _coolDown; }

        public override Sprite sprite { get => _sprite; set {
            if(value != null)
                _sprite = value;
        } }

        public override AudioClip sound { get => _sound; set {
            if(value != null)
                _sound = value;
        } }

        public override int actionPointsCost { get => _actionPointCost; set {
            _actionPointCost = value;
        } }

        public override int level => _level;


        public override void Init(AbilitesSystem abSystem)
        {
            if(GridCombatSystem.debugS) _level = 1;
            visualiationHandler = VisualiationHandler.Instance;
            if(abSystem.gameObject.GetComponent<BurnWiringAbility>() == null)
                throw new AbilityAlreadyOnCharacter();
            abSystem.gameObject.AddComponent<BurnWiringAbility>();
        }

        public override void LevelUp()
        {
            _level++;
            _damage = _level * 20;
        }

        public override void TrigerAbility(GridObject target)
        {
            throw new NotImplementedException();
        }

        public override void VisualizeAbility(GridObject target)
        {
            throw new NotImplementedException();
        }

        public override void EndVisualization()
        {
            throw new NotImplementedException();
        }
    
        private int CalculateDistance(GridObject pn1, GridObject pn2)
        {
            int discX = Math.Abs(pn1.x() - pn2.x());
            int discY = Math.Abs(pn1.y() - pn2.y());
            int reamaing = Math.Abs(discX - discY);
            return DIAGONAL_MOVE_COST*Math.Min(discX, discY) + STRAIGH_MOVE_COST*reamaing;
        }
    }
}
}