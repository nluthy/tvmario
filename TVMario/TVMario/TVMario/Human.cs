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
    public class Human : Character
    {

        public void Init(ContentManager content, string strXML)
        {
            XmlTextReader xml = new XmlTextReader(strXML);
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode human = doc.GetElementsByTagName("human")[0];
            string strPreTextures = human["pretextures"].InnerText;
            string strTopLeftX = human["topleft"]["x"].InnerText;
            string strTopLeftY = human["topleft"]["y"].InnerText;
            string strSizeX = human["size"]["x"].InnerText;
            string strSizeY = human["size"]["y"].InnerText;
            float topLeftX = float.Parse(strTopLeftX);
            float topLeftY = float.Parse(strTopLeftY);
            float sizeX = float.Parse(strSizeX);
            float sizeY = float.Parse(strSizeY);
            int textureCount = Int32.Parse(human["texturecount"].InnerText);
            Vector2 topLeft = new Vector2(topLeftX, topLeftY);
            Vector2 size = new Vector2(sizeX, sizeY);
            xml.Close();
            this.Init(content, strPreTextures, textureCount, topLeft, size);
        }
    }
}
