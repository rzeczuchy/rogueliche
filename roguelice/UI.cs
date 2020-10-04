using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class UI
    {
        public UI()
        {
            displayOverheads = true;
        }

        private bool flashWarnings = false;

        private bool displayOverheads;

        public void ToggleOverheads()
        {
            displayOverheads = !displayOverheads;
        }

        public void Draw(Graphics render, Player player)
        {
            flashWarnings = !flashWarnings;

            if (displayOverheads)
            {
                DisplayOverheads(render, player);
            }

            DisplayPlayerHUD(render, player);

            DisplayWeaponHUD(render, player);
        }

        void DisplayPlayerHUD(Graphics render, Player player)
        {
            int leftOffset = 2;

            int hBarLength = (render.Width - leftOffset * 2) / 2;

            string name = player.Name;
            render.DrawString(name, render.Width * 1 / 2 - name.Length / 2, 1);

            string lvl = "Level: " + player.Lvl;
            render.DrawString(lvl, render.Width * 1 / 5 - lvl.Length / 2, 3);

            string exp = "Experience: " + player.Exp + "/" + player.ExpToNextLvl;
            render.DrawString(exp, render.Width * 1 / 2 - exp.Length / 2, 3);
            render.DrawBar(player.Exp, player.ExpToNextLvl, hBarLength, render.Width / 2 - hBarLength / 2, 4);

            string flr = "Location: " + player.Location.Name;
            render.DrawString(flr, render.Width * 4 / 5 - flr.Length / 2, 3);

            string hp = "Health: " + player.Health + "/" + player.MaxHealth;
            render.DrawString(hp, render.Width * 1 / 4 - hp.Length / 2, 6);
            if (player.Health <= player.MaxHealth * 1 / 5 && flashWarnings)
                render.DrawString("Health critical!", render.Width * 1 / 4 - hp.Length / 2 + hp.Length + 1, 6);
            render.DrawBar(player.Health, player.MaxHealth, hBarLength, render.Width * 1 / 4 - hBarLength / 2, 7);

            string exer = "Exertion: " + player.Exertion + "/" + player.MaxExertion;
            render.DrawString(exer, render.Width * 3 / 4 - exer.Length / 2, 6);

            if (player.Exertion >= player.MaxExertion * 4 / 5 && flashWarnings)
                render.DrawString("Overexerted!", render.Width * 3 / 4 - exer.Length / 2 + exer.Length + 1, 6);
            render.DrawBar(player.Exertion, player.MaxExertion, hBarLength, render.Width * 3 / 4 - hBarLength / 2, 7);
        }

        void DisplayWeaponHUD(Graphics render, Player player)
        {
            int leftOffset = 2;
            int topOffset = render.Height * 7 / 8;

            int hBarLength = (render.Width - leftOffset * 2) / 2;

            string wpn = player.CurrentWeapon.Overhead;
            render.DrawString(wpn, render.Width * 1 / 2 - wpn.Length / 2, topOffset);

            string drb = "Durability: " + player.CurrentWeapon.Durability + "/" + player.CurrentWeapon.MaxDurability;
            render.DrawString(drb, render.Width * 1 / 2 - drb.Length / 2, topOffset + 2);
            render.DrawBar(player.CurrentWeapon.Durability, player.CurrentWeapon.MaxDurability, hBarLength, render.Width / 2 - hBarLength / 2, topOffset + 3);

            string dmg = "Damage: " + player.CurrentWeapon.Damage;
            render.DrawString(dmg, render.Width * 2 / 5 - dmg.Length / 2, topOffset + 5);

            string wgt = "Weight: " + player.CurrentWeapon.StaminaCost;
            render.DrawString(wgt, render.Width * 3 / 5 - wgt.Length / 2, topOffset + 5);
        }

        void DisplayOverheads(Graphics render, Player player)
        {
            // camera transforms
            int xtransform = player.Position.X - render.Width / 2;
            int ytransform = player.Position.Y - render.Height / 2;

            // overhead offset
            Point offset = new Point(-2, -2);

            ILocation level = player.Location;
            for (int y = 0; y < level.Tilemap.Height; y++)
                for (int x = 0; x < level.Tilemap.Width; x++)
                {
                    var pos = new Point(x, y);
                    var item = level.Tilemap.GetItem(pos);
                    var creature = level.Tilemap.GetCreature(pos);

                    if (item != null && item.Overhead != null && creature == null
                        && render.IsWithinBuffer(x - xtransform, y - ytransform) && player.CanSee(pos))
                    {
                        render.DrawString(item.Overhead, x - xtransform + offset.X, y - ytransform + offset.Y);
                    }

                    if (creature != null && creature.Overhead != null
                        && render.IsWithinBuffer(x - xtransform, y - ytransform) && player.CanSee(pos))
                    {
                        render.DrawString(creature.Overhead, x - xtransform + offset.X, y - ytransform + offset.Y);
                    }
                }
        }

    }
}
