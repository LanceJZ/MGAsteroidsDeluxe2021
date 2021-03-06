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
    public class Wedge : VectorModel
    {
        #region Fields
        int score = 200;
        float rotateMagnitude = MathHelper.PiOver2;
        Color color = new Color(175, 175, 255);
        bool alone = true;
        bool newWave = false;
        #endregion
        #region Properties
        public bool Alone { set => alone = value; }
        public bool Disperse { get => newWave; set => newWave = value; }
        public int Score { get => score; }
        #endregion
        #region Constructor
        public Wedge(Game game, Camera camera) : base(game, camera)
        {

        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();

            Enabled = false;
        }

        public new void LoadContent()
        {
            base.LoadContent();
            LoadVectorModel("WedgeShip", color);

        }

        public void BeginRun()
        {

        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (alone && !newWave)
            {
                PO.RotationVelocity.Z = RotateToChase(Position, Rotation.Z);
                Velocity = VelocityToChase(Rotation.Z);
            }
            else if (newWave)
            {
                if (PO.OffScreen())
                {
                    Enabled = false;
                }
            }
        }
        #endregion
        #region Public Methods
        public bool CheckCollision(Entity entity)
        {
            return CirclesIntersect(entity);
        }

        public float RotateToChase(Vector3 origin, float rotation)
        {
            return Core.AimAtTargetZ(origin, Main.instance.ThePlayer.Position,
                rotation, rotateMagnitude);
        }

        public Vector3 VelocityToChase(float rotation)
        {
            return Core.VelocityFromAngleZ(rotation, 10);
        }
        #endregion
        #region Private Methods
        #endregion
    }
}
