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
        bool pairsGone = false;
        bool groupGone = false;
        bool ready = false;
        bool start = false;
        #endregion
        #region Properties
        public WedgeGroup TheWedgeGroup { get => wedgeGroup; }
        public bool Ready { get => ready; set => ready = value; }
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

            spawnTimer.Amount = 10;
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
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ready)
            {
                if (spawnTimer.Elapsed)
                {
                    SpawnGroup();
                    ready = false;
                }
            }
            else
            {
                CheckGroupCollision();
                CheckPairsCollision();
                CheckWedgesCollision();
            }
        }
        #endregion
        #region Public Methods
        public void Disperse()
        {
            wedgeGroup.Disperse = true;
            start = false;

            foreach(WedgePair wedgePair in wedgePairs)
            {
                wedgePair.Disperse = true;
                wedgePair.PO.RotationVelocity.Z = 0;
            }

            foreach (Wedge wedge in wedges)
            {
                wedge.Disperse = true;
                wedge.PO.RotationVelocity.Z = 0;
            }
        }

        public void Start()
        {
            if (!start)
            {
                ready = true;
                start = true;
                spawnTimer.Reset();

                wedgeGroup.Disperse = false;

                foreach (WedgePair wedgePair in wedgePairs)
                {
                    wedgePair.Disperse = false;

                    foreach (Wedge wedge in wedgePair.TheWedges)
                    {
                        wedge.Disperse = false;
                    }
                }
            }
        }

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
        void SpawnGroup()
        {
            wedgeGroup.BeginRun();
            wedgeGroup.Enable(true);
            ready = false;
        }

        void CheckGroupCollision()
        {
            if (wedgeGroup.Enabled)
            {
                foreach (Shot shot in Main.instance.ThePlayer.Shots)
                {
                    if (shot.Enabled)
                    {
                        if (wedgeGroup.CheckCollision(shot))
                        {
                            shot.Enabled = false;
                            GroupHit();
                            Main.instance.PlayerScore((uint)wedgeGroup.Score);
                            return;
                        }
                    }
                }

               if (Main.instance.TheUFOManager.TheUFO.Shot.Enabled)
                {
                    if (wedgeGroup.CheckCollision(Main.instance.TheUFOManager.TheUFO.Shot))
                    {
                        Main.instance.TheUFOManager.TheUFO.Shot.Enabled = false;
                        GroupHit();
                        return;
                    }
                }

               if (Main.instance.ThePlayer.Enabled)
                {
                    if (wedgeGroup.CheckCollision(Main.instance.ThePlayer))
                    {
                        if (Main.instance.ThePlayer.ShieldOn)
                        {
                            Main.instance.ThePlayer.ShieldHit(wedgeGroup);
                        }
                        else
                        {
                            GroupHit();
                            Main.instance.PlayerHit();
                            Main.instance.PlayerScore((uint)wedgeGroup.Score);
                        }
                    }
                }
            }
        }

        void GroupHit()
        {
            SpawnPairs();
            wedgeGroup.Enable(false);
            groupGone = true;
        }

        void CheckPairsCollision()
        {
            bool allGone = false;

            if (groupGone)
                allGone = true;

            foreach (WedgePair wedgePair in wedgePairs)
            {
                if (wedgePair.Enabled)
                {
                    allGone = false;

                    foreach (Shot shot in Main.instance.ThePlayer.Shots)
                    {
                        if (shot.Enabled)
                        {
                            if (wedgePair.CheckCollision(shot))
                            {
                                WedgePairHit(wedgePair, shot);
                                Main.instance.PlayerScore((uint)wedgePair.Score);
                                return;
                            }
                        }
                    }

                    if (Main.instance.TheUFOManager.TheUFO.Shot.Enabled)
                    {
                        if(wedgePair.CheckCollision(Main.instance.TheUFOManager.TheUFO.Shot))
                        {
                            WedgePairHit(wedgePair, Main.instance.TheUFOManager.TheUFO.Shot);
                            return;
                        }
                    }

                    if (Main.instance.ThePlayer.Enabled)
                    {
                        if (wedgePair.CheckCollision(Main.instance.ThePlayer))
                        {
                            if (Main.instance.ThePlayer.ShieldOn)
                            {
                                Main.instance.ThePlayer.ShieldHit(wedgePair);
                                return;
                            }
                            else
                            {
                                WedgePairHit(wedgePair);
                                Main.instance.PlayerScore((uint)wedgePair.Score);
                                Main.instance.PlayerHit();
                                return;
                            }
                        }
                    }
                }
            }

            if (allGone)
                pairsGone = true;
        }

        void WedgePairHit(WedgePair wedgePair, Shot shot)
        {
            WedgePairHit(wedgePair);
            shot.Enabled = false;
        }

        void WedgePairHit(WedgePair wedgePair)
        {
            SpawnWedges(wedgePair);
            wedgePair.Enable(false);
        }

        void CheckWedgesCollision()
        {
            bool allGone = false;

            if(pairsGone)
                allGone = true;

            foreach(Wedge wedge in wedges)
            {
                if (wedge.Enabled)
                {
                    allGone = false;

                    foreach (Shot shot in Main.instance.ThePlayer.Shots)
                    {
                        if (shot.Enabled)
                        {
                            if (wedge.CheckCollision(shot))
                            {
                                WedgeHit(shot, wedge);
                                return;
                            }
                        }
                    }

                    if (Main.instance.TheUFOManager.TheUFO.Shot.Enabled)
                    {
                        if (wedge.CheckCollision(Main.instance.TheUFOManager.TheUFO.Shot))
                        {
                            WedgeHit(Main.instance.TheUFOManager.TheUFO.Shot, wedge);
                            return;
                        }
                    }

                    if (Main.instance.ThePlayer.Enabled)
                    {
                        if (wedge.CheckCollision(Main.instance.ThePlayer))
                        {
                            if (Main.instance.ThePlayer.ShieldOn)
                            {
                                Main.instance.ThePlayer.ShieldHit(wedge);
                            }
                            else
                            {
                                WedgeHit(wedge);
                                Main.instance.PlayerScore((uint)wedge.Score);
                                Main.instance.PlayerHit();
                            }
                        }
                    }
                }
            }

            if (allGone)
            {
                pairsGone = false;
                groupGone = false;
                ready = true;
                spawnTimer.Reset();
            }
        }

        void WedgeHit(Shot shot, Wedge wedge)
        {
            shot.Enabled = false;
            WedgeHit(wedge);
        }

        void WedgeHit(Wedge wedge)
        {
            wedge.Enabled = false;
        }

        void SpawnPairs()
        {
            for (int i = 0; i < 3; i++)
            {
                wedgePairs[i].Position = wedgeGroup.TheWedgePairs[i].WorldPosition;
                wedgePairs[i].Rotation = wedgeGroup.TheWedgePairs[i].WorldRotation;
                wedgePairs[i].Alone = true;
                wedgePairs[i].Disperse = false;
                wedgePairs[i].Enable(true);

                foreach(Wedge wedge in wedgePairs[i].TheWedges)
                {
                    wedge.UpdateMatrix();
                }
            }
        }

        void SpawnWedges(WedgePair pair)
        {
            for (int i = 0; i < 6; i++)
            {
                if (!wedges[i].Enabled)
                {
                    wedges[i].Position = pair.TheWedges[0].WorldPosition;
                    wedges[i + 1].Position = pair.TheWedges[1].WorldPosition;
                    wedges[i].Rotation = pair.TheWedges[0].WorldRotation;
                    wedges[i + 1].Rotation = pair.TheWedges[1].WorldRotation;
                    wedges[i].Alone = true;
                    wedges[i + 1].Alone = true;
                    wedges[i].Disperse = false;
                    wedges[i + 1].Disperse = false;
                    wedges[i].Enabled = true;
                    wedges[i + 1].Enabled = true;
                    wedges[i].UpdateMatrix();
                    wedges[i + 1].UpdateMatrix();
                    wedges[i].Velocity = wedges[i].VelocityToChase(wedges[i].Rotation.Z);
                    wedges[i + 1].Velocity = wedges[i + 1].VelocityToChase(wedges[i + 1].Rotation.Z);
                    break;
                }
            }
        }
        #endregion
    }
}
