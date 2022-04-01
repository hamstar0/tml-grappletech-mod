using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


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

			int rectRad = 2;
			int xMin = tileX - rectRad;
			int xMax = tileX + rectRad;
			int yMin = tileY - rectRad;
			int yMax = tileY + rectRad;

			for( int x=xMin; x<=xMax; x++ ) {
				if( x <= 0 || x >= Main.maxTilesX ) {
					continue;
				}

				for( int y=yMin; y<=yMax; y++ ) {
					if( y <= 0 || y >= Main.maxTilesY ) {
						continue;
					}
					if( x == tileX && y == tileY ) {
						continue;
					}

					tilesNear.Add( Framing.GetTileSafely(x, y) );
				}
			}

			//

			int neighbors = 0;
			//int consecutiveNeighbors = 0;

			for( int k=0; k<tilesNear.Count; k++ ) {
				int next = k < (tilesNear.Count - 1) ? k + 1 : 0;
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

			if( neighbors <= 10 ) {	//3
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
	}
}
