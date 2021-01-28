﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Panther;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MGAsteroidsDeluxe2021.Entities
{
    class WedgePair : Entity
    {
        #region Fields
        Wedge[] wedges = new Wedge[2];
        int score = 100;
        bool alone = true;
        #endregion
        #region Properties
        public bool Alone { set => alone = value; }
        public Wedge[] TheWedges { get => wedges; }
        #endregion
        #region Constructor
        public WedgePair(Game game, Camera camera) : base(game, camera)
        {
            for (int i = 0; i < 2; i++)
            {
                wedges[i] = new Wedge(game, camera);
            }
        }
        #endregion
        #region Initialize-Load-Begin
        public override void Initialize()
        {
            base.Initialize();

            foreach (Wedge wedge in wedges)
            {
                wedge.Alone = false;
                wedge.PO.AddAsChildOf(PO, true, false, true, false);
            }

            wedges[1].PO.Rotation.Z = (float)Math.PI;
            wedges[1].PO.X = -0.65f;
            wedges[0].PO.X = 0.65f;
        }

        public new void LoadContent()
        {
            base.LoadContent();

            foreach (Wedge wedge in wedges)
            {
                wedge.LoadContent();
            }
        }

        public void BeginRun()
        {

        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (alone)
            {
                PO.RotationVelocity.Z = wedges[0].RotateToChase(Position, Rotation.Z);
                Velocity = wedges[0].VelocityToChase(Rotation.Z);
            }
        }
        #endregion
        #region Public Methods
        public bool CheckCollision(Entity entity)
        {
            bool hit = false;

            foreach (Wedge wedge in wedges)
            {
                if (wedge.CheckCollision(entity))
                {
                    hit = true;
                }
            }

            return hit;
        }

        public void Enable(bool enable)
        {
            foreach (Wedge wedge in wedges)
            {
                wedge.Enabled = enable;
            }
        }

        public void MakeVisable(bool visable)
        {
            foreach(Wedge wedge in wedges)
            {
                wedge.Visible = visable;
            }
        }
        #endregion
        #region Private Methods
        #endregion
    }
}
