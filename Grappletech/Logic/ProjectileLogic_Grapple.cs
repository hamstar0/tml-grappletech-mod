using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsGeneral.Libraries.Fx;


namespace Grappletech.Logic {
	static partial class ProjectileLogic {
		public static bool IsTileIgnoringGrapple( Tile tile ) {
			return ProjectileLogic.IsTileStateIgnoringGrapple( tile )
				|| ProjectileLogic.IsTileTypeIgnoringGrapple( tile.type );
		}
		
		public static bool IsTileStateIgnoringGrapple( Tile tile ) {
			if( tile?.active() != true ) {
				return true;
			}
			if( tile.inActive() ) {	// actuated
				return true;
			}

			return false;
		}

		public static bool IsTileTypeIgnoringGrapple( int tileType ) {
			if( !Main.tileSolid[tileType] ) {
				if( tileType != TileID.MinecartTrack ) {
					return true;
				}
			}

			return false;
		}
		
		////
		
		public static bool IsTileTypeAlwaysGrappleable( int tileType ) {
			var config = GrappletechConfig.Instance;

			if( config.Get<bool>( nameof(config.GrappleableTileWhitelistWoodAndPlatforms) ) ) {
				switch( tileType ) {
				case TileID.Platforms:
				case TileID.PlanterBox:
				case TileID.MinecartTrack:  // i guess
				case TileID.WoodBlock:
				case TileID.BorealWood:
				case TileID.DynastyWood:
				case TileID.PalmWood:
				case TileID.Ebonwood:
				case TileID.Shadewood:
				case TileID.Pearlwood:
				case TileID.SpookyWood:
				case TileID.LivingWood:
				case TileID.LeafBlock:  // sure
				case TileID.LivingMahogany:
				case TileID.RichMahogany:
				case TileID.LivingMahoganyLeaves:   // sure
					return true;
				}
			}

			//

			var whitelist = config.Get<HashSet<string>>( nameof( config.GrappleableTileWhitelist ) );
			string tileUid = TileID.GetUniqueKey( tileType );

			return whitelist.Contains( tileUid );
		}
		
		////

		public static bool IsTilePositionGrappleable( int tileX, int tileY ) {
			if( tileX <= 1 || tileX >= Main.maxTilesX-1 ) {
				return false;
			}
			if( tileY <= 1 || tileY >= Main.maxTilesY-1 ) {
				return false;
			}

			//

			var config = GrappletechConfig.Instance;
			if( !config.Get<bool>( nameof(config.GrappleableTileNarrowFormations) ) ) {
				return false;
			}

			//

			var tilesNear = new List<Tile>( 8 );
			tilesNear.Add( Framing.GetTileSafely( tileX-1, tileY-1 ) ); //nw
			tilesNear.Add( Framing.GetTileSafely( tileX, tileY-1 ) );   //n
			tilesNear.Add( Framing.GetTileSafely( tileX+1, tileY-1 ) ); //ne

			tilesNear.Add( Framing.GetTileSafely( tileX-1, tileY ) );   //w
			tilesNear.Add( Framing.GetTileSafely( tileX+1, tileY ) );   //e

			tilesNear.Add( Framing.GetTileSafely( tileX-1, tileY+1 ) ); //sw
			tilesNear.Add( Framing.GetTileSafely( tileX, tileY+1 ) );   //s
			tilesNear.Add( Framing.GetTileSafely( tileX+1, tileY+1 ) ); //se

			//

			int neighbors = 0;
			//int consecutiveNeighbors = 0;

			for( int k=0; k<tilesNear.Count; k++ ) {
				int next = k <= tilesNear.Count - 1 ? k + 1 : 0;
				Tile tileNear = tilesNear[k];
				Tile tileNext = tilesNear[next];

				bool hereIgnore = ProjectileLogic.IsTileIgnoringGrapple( tileNear );
				bool nextIgnore = ProjectileLogic.IsTileIgnoringGrapple( tileNext );

				if( !hereIgnore ) {
					neighbors++;
				}

				//if( !hereIgnore && !nextIgnore ) {
				//	consecutiveNeighbors++;
				//}
			}

			if( neighbors <= 3 ) {
				return true;
			}

			//if( consecutiveNeighbors <= 2 ) {
			//	return true;
			//}

			return false;
		}
		
		////

		public static bool? IsTileNormallyGrappleable( int tileX, int tileY ) {
			Tile tile = Main.tile[tileX, tileY];

			if( ProjectileLogic.IsTileIgnoringGrapple(tile) ) {
				return null;
			}

			if( ProjectileLogic.IsTileTypeAlwaysGrappleable(tile.type) ) {
				return true;
			}
			if( ProjectileLogic.IsTilePositionGrappleable(tileX, tileY) ) {
				return true;
			}

			return false;
		}


		////////////////
		
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
