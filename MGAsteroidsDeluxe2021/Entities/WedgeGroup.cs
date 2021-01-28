using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Panther;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MGAsteroidsDeluxe2021.Entities
{
    class WedgeGroup : Entity
    {
        #region Fields
        WedgePair[] wedgePairs = new WedgePair[3];
        int score = 50;
        #endregion
        #region Properties
        public WedgePair[] TheWedgePairs { get => wedgePairs; }
        #endregion
        #region Constructor
        public WedgeGroup(Game game, Camera camera) : base(game, camera)
        {
            for (int i = 0; i < 3; i++)
            {
                wedgePairs[i] = new WedgePair(game, camera);
            }
        }
        #endregion
        #region Initialize-Load-Begin
        public override void Initialize()
        {
            base.Initialize();

            foreach(WedgePair wedgePair in wedgePairs)
            {
                wedgePair.PO.AddAsChildOf(PO);
                wedgePair.Alone = false;
            }

            float wY = 0.56f;
            float wX = 0.65f;
            float rot = 0.333f;
            wedgePairs[0].PO.Position.Y = wY;
            wedgePairs[1].PO.Position = new Vector3(wX, -wY, 0);
            wedgePairs[2].PO.Position = new Vector3(-wX, -wY, 0);
            wedgePairs[1].PO.Rotation.Z = (float)Math.PI * rot;
            wedgePairs[2].PO.Rotation.Z = (float)-Math.PI * rot;
        }

        public new void LoadContent()
        {
            base.LoadContent();

            foreach(WedgePair wedgePair in wedgePairs)
            {
                wedgePair.LoadContent();
            }
        }

        public void BeginRun()
        {
            Velocity = Core.RandomVelocity(3);
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Position = Core.WrapSideToSide(Core.WrapTopBottom(Position, Core.ScreenHeight), Core.ScreenWidth);
        }
        #endregion
        #region Public Methods
        public bool CheckCollision(Entity entity)
        {
            bool hit = false;

            foreach (WedgePair wedgePair in wedgePairs)
            {
                if (wedgePair.CheckCollision(entity))
                {
                    hit = true;
                }
            }

            return hit;
        }

        public void Enable(bool enable)
        {
            foreach (WedgePair wedgePair in wedgePairs)
            {
                wedgePair.Enable(enable);
            }
        }

        public void MakeVisable(bool visable)
        {
            foreach(WedgePair wedgePair in wedgePairs)
            {
                wedgePair.MakeVisable(visable);
            }
        }
        #endregion
        #region Private Methods
        #endregion
    }
}
