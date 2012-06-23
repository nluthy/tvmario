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
    public class Boss : Monster
    {
        Skill _skill;

        public Skill skill
        {
            get { return _skill; }
            set { _skill = value; }
        }

        public override void Init(ContentManager content, Vector2 topLeft, string strData)
        {
            //base.Init(content, topLeft, strData);
            XmlTextReader xml = new XmlTextReader(strData);
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            XmlNode monster = doc.GetElementsByTagName("monster")[0];
            string strPreTextures = monster["pretextures"].InnerText;
            string strBlood = monster["blood"].InnerText;
            string strSizeX = monster["size"]["x"].InnerText;
            string strSizeY = monster["size"]["y"].InnerText;
            float sizeX = float.Parse(strSizeX);
            float sizeY = float.Parse(strSizeY);
            int textureCount = Int32.Parse(monster["texturecount"].InnerText);
            int blood = Int32.Parse(strBlood);
            Vector2 size = new Vector2(sizeX, sizeY);

            string strSkillPreTextures = monster["skill"]["pretextures"].InnerText;
            string strSkillSizeX = monster["skill"]["size"]["x"].InnerText;
            string strSkillSizeY = monster["skill"]["size"]["y"].InnerText;
            float sizeSkillX = float.Parse(strSkillSizeX);
            float sizeSkillY = float.Parse(strSkillSizeY);
            int textureSkillCount = Int32.Parse(monster["skill"]["texturecount"].InnerText);
            Vector2 sizeSkill = new Vector2(sizeSkillX, sizeSkillY);
            string strSkillMaxLenght = monster["skill"]["maxlength"].InnerText;
            int maxSkillLength = Int32.Parse(strSkillMaxLenght);
            string strSkillType = monster["skill"]["type"].InnerText;
            int typeSkill = Int32.Parse(strSkillType);
            string strSkillDamage = monster["skill"]["damage"].InnerText;
            int damageSkill = Int32.Parse(strSkillDamage);
            skill = new Skill();
            skill.Init(content, strSkillPreTextures, textureSkillCount, sizeSkill, maxSkillLength, typeSkill, damageSkill);

            xml.Close();
            this.Init(content, strPreTextures, textureCount, topLeft, size, blood);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (skill.TopLeft == Vector2.Zero)
            {
                skill.skillRight = isRight;
                Vector2 cur = skill.TopLeft;
                if (isRight)
                {
                    cur.X = TopLeft.X + Sprites[0].Width;
                }
                else
                {
                    cur.X = TopLeft.X - skill.Sprites[0].Width;
                }
                cur.Y = TopLeft.Y;
                skill.TopLeft = cur;
                skill.show = true;
            }
            else
                skill.Update(gameTime, skill.skillRight, 2);
        }

        public override void NextFrame()
        {
            if (Sprites[0].CurrentTexture == Sprites[0].TexturesCount - 1)
            {
                Sprites[0].CurrentTexture = 0;
            }
            else
            {
                if (Sprites[0].CurrentTexture < Sprites[0].TexturesCount - 1)
                {
                    Sprites[0].CurrentTexture++;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, Color color)
        {
            base.Draw(spriteBatch, gameTime, color);
            if (isRight)
            {
                if (skill.TopLeft != Vector2.Zero)
                {
                    skill.Draw(spriteBatch, gameTime, color);
                }
            }
            else
            {
                if (skill.TopLeft != Vector2.Zero)
                {
                    skill.DrawFlipHorizontal(spriteBatch, gameTime, color);
                }
            }
        }

    }
}
