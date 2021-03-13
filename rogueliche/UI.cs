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

        public UI(Graphics graphics)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException();
            }
            displayOverheads = true;
            HudBarLength = (graphics.Width - LeftHudOffset * 2) / 2;
            WeaponHudTopOffset = graphics.Height * 7 / 8;
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

            DrawPlayerHud(render, player);

            DrawWeaponHud(render, player);
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

        private void DrawPlayerHud(Graphics render, Player player)
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
            var exertionPos = new Point(render.Width * 3 / 4, 6);
            var exertionText = "Exertion: " + player.Exertion + "/" + player.MaxExertion;
            var exertionTextPos = new Point(exertionPos.X - exertionText.Length, exertionPos.Y);

            render.DrawString(exertionText, exertionTextPos);

            if (player.Exertion >= player.MaxExertion * 4 / 5 && warningsVisible)
                render.DrawString("Overexerted!", exertionTextPos.X + exertionText.Length + 1, exertionTextPos.Y);
            render.DrawBar(player.Exertion, player.MaxExertion, HudBarLength, render.Width * 3 / 4 - HudBarLength / 2, exertionPos.Y + 1);
        }

        private void DrawPlayerHealth(Graphics render, Player player)
        {
            var healthPos = new Point(render.Width * 1 / 4, 6);
            var healthText = "Health: " + player.Health + "/" + player.MaxHealth;
            var healthTextPos = new Point(healthPos.X - healthText.Length, healthPos.Y); 

            render.DrawString(healthText, healthTextPos);

            if (player.Health <= player.MaxHealth * 1 / 5 && warningsVisible)
                render.DrawString("Health critical!", healthTextPos.X + healthText.Length + 1, healthTextPos.Y);
            render.DrawBar(player.Health, player.MaxHealth, HudBarLength, healthPos.X - HudBarLength / 2, healthPos.Y + 1);
        }

        private static void DrawPlayerLocationName(Graphics render, Player player)
        {
            var floorText = player.Location.Name;
            var floorTextPos = new Point(render.Width * 4 / 5 - floorText.Length / 2, 3);
            render.DrawString(floorText, floorTextPos.X, floorTextPos.Y);
        }

        private void DrawPlayerExperience(Graphics render, Player player)
        {
            var experiencePos = new Point(render.Width * 1 / 2, 3);
            var experienceText = "Experience: " + player.Exp + "/" + player.ExpToNextLvl;
            var experienceTextPos = new Point(experiencePos.X - experienceText.Length / 2, experiencePos.Y);
            render.DrawString(experienceText, experienceTextPos);
            render.DrawBar(player.Exp, player.ExpToNextLvl, HudBarLength, experiencePos.X - HudBarLength / 2, experiencePos.Y + 1);
        }

        private static void DrawPlayerLevel(Graphics render, Player player)
        {
            var lvlPos = new Point(render.Width * 1 / 5, 3);
            var lvlText = "Level: " + player.Lvl;
            render.DrawString(lvlText, lvlPos.X - lvlText.Length / 2, lvlPos.Y);
        }

        private static void DrawPlayerName(Graphics render, Player player)
        {
            var namePos = new Point(render.Width * 1 / 2, 1);
            var nameText = player.Name;
            render.DrawString(nameText, namePos.X - nameText.Length / 2, namePos.Y);
        }

        private void DrawWeaponHud(Graphics render, Player player)
        {
            DrawCurrentWeaponString(render, player);

            DrawWeaponDurability(render, player);

            DrawWeaponDamage(render, player);

            DrawWeaponWeight(render, player);
        }

        private void DrawCurrentWeaponString(Graphics render, Player player)
        {
            var weaponText = player.CurrentWeapon.Overhead;
            render.DrawString(weaponText, render.Width * 1 / 2 - weaponText.Length / 2, WeaponHudTopOffset);
        }

        private void DrawWeaponDurability(Graphics render, Player player)
        {
            var durabilityText = "Durability: " + player.CurrentWeapon.Durability + "/" + player.CurrentWeapon.MaxDurability;
            render.DrawString(durabilityText, render.Width * 1 / 2 - durabilityText.Length / 2, WeaponHudTopOffset + 2);
            render.DrawBar(player.CurrentWeapon.Durability, player.CurrentWeapon.MaxDurability, HudBarLength, render.Width / 2 - HudBarLength / 2, WeaponHudTopOffset + 3);
        }

        private void DrawWeaponDamage(Graphics render, Player player)
        {
            var damageText = "Damage: " + player.CurrentWeapon.Damage;
            render.DrawString(damageText, render.Width * 2 / 5 - damageText.Length / 2, WeaponHudTopOffset + 5);
        }

        private void DrawWeaponWeight(Graphics render, Player player)
        {
            var weightText = "Weight: " + player.CurrentWeapon.StaminaCost;
            render.DrawString(weightText, render.Width * 3 / 5 - weightText.Length / 2, WeaponHudTopOffset + 5);
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
