using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OverlayWindow;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using System.Windows.Forms;
using System.IO;

namespace SimpleOverlayWindow
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : FullScreenOverlayGame
    {
        //GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D icon = null;
        byte transparency = 128;
        bool transparencyLetGo = false;
        bool fileSelectLetGo = false;
        bool openFileSelect = true;
        OpenFileDialog fileOpenDialog = null;
        DialogResult result = DialogResult.None;

        public Game1()
        {
            //graphics = new GraphicsDeviceManager(this);
            //Content.RootDirectory = "Content";
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
            fileOpenDialog = new OpenFileDialog();

            fileOpenDialog.InitialDirectory = "c:\\";
            fileOpenDialog.Filter = "Images (*.jpg;*.bmp;*.png;*.gif)|*.jpg;*.bmp;*.png;*.gif|All files (*.*)|*.*";
            fileOpenDialog.FilterIndex = 1;
            fileOpenDialog.RestoreDirectory = false;
            fileOpenDialog.CheckFileExists = true;


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
            icon = Content.Load<Texture2D>("BaseInstructions");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed ||
                ((Keyboard.GetState().IsKeyDown(Keys.LeftControl) || Keyboard.GetState().IsKeyDown(Keys.RightControl)) &&
                 Keyboard.GetState().IsKeyDown(Keys.OemTilde)))
            {
                if (transparencyLetGo == false)
                {
                    switch (transparency)
                    {
                        default:
                        case 0:
                            transparency = 64;
                            break;
                        case 64:
                            transparency = 128;
                            break;
                        case 128:
                            transparency = 192;
                            break;
                        case 192:
                            transparency = 255;
                            break;
                        case 255:
                            transparency = 0;
                            break;
                    }
                    transparencyLetGo = true;
                }
            }
            else
            {
                transparencyLetGo = false;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed ||
               ((Keyboard.GetState().IsKeyDown(Keys.LeftControl) || Keyboard.GetState().IsKeyDown(Keys.RightControl)) &&
                Keyboard.GetState().IsKeyDown(Keys.OemPipe)))
            {
                if (fileSelectLetGo == false)
                {
                    if (openFileSelect == true)
                    {
                        result = fileOpenDialog.ShowDialog();
                        fileSelectLetGo = true;
                        openFileSelect = false;
                    }
                    if (result == DialogResult.OK)
                    {
                        string fileName = fileOpenDialog.FileName;
                        FileStream filestream = new FileStream(fileName, FileMode.Open);
                        icon = Texture2D.FromStream(GraphicsDevice, filestream);
                        filestream.Dispose();
                        openFileSelect = true;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        openFileSelect = true;
                    }
                    else if (result == DialogResult.None)
                    {
                        // do nothing
                    }
                }
            }
            else
            {
                fileSelectLetGo = false;
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

            // TODO: Add your drawing code here
            Vector2 center = new Vector2(GetVirtualScreenAreaSize().Width / 2f, GetVirtualScreenAreaSize().Height / 2f);
            if (icon != null)
            {
                Vector2 origin = new Vector2(icon.Width / 2f, icon.Height / 2f);
                spriteBatch.Draw(icon, center, null, new Color(Color.White.R, Color.White.G, Color.White.B, transparency), 0, origin, 1, SpriteEffects.None, 0);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
