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
        Camera cameraRef;
        Timer spawnTimer;
        Wedge wedge;
        WedgePair wedgePair;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public WedgeManager(Game game, Camera camera) : base(game)
        {
            cameraRef = camera;
            spawnTimer = new Timer(game);
            wedge = new Wedge(game, camera);
            wedgePair = new WedgePair(game, camera);
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
            wedge.LoadContent();
            wedgePair.LoadContent();
        }

        public void BeginRun()
        {
            wedge.X = -30;
            wedge.Y = -30;
            wedgePair.X = 30;
            wedgePair.Y = 30;
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        #endregion
        #region Public Methods
        #endregion
        #region Private Methods
        #endregion
    }
}
