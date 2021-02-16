using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace Grappletech.Logic {
	static partial class ProjectileLogic {
		public static bool IsTileGrappleable( Tile tile ) {
			if( !tile.active() || (!Main.tileSolid[tile.type] && tile.type != TileID.MinecartTrack) ) {
				return false;
			}

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
				return true;
			default:
				var config = GrappletechConfig.Instance;

				if( !config.Get<bool>( nameof(config.GrappleOnlyWoodAndPlatforms) ) ) {
					return true;
				}

				string tileUid = TileID.GetUniqueKey( tile.type );
				if( config.Get<HashSet<string>>( nameof(config.GrappleableTileWhitelist) ).Contains(tileUid) ) {
					return true;
				}

				return false;
			}
		}



		////////////////

		public static void UpdateGrapplePullSpeed( Player player, Projectile projectile, ref float speed ) {
			int x = (int)( projectile.Center.X / 16f );
			int y = (int)( projectile.Center.Y / 16f );

			bool? isGrappleable = ProjectileLogic.IsTileGrappleable( Main.tile[x, y] );

			if( isGrappleable.HasValue && !isGrappleable.Value ) {
				var myplayer = player.GetModPlayer<GrappletechPlayer>();

				if( !myplayer.IsEquippingTensionedHookBracer ) {
					speed = 0f;
				}
			}
		}

		////

		public static void UpdateForGrappleProjectile( Player player, Projectile projectile ) {
			var config = GrappletechConfig.Instance;
			if( !GrappletechConfig.Instance.Get<bool>( nameof(config.GrappleOnlyWoodAndPlatforms) ) ) {
				return;
			}

			if( projectile.ai[0] != 0 && projectile.ai[0] != 2 ) {
				return;
			}

			var myplayer = player.GetModPlayer<GrappletechPlayer>();
			if( myplayer.IsEquippingTensionedHookBracer ) {
				return;
			}

			Vector2 projCen = projectile.Center;
			Vector2 projVel = projectile.velocity;
			int nowX = (int)( projCen.X / 16f );
			int nowY = (int)( projCen.Y / 16f );
			int nextX = (int)( ( ( projVel.X * 0.5f ) + projCen.X ) / 16f );
			int nextY = (int)( ( ( projVel.Y * 0.5f ) + projCen.Y ) / 16f );
			int lastX = (int)( ( projVel.X + projCen.X ) / 16f );
			int lastY = (int)( ( projVel.Y + projCen.Y ) / 16f );

			/*int bah = 120;
			Timers.SetTimer( "grap", 3, false, () => {
				Dust.QuickDust( new Point(x, y), isNextSolid ? Color.Red : Color.Green );
				return bah-- > 0;
			} );*/
			if( projectile.ai[0] == 0 ) {
				Tile nextTile = Main.tile[nextX, nextY];
				Tile lastTile = Main.tile[lastX, lastY];

				if( nextTile?.active() == true && ( Main.tileSolid[nextTile.type] || nextTile.type == TileID.MinecartTrack ) ) {
					if( !ProjectileLogic.IsTileGrappleable( nextTile ) ) {
						projectile.ai[0] = 1;
					}
				} else
				if( lastTile?.active() == true && ( Main.tileSolid[lastTile.type] || lastTile.type == TileID.MinecartTrack ) ) {
					if( !ProjectileLogic.IsTileGrappleable( lastTile ) ) {
						projectile.ai[0] = 1;
					}
				}
			} else {
				Tile nowTile = Main.tile[nowX, nowY];

				if( nowTile?.active() == true && ( Main.tileSolid[nowTile.type] || nowTile.type == TileID.MinecartTrack ) ) {
					if( !ProjectileLogic.IsTileGrappleable( nowTile ) ) {
						projectile.ai[0] = 1;
					}
				}
			}
		}
	}
}
