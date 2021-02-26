using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Grappletech {
	public partial class GrappletechConfig : ModConfig {
		public static GrappletechConfig Instance => ModContent.GetInstance<GrappletechConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		[DefaultValue( true )]
		public bool GrappleableWoodAndPlatforms { get; set; } = true;

		//

		public HashSet<string> GrappleableTileWhitelist { get; set; } = new HashSet<string>();


		////

		[DefaultValue( true )]
		public bool TensionedHookBracerRecipeEnabled { get; set; } = true;



		////////////////

		public override ModConfig Clone() {
			var clone = base.Clone() as GrappletechConfig;

			clone.GrappleableTileWhitelist = new HashSet<string>( this.GrappleableTileWhitelist );

			return clone;
		}
	}
}
