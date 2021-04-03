using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace Grappletech.Logic {
	static partial class ProjectileLogic {
		public static bool IsTileNormallyGrappleable( Tile tile ) {
			if( !tile.active() ) {
				return false;
			}
			if( !Main.tileSolid[tile.type] && tile.type != TileID.MinecartTrack ) {
				return false;
			}
			if( tile.inActive() ) {	// actuated
				return false;
			}
			
			var config = GrappletechConfig.Instance;

			if( config.Get<bool>( nameof(config.GrappleableWoodAndPlatforms) ) ) {
				switch( tile.type ) {
				case TileID.Platforms:
				case TileID.MinecartTrack:
				case TileID.WoodBlock:
				case TileID.BorealWood:
				case TileID.DynastyWood:
				case TileID.LivingWood:
				case TileID.LeafBlock:
				case TileID.PalmWood:
				case TileID.SpookyWood:
				case TileID.LivingMahogany:
				case TileID.RichMahogany:
				case TileID.LivingMahoganyLeaves:
				case TileID.PlanterBox:
					return true;
				}
			}

			var whitelist = config.Get<HashSet<string>>( nameof(config.GrappleableTileWhitelist) );
			string tileUid = TileID.GetUniqueKey( tile.type );

			return whitelist.Contains( tileUid );
		}



		////////////////

		public static void UpdateGrapplePullSpeedForPlayer( Player player, Projectile projectile, ref float speed ) {
			int x = (int)projectile.Center.X / 16;
			int y = (int)projectile.Center.Y / 16;

			bool? isGrappleable = ProjectileLogic.IsTileNormallyGrappleable( Main.tile[x, y] );

			if( isGrappleable.HasValue && !isGrappleable.Value ) {
				var myplayer = player.GetModPlayer<GrappletechPlayer>();

				if( !myplayer.IsEquippingTensionedHookBracer ) {
					speed = 0f;
				}
			}
		}

		////

		public static void UpdateForGrappleProjectileForPlayer( Player player, Projectile projectile ) {
			if( projectile.ai[0] != 0f && projectile.ai[0] != 2f ) {
				return;
			}

			var myplayer = player.GetModPlayer<GrappletechPlayer>();
			if( myplayer.IsEquippingTensionedHookBracer ) {
				return;
			}

			var config = GrappletechConfig.Instance;
			if( !config.Get<bool>( nameof(config.GrappleableWoodAndPlatforms) ) ) {
				if( config.Get<HashSet<string>>( nameof(config.GrappleableTileWhitelist) ).Count == 0 ) {
					return;
				}
			}

			Vector2 projCen = projectile.Center;
			Vector2 projVel = projectile.velocity;
			int nowX = (int)projCen.X / 16;
			int nowY = (int)projCen.Y / 16;
			int nextX = (int)((projVel.X * 0.5f) + projCen.X) / 16;
			int nextY = (int)((projVel.Y * 0.5f) + projCen.Y) / 16;
			int lastX = (int)(projVel.X + projCen.X) / 16;
			int lastY = (int)(projVel.Y + projCen.Y) / 16;

			/*int bah = 120;
			Timers.SetTimer( "grap", 3, false, () => {
				Dust.QuickDust( new Point(x, y), isNextSolid ? Color.Red : Color.Green );
				return bah-- > 0;
			} );*/
			if( projectile.ai[0] == 0 ) {
				Tile nextTile = Main.tile[nextX, nextY];
				Tile lastTile = Main.tile[lastX, lastY];

				if( nextTile?.nactive() == true && (Main.tileSolid[nextTile.type] || nextTile.type == TileID.MinecartTrack) ) {
					if( !ProjectileLogic.IsTileNormallyGrappleable( nextTile ) ) {
						projectile.ai[0] = 1f;
					}
				} else
				if( lastTile?.nactive() == true && (Main.tileSolid[lastTile.type] || lastTile.type == TileID.MinecartTrack) ) {
					if( !ProjectileLogic.IsTileNormallyGrappleable( lastTile ) ) {
						projectile.ai[0] = 1f;
					}
				}
			} else {
				Tile nowTile = Main.tile[nowX, nowY];

				if( nowTile?.nactive() == true && (Main.tileSolid[nowTile.type] || nowTile.type == TileID.MinecartTrack) ) {
					if( !ProjectileLogic.IsTileNormallyGrappleable( nowTile ) ) {
						projectile.ai[0] = 1f;
					}
				}
			}
		}
	}
}
