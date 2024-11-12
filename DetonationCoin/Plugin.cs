using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using UnityEngine;

namespace DetonationCoin
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "LuckyCoin";
        public override string Prefix => "LuckyCoin";
        public override string Author => "SIN KIPU";
        public override Version Version { get; } = new Version(1, 0, 1);
        public static Plugin plugin;

        private Vector3 surfaceNukePosition = new Vector3(30, 992, -26);

        public override void OnEnabled()
        {
            plugin = this;
            Exiled.Events.Handlers.Player.FlippingCoin += OnFlippingCoin;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.FlippingCoin -= OnFlippingCoin;
            base.OnDisabled();
        }

        public void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            float random = UnityEngine.Random.Range(0f, 100f);
            if (random <= 30)
            {
                ev.Player.Role.Set(RoleTypeId.Scp0492);
                Log.Info($"Player {ev.Player.Nickname} has made a zombie:)");
                ev.Player.Broadcast(3, "Ты стал зомби :)");
            }
            else if (random > 30f && random <= 60f)
            {
                ev.Player.Broadcast(3, "Ничего не произошло...");
            }
            else if (random > 60f && random <= (100f - Config.SCP001Chance))
            {
                ev.Player.Broadcast(3, "Удача улыбнулась тебе!");
                ev.Player.AddItem(ItemType.SCP500);
                ev.Player.AddItem(ItemType.Coin);
                ev.Player.AddItem(ItemType.ArmorLight);
                ev.Player.AddItem(ItemType.GunCOM15);
                ev.Player.AddAmmo(Exiled.API.Enums.AmmoType.Nato9, 20);
                ev.Player.ThrowGrenade(Exiled.API.Enums.ProjectileType.Flashbang);
            }
            else
            {
                ev.Player.EnableEffect(Exiled.API.Enums.EffectType.Flashed, 1.5f);
                Log.Info($"Warning! Player {ev.Player.Nickname} has made an SCP-001!");
                ev.Player.Broadcast(5, "Что произошло???");
                ev.Player.Role.Set(RoleTypeId.Tutorial);
                ev.Player.ClearInventory();
                ev.Player.AddItem(ItemType.GunRevolver);
                ev.Player.AddAmmo(Exiled.API.Enums.AmmoType.Ammo44Cal, 1000);
                ev.Player.AddItem(ItemType.ArmorHeavy);
                ev.Player.Position = surfaceNukePosition;
                ev.Player.CustomName = "SCP 001";
                ev.Player.Heal(1500);
                ev.Player.Health = 1500;
            }
        }
    }
}