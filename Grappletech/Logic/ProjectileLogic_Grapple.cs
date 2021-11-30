using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsGeneral.Libraries.Fx;


namespace Grappletech.Logic {
	static partial class ProjectileLogic {
		public static void UpdateGrapplePullSpeedForPlayerIf( Player player, Projectile projectile, ref float speed ) {
			var myplayer = player.GetModPlayer<GrappletechPlayer>();
			if( myplayer.IsEquippingTensionedHookBracer ) {
				return;
			}

			int tileX = (int)projectile.Center.X / 16;
			int tileY = (int)projectile.Center.Y / 16;

			bool? isGrappleable = ProjectileLogic.IsTileNormallyGrappleable( tileX, tileY );
			if( !isGrappleable.HasValue || isGrappleable.Value ) {
				return;
			}

			speed = 0f;
		}

		////

		public static void UpdateForGrappleProjectileForPlayerIf( Player player, Projectile projectile ) {
			if( projectile.ai[0] != 0f && projectile.ai[0] != 2f ) {
				return;
			}

			var myplayer = player.GetModPlayer<GrappletechPlayer>();
			if( myplayer.IsEquippingTensionedHookBracer ) {
				return;
			}

			var config = GrappletechConfig.Instance;
			if( !config.Get<bool>( nameof(config.GrappleableTileWhitelistWoodAndPlatforms) ) ) {
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
					bool? isGrappleable = ProjectileLogic.IsTileNormallyGrappleable( nextX, nextY );
					if( !isGrappleable.HasValue || !isGrappleable.Value ) {
						ProjectileLogic.HaltGrapple( projectile );
					}
				} else
				if( lastTile?.nactive() == true && (Main.tileSolid[lastTile.type] || lastTile.type == TileID.MinecartTrack) ) {
					bool? isGrappleable = ProjectileLogic.IsTileNormallyGrappleable( lastX, lastY );
					if( !isGrappleable.HasValue || !isGrappleable.Value ) {
						ProjectileLogic.HaltGrapple( projectile );
					}
				}
			} else {
				Tile nowTile = Main.tile[nowX, nowY];

				if( nowTile?.nactive() == true && (Main.tileSolid[nowTile.type] || nowTile.type == TileID.MinecartTrack) ) {
					bool? isGrappleable = ProjectileLogic.IsTileNormallyGrappleable( nowX, nowY );
					if( !isGrappleable.HasValue || !isGrappleable.Value ) {
						ProjectileLogic.HaltGrapple( projectile );
					}
				}
			}
		}


		////////////////

		public static void HaltGrapple( Projectile projectile ) {
			projectile.ai[0] = 1f;

			//

			ParticleFxLibraries.MakeDustCloud( projectile.Center, 1, 0.3f, 0.5f );
			Main.PlaySound( SoundID.Tink, projectile.Center );
		}
	}
}
