using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace rogueliche
{
    public class UI
    {
        private const int LeftHudOffset = 2;
        private const int WarningFlashDelay = 300;
        private readonly int HudBarLength;
        private readonly int WeaponHudTopOffset;
        private bool warningsVisible = false;
        private float warningFlashTimer = 0;
        private bool displayOverheads;
        private Point overheadOffset = new Point(-2, -2);

        public UI(Graphics render)
        {
            displayOverheads = true;
            HudBarLength = (render.Width - LeftHudOffset * 2) / 2;
            WeaponHudTopOffset = render.Height * 7 / 8;
        }

        public void ToggleOverheads()
        {
            displayOverheads = !displayOverheads;
        }

        public void Update()
        {
            FlashWarnings();
        }

        public void Draw(Graphics render, Player player)
        {
            if (displayOverheads)
            {
                DisplayOverheads(render, player);
            }

            DrawPlayerHUD(render, player);

            DrawWeaponHUD(render, player);
        }

        private void FlashWarnings()
        {
            float delay = WarningFlashDelay / 1000;

            if (warningFlashTimer > delay)
            {
                warningFlashTimer = 0;
                warningsVisible = !warningsVisible;
            }
            else
            {
                warningFlashTimer += 0.1f;
            }
        }

        private void DrawPlayerHUD(Graphics render, Player player)
        {
            DrawPlayerName(render, player);
            DrawPlayerLevel(render, player);
            DrawPlayerExperience(render, player);
            DrawPlayerLocationName(render, player);
            DrawPlayerHealth(render, player);
            DrawPlayerExertion(render, player);
        }

        private void DrawPlayerExertion(Graphics render, Player player)
        {
            string exer = "Exertion: " + player.Exertion + "/" + player.MaxExertion;
            render.DrawString(exer, render.Width * 3 / 4 - exer.Length / 2, 6);
            if (player.Exertion >= player.MaxExertion * 4 / 5 && warningsVisible)
                render.DrawString("Overexerted!", render.Width * 3 / 4 - exer.Length / 2 + exer.Length + 1, 6);
            render.DrawBar(player.Exertion, player.MaxExertion, HudBarLength, render.Width * 3 / 4 - HudBarLength / 2, 7);
        }

        private void DrawPlayerHealth(Graphics render, Player player)
        {
            string hp = "Health: " + player.Health + "/" + player.MaxHealth;
            render.DrawString(hp, render.Width * 1 / 4 - hp.Length / 2, 6);
            if (player.Health <= player.MaxHealth * 1 / 5 && warningsVisible)
                render.DrawString("Health critical!", render.Width * 1 / 4 - hp.Length / 2 + hp.Length + 1, 6);
            render.DrawBar(player.Health, player.MaxHealth, HudBarLength, render.Width * 1 / 4 - HudBarLength / 2, 7);
        }

        private static void DrawPlayerLocationName(Graphics render, Player player)
        {
            string flr = player.Location.Name;
            render.DrawString(flr, render.Width * 4 / 5 - flr.Length / 2, 3);
        }

        private void DrawPlayerExperience(Graphics render, Player player)
        {
            string exp = "Experience: " + player.Exp + "/" + player.ExpToNextLvl;
            render.DrawString(exp, render.Width * 1 / 2 - exp.Length / 2, 3);
            render.DrawBar(player.Exp, player.ExpToNextLvl, HudBarLength, render.Width / 2 - HudBarLength / 2, 4);
        }

        private static void DrawPlayerLevel(Graphics render, Player player)
        {
            string lvl = "Level: " + player.Lvl;
            render.DrawString(lvl, render.Width * 1 / 5 - lvl.Length / 2, 3);
        }

        private static void DrawPlayerName(Graphics render, Player player)
        {
            string name = player.Name;
            render.DrawString(name, render.Width * 1 / 2 - name.Length / 2, 1);
        }

        private void DrawWeaponHUD(Graphics render, Player player)
        {
            string wpn = player.CurrentWeapon.Overhead;
            render.DrawString(wpn, render.Width * 1 / 2 - wpn.Length / 2, WeaponHudTopOffset);

            string drb = "Durability: " + player.CurrentWeapon.Durability + "/" + player.CurrentWeapon.MaxDurability;
            render.DrawString(drb, render.Width * 1 / 2 - drb.Length / 2, WeaponHudTopOffset + 2);
            render.DrawBar(player.CurrentWeapon.Durability, player.CurrentWeapon.MaxDurability, HudBarLength, render.Width / 2 - HudBarLength / 2, WeaponHudTopOffset + 3);

            string dmg = "Damage: " + player.CurrentWeapon.Damage;
            render.DrawString(dmg, render.Width * 2 / 5 - dmg.Length / 2, WeaponHudTopOffset + 5);

            string wgt = "Weight: " + player.CurrentWeapon.StaminaCost;
            render.DrawString(wgt, render.Width * 3 / 5 - wgt.Length / 2, WeaponHudTopOffset + 5);
        }

        private void DisplayOverheads(Graphics render, Player player)
        {
            var tilemap = player.Location.Tilemap;

            tilemap.PerformOnVisibleTiles((point) => DrawOverheadAtPosition(point, tilemap, render, player), render, player);
        }

        private void DrawOverheadAtPosition(Point pos, Tilemap tilemap, Graphics render, Player player)
        {
            var offsetPos = new Point(pos.X - Graphics.CameraTransform(player, render).X, pos.Y - Graphics.CameraTransform(player, render).Y);

            if (render.IsWithinBuffer(offsetPos) && tilemap.IsVisible(pos) && tilemap.TopMappable(pos) is IMappable mappable)
            {
                var overheadPos = new Point(offsetPos.X + overheadOffset.X, offsetPos.Y + overheadOffset.Y);

                DrawOverhead(mappable, overheadPos, render);
            }
        }

        private void DrawOverhead(IMappable mappable, Point pos, Graphics render)
        {
            render.DrawString(mappable.Overhead, pos);
        }
    }
}
