using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace TVMario
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public enum GameState { MainMenu, NewGame, Exit };

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MyForm _form;
        private GameState _gameState;

        public GameState gameState
        {
            get { return _gameState; }
            set { _gameState = value; }
        }
        private GameState _prevState;

        Song _menuSong;
        Song _backgroundSong;
        SoundEffect _selectSound;
        private SoundEffect _jumpSound;

        public SoundEffect jumpSound
        {
            get { return _jumpSound; }
            set { _jumpSound = value; }
        }
        private SoundEffect _dieSound;

        public SoundEffect dieSound
        {
            get { return _dieSound; }
            set { _dieSound = value; }
        }
        private SoundEffect _coinSound;

        public SoundEffect coinSound
        {
            get { return _coinSound; }
            set { _coinSound = value; }
        }
        private SoundEffect _cutSound;

        public SoundEffect cutSound
        {
            get { return _cutSound; }
            set { _cutSound = value; }
        }
        private SoundEffect _fireBallSound;

        public SoundEffect fireBallSound
        {
            get { return _fireBallSound; }
            set { _fireBallSound = value; }
        }
        private SoundEffect _boltSound;

        public SoundEffect boltSound
        {
            get { return _boltSound; }
            set { _boltSound = value; }
        }
        private SoundEffect _growSound;

        public SoundEffect growSound
        {
            get { return _growSound; }
            set { _growSound = value; }
        }
        private SoundEffect _getSkillSound;

        public SoundEffect getSkillSound
        {
            get { return _getSkillSound; }
            set { _getSkillSound = value; }
        }


        Stage _stage;

        KeyboardState _kbState;

        int iDelay = 0;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 672;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _gameState = GameState.MainMenu;
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _form = new MyForm("Forms/MainMenu.dat", Content);
            _stage = new Stage();


            _menuSong = Content.Load<Song>(@"Audios/menu");
            _backgroundSong = Content.Load<Song>(@"Audios/background");
            _selectSound = Content.Load<SoundEffect>(@"Audios/select");
            _jumpSound = Content.Load<SoundEffect>(@"Audios/jump");
            _dieSound = Content.Load<SoundEffect>(@"Audios/die");
            _coinSound = Content.Load<SoundEffect>(@"Audios/coin");
            _cutSound = Content.Load<SoundEffect>(@"Audios/cut");
            _fireBallSound = Content.Load<SoundEffect>(@"Audios/fireball");
            _boltSound = Content.Load<SoundEffect>(@"Audios/bolt");
            _growSound = Content.Load<SoundEffect>(@"Audios/grow");
            _getSkillSound = Content.Load<SoundEffect>(@"Audios/getskill");
            _stage.Init(Content, "Data/Stages/Stage01.dat", this);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        bool loadMenuDone = false;    // Cho biết đã load menu xong chưa
        int _selectedButton = 0;      // Cho biết Button nào đang được chọn
        public bool songPlayed = false;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            switch (_gameState)
            {
                case GameState.MainMenu:
                    if (!songPlayed)
                    {
                        MediaPlayer.Play(_menuSong);
                        MediaPlayer.IsRepeating = true;
                        songPlayed = true;
                    }
                    if (!loadMenuDone)
                    {
                        if (_form.Buttons[0].TopLeft.X > 100)
                        {
                            Vector2 temp = _form.Buttons[0].TopLeft;
                            temp.X -= 15;
                            _form.Buttons[0].TopLeft = temp;
                        }
                        else
                        {

                            _form.Buttons[0].Color = Color.Wheat;
                            if (_form.Buttons[1].TopLeft.X > 100)
                            {
                                Vector2 temp = _form.Buttons[1].TopLeft;
                                temp.X -= 15;
                                _form.Buttons[1].TopLeft = temp;
                            }
                            else
                            {

                                _form.Buttons[1].Color = Color.Wheat;
                                if (_form.Buttons[2].TopLeft.X > 100)
                                {
                                    Vector2 temp = _form.Buttons[2].TopLeft;
                                    temp.X -= 15;
                                    _form.Buttons[2].TopLeft = temp;
                                }
                                else
                                {

                                    _form.Buttons[2].Color = Color.Wheat;
                                    if (_form.Buttons[3].TopLeft.X > 100)
                                    {
                                        Vector2 temp = _form.Buttons[3].TopLeft;
                                        temp.X -= 15;
                                        _form.Buttons[3].TopLeft = temp;
                                    }
                                    else
                                    {

                                        _form.Buttons[3].Color = Color.Wheat;
                                        if (_form.Buttons[4].TopLeft.X > 100)
                                        {
                                            Vector2 temp = _form.Buttons[4].TopLeft;
                                            temp.X -= 15;
                                            _form.Buttons[4].TopLeft = temp;
                                        }
                                        else
                                        {

                                            _form.Buttons[4].Color = Color.Wheat;
                                            loadMenuDone = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        iDelay++; // Tang delay de tranh truong hop su kien press key qua nhanh
                        if (iDelay % 8 == 0) // 8 la dep roi. Them nua thi su kien press hoi cham.
                        {
                            iDelay = 0;
                            _kbState = Keyboard.GetState();
                            if (_kbState.IsKeyDown(Keys.Up))
                            {
                                if (_selectedButton > 0)
                                {
                                    _selectedButton--;
                                    _selectSound.Play();
                                }
                            }
                            else
                            {
                                if (_kbState.IsKeyDown(Keys.Down))
                                    if (_selectedButton < 4)
                                    {
                                        _selectSound.Play();
                                        _selectedButton++;
                                    }
                            }

                            // Bat su kien enter button
                            if (_kbState.IsKeyDown(Keys.Enter))
                            {
                                if (_selectedButton == 4) // Exit game
                                    this.Exit();
                                else if (_selectedButton == 2) // Setting menu
                                {
                                    //this.Exit();
                                }
                                else if (_selectedButton == 0) // New Game
                                {
                                    _gameState = GameState.NewGame;
                                    songPlayed = false;
                                    MediaPlayer.Stop();
                                }
                            }

                            _form.Buttons[_selectedButton].Color = Color.Tomato;
                            for (int i = 0; i < _form.Buttons.Count; i++)
                            {
                                if (i != _selectedButton)
                                    _form.Buttons[i].Color = Color.Wheat;
                            }
                        }
                    }
                    _form.Update(gameTime);
                    break;
                case GameState.NewGame:
                    if (!songPlayed)
                    {

                        MediaPlayer.Play(_backgroundSong);
                        MediaPlayer.IsRepeating = true;
                        songPlayed = true;
                    }
                    if (!_stage.isPlaying)
                    {
                        _stage.Init(Content, "Data/Stages/Stage01.dat", this);
                    }
                    else
                        _stage.Update(gameTime);

                    break;
                case GameState.Exit:
                    this.Exit();
                    break;
            }

            //base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (_gameState == GameState.MainMenu)
            {
                _form.Draw(spriteBatch, gameTime, Color.White);
            }
            else if (_gameState == GameState.NewGame)
            {
                _stage.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            //base.Draw(gameTime);
        }
    }
}
