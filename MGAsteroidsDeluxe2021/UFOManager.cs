﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using Panther;
using MGAsteroidsDeluxe2021.Entities;

namespace MGAsteroidsDeluxe2021
{
    public class UFOManager : GameComponent
    {
        #region Fields
        Camera cameraRef;
        UFO theUFO;
        Timer spawnTimer;
        uint spawnCount;
        #endregion
        #region Properties
        public UFO TheUFO { get => theUFO; }
        public Vector3[] DotVerts { set => theUFO.DotVerts = value; }
        #endregion
        #region Constructor
        public UFOManager(Game game, Camera camera) : base(game)
        {
            cameraRef = camera;
            theUFO = new UFO(game, camera);
            spawnTimer = new Timer(game);

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
            theUFO.LoadContent();
        }

        public void BeginRun()
        {
            ResetTimer();
            theUFO.BeginRun();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (spawnTimer.Elapsed)
            {
                ResetTimer();

                if (!theUFO.Enabled && Main.instance.CurrentMode != GameState.PlayerHit)
                {
                    Spawn();
                }
            }
        }
        #endregion
        #region Public Methods
        public void ResetTimer()
        {
            spawnTimer.Reset(Core.RandomMinMax(10 - Main.instance.Wave * 0.1f,
                10.15f + Main.instance.Wave * 0.1f));
        }
        
        public void Reset()
        {
            theUFO.Enabled = false;
            spawnCount = 0;
            ResetTimer();
        }

        public void MakeVisable(bool visable)
        {
            TheUFO.Visible = visable;
            TheUFO.Shot.Visible = visable;
            TheUFO.explodeFX = visable;

            if (!visable)
                TheUFO.HideExplode();
        }
        #endregion
        #region Private Methods
        void Spawn()
        {
            float spawnPercent = (float)(Math.Pow(0.915, spawnCount / (Main.instance.Wave + 1)) * 100);
            Vector3 position = new Vector3();

            if (Core.RandomMinMax(0, 99) < spawnPercent - Main.instance.Score / 400)
            {
                theUFO.type = GameLogic.UFOType.Large;
                theUFO.Scale = 1;
            }
            else
            {
                theUFO.type = GameLogic.UFOType.Small;
                theUFO.Scale = 0.5f;
            }

            position.Y = Core.RandomMinMax(-Core.ScreenHeight * 0.25f, Core.ScreenHeight * 0.25f);

            if (Core.RandomMinMax(1, 10) > 5)
            {
                position.X = -Core.ScreenWidth;
            }
            else
            {
                position.X = Core.ScreenWidth;
            }

            theUFO.Spawn(position);
        }
        #endregion
    }
}
