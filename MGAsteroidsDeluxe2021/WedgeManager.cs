using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Panther;
using System;
using System.Collections.Generic;
using System.Linq;
using MGAsteroidsDeluxe2021.Entities;

namespace MGAsteroidsDeluxe2021
{
    public class WedgeManager : GameComponent
    {
        #region Fields
        Timer spawnTimer;
        Wedge[] wedges = new Wedge[6];
        WedgePair[] wedgePairs = new WedgePair[3];
        WedgeGroup wedgeGroup;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public WedgeManager(Game game, Camera camera) : base(game)
        {
            spawnTimer = new Timer(game);

            for (int i = 0; i < 6; i++)
            {
                wedges[i] = new Wedge(game, camera);
            }

            for (int i = 0; i < 3; i++)
            {
                wedgePairs[i] = new WedgePair(game, camera);
            }

            wedgeGroup = new WedgeGroup(game, camera);

            game.Components.Add(this);
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();
            

        }

        public void LoadContent()
        {
            foreach(Wedge wedge in wedges)
            {
                wedge.LoadContent();
            }

            foreach(WedgePair wedgePair in wedgePairs)
            {
                wedgePair.LoadContent();
            }

            wedgeGroup.LoadContent();
        }

        public void BeginRun()
        {

            wedgeGroup.BeginRun();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        #endregion
        #region Public Methods
        public void MakeVisable(bool visable)
        {
            foreach (Wedge wedge in wedges)
            {
                wedge.Visible = false;
            }

            foreach (WedgePair wedgePair in wedgePairs)
            { 
                wedgePair.MakeVisable(visable);
            }

            wedgeGroup.MakeVisable(visable);
        }
        #endregion
        #region Private Methods
        void CheckGroupCollision()
        {
            foreach (Shot shot in Main.instance.ThePlayer.Shots)
            {
                if(wedgeGroup.CheckCollision(shot))
                {
                    for(int i = 0; i < 3; i ++)
                    {

                    }
                }
            }
        }
        #endregion
    }
}
