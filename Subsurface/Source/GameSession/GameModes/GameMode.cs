﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Subsurface
{
    class GameMode
    {
        public static List<GameModePreset> PresetList = new List<GameModePreset>();

        TimeSpan duration;
        protected DateTime startTime;
        protected DateTime endTime;

        //public readonly bool IsSinglePlayer;

        private GUIProgressBar timerBar;
        
        protected bool isRunning;

        //protected string name;

        protected GameModePreset preset;

        private string endMessage;

        public virtual Quest Quest
        {
            get { return null; }
        }

        public DateTime StartTime
        {
            get { return startTime; }
        }

        public DateTime EndTime
        {
            get { return endTime; }
        }

        public bool IsRunning
        {
            get { return isRunning; }
        }

        public bool IsSinglePlayer
        {
            get { return preset.IsSinglePlayer; }
        }

        public string Name
        {
            get { return preset.Name; }
        }

        public string EndMessage
        {
            get { return endMessage; }
        }

        public GameMode(GameModePreset preset)
        {
            this.preset = preset;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (timerBar != null) timerBar.Draw(spriteBatch);
        }

        public virtual void Start(TimeSpan duration)
        {
            startTime = DateTime.Now;
            if (duration!=TimeSpan.Zero)
            {
                endTime = startTime + duration;
                this.duration = duration;

                timerBar = new GUIProgressBar(new Rectangle(Game1.GraphicsWidth - 120, 20, 100, 25), Color.Gold, 0.0f, null);  
            }

            endMessage = "The round has ended!";

            isRunning = true;
        }

        public virtual void Update(float deltaTime)
        {
            if (!isRunning) return;

            if (duration != TimeSpan.Zero)
            {
                double elapsedTime = (DateTime.Now - startTime).TotalSeconds;
                timerBar.BarSize = (float)(elapsedTime / duration.TotalSeconds);
            }
            //if (DateTime.Now >= endTime)
            //{
            //    End(endMessage);                
            //}
        }

        public virtual void End(string endMessage = "")
        {
            isRunning = false;

            if (endMessage != "" || this.endMessage == null) this.endMessage = endMessage;

            Game1.GameSession.EndShift(endMessage);
        }
        
        public static void Init()
        {
            new GameModePreset("Single Player", typeof(SinglePlayerMode), true);
            

            var mode = new GameModePreset("SandBox", typeof(GameMode), false);
            mode.Description = "A game mode with no specific objectives.";

            mode = new GameModePreset("Traitor", typeof(TraitorMode), false);
            mode.Description = "One of the players is selected as a traitor and given a secret objective. "
                + "The rest of the crew will win if they reach the end of the level or kill the traitor "
                + "before the objective is completed.";

            mode = new GameModePreset("Quest", typeof(QuestMode), false);
            mode.Description = "The crew must work together to complete a specific task, such as retrieving "
                + "an alien artifact or killing a creature that's terrorizing nearby outposts. The game ends "
                + "when the task is completed or everyone in the crew has died.";


        }
    }
}