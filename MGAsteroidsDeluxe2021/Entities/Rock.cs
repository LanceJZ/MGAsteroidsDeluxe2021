using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using Panther;

namespace MGAsteroidsDeluxe2021.Entities
{
    public class Rock : VectorModel
    {
        #region Fields
        Explode explosion;
        public GameLogic.RockSize size = new GameLogic.RockSize();
        public float baseRadius;
        public bool explodeFX = true;
        #endregion
        #region Properties
        public float Radius { get => PO.Radius; set => PO.Radius = value; }
        public Vector3[] DotVerts { set => explosion.DotVerts = value; }
        #endregion
        #region Constructor
        public Rock(Game game, Camera camera) : base(game, camera)
        {
            explosion = new Explode(game, camera);
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();

        }

        public new void LoadContent()
        {
            base.LoadContent();

        }

        public void BeginRun()
        {
            explosion.AddAsChildOf(PO, false, true);
            explosion.Color = new Color(100, 100, 166);
            explosion.Speed = 4.666f;
            explosion.Maxlife = 0.8f;
            explosion.Minlife = 0.25f;

            PO.RotationVelocity.Z = Core.RandomMinMax(-3.33f, 3.33f);
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Enabled)
            {
                Position = Core.WrapSideToSide(Core.WrapTopBottom(Position, Core.ScreenHeight), Core.ScreenWidth);
                CheckCollusion();
            }
        }
        #endregion
        #region Public Methods
        public void HideExplode()
        {
            explosion.Hide();
        }
        #endregion
        #region Private Methods
        void CheckCollusion()
        {
            Player player = Main.instance.ThePlayer;
            UFO ufo = Main.instance.TheUFO.TheUFO;

            foreach (Shot shot in player.Shots)
            {
                if (PO.CirclesIntersect(shot.PO))
                {
                    shot.Enabled = false;
                    Explode();
                    PlayerScored();
                }
            }

            if (!player.ShieldOn)
            {
                if (PO.CirclesIntersect(player.PO))
                {
                    Explode();
                    PlayerScored();
                    Main.instance.PlayerHit();
                }
            }
            else
            {
                if (PO.CirclesIntersect(player.PO))
                {
                    player.ShieldHit(Position, Velocity);
                }
            }

            if (PO.CirclesIntersect(ufo.PO))
            {
                ufo.Explode();
                Explode();
            }

            if (PO.CirclesIntersect(ufo.Shot.PO))
            {
                ufo.Shot.Enabled = false;
                Explode();
            }
        }

        void PlayerScored()
        {
            uint points = 0;

            switch(size)
            {
                case GameLogic.RockSize.Large:
                    points = 20;
                    break;
                case GameLogic.RockSize.Medium:
                    points = 50;
                    break;
                case GameLogic.RockSize.Small:
                    points = 100;
                    break;
            }

            Main.instance.PlayerScore(points);
        }

        void Explode()
        {
            Enabled = false;
            Moveable = false;
            Main.instance.TheRocks.RockDistroyed(this);

            if (explodeFX)
            {
                explosion.Spawn(Core.RandomMinMax(20, 40));
            }
        }
        #endregion
    }
}
