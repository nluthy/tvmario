using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;

namespace TVMario
{
    public class MyForm : VisibleGameEntity
    {
        List<MyButton> _buttons;
        int iDelay = 0;

        public List<MyButton> Buttons
        {
            get { return _buttons; }
            set { _buttons = value; }
        }
        Texture2D _background;

        public Texture2D Background
        {
            get { return _background; }
            set { _background = value; }
        }

        public MyForm(String strXML, ContentManager content)
        {
            Buttons = new List<MyButton>();

            XmlTextReader xml = new XmlTextReader(strXML);
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode form = doc.GetElementsByTagName("myform")[0];
            XmlNode bg = form.FirstChild;
            String strBackground = bg.InnerText;
            XmlNodeList buttonslist = doc.GetElementsByTagName("mybutton");
            foreach (XmlNode node in buttonslist)
            {
                String strfont = node["font"].InnerText;
                String strText = node["text"].InnerText;
                String strTopLeftX = node["topleft"]["x"].InnerText;
                String strTopLeftY = node["topleft"]["y"].InnerText;
                String strSizeX = node["size"]["x"].InnerText;
                String strSizeY = node["size"]["y"].InnerText;
                float topLeftX = float.Parse(strTopLeftX);
                float topLeftY = float.Parse(strTopLeftY);
                float sizeX = float.Parse(strSizeX);
                float sizeY = float.Parse(strSizeY);
                int texturesCount = Int32.Parse(node["texturescount"].InnerText);
                XmlNodeList textures = node["textures"].ChildNodes;
                String[] strTextures = new String[textures.Count];
                for (int i = 0; i < textures.Count; i++)
                {
                    strTextures[i] = textures[i].InnerText;
                }
                SpriteFont font = content.Load<SpriteFont>(strfont);
                MyButton button = new MyButton(font, strText, content, strTextures, texturesCount, new Vector2(topLeftX, topLeftY), new Vector2(sizeX, sizeY));
                Buttons.Add(button);
            }
            xml.Close();
            Background = content.Load<Texture2D>(strBackground);

        }

        public override void Update(GameTime gameTime)
        {
            foreach (MyButton button in _buttons)
            {
                button.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, Color color)
        {
            if (Background != null)
                spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            foreach (MyButton button in _buttons)
            {
                if (button != null)
                    button.Draw(gameTime, spriteBatch);
            }
            
        }
    }
}
