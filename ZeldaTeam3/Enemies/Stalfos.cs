﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zelda.Enemies
{
    public class Stalfos : IEnemy
    {
        private StalfosStateMachine _stateMachine;

        public Stalfos(int posX, int posY)
        {
            _stateMachine = new StalfosStateMachine(this, posX, posY);
            
        }

        public void FaceDown()
        {
            return;
        }

        public void FaceLeft()
        {
            return;
        }

        public void FaceRight()
        {
            return;
        }

        public void FaceUp()
        {
            return;
        }

        public void Idle()
        {
            return;
        }

        public void Kill()
        {
            _stateMachine.Kill();
        }

        public void MoveDown()
        {
            _stateMachine.MoveDown();
        }

        public void MoveLeft()
        {
            _stateMachine.MoveLeft();
        }

        public void MoveRight()
        {
            _stateMachine.MoveRight();
        }

        public void MoveUp()
        {
            _stateMachine.MoveUp();
        }

        public void Spawn()
        {
            _stateMachine.Spawn();
        }

        public void TakeDamage()
        {
            _stateMachine.TakeDamage();
        }

        public void UseAttack()
        {
            _stateMachine.UseAttack();
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
