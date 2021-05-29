using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsGeneral.Libraries.Items.Attributes;
using ModLibsInterMod.Libraries.Mods.APIMirrors.ModHelpersAPIMirrors;


namespace Grappletech {
	class GrappletechItem : GlobalItem {
		public override bool OnPickup( Item item, Player player ) {
			if( ItemAttributeLibraries.IsGrapple( item ) ) {
				InboxAPIMirrorsLibraries.SetMessage(
					"GrappletechGrappleChanges",
					"Grappletech: Grappling hooks must now be used on only wood objects.",
					false
				);
			}
			return base.OnPickup( item, player );
		}


		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			if( !ItemAttributeLibraries.IsGrapple( item ) ) {
				return;
			}

			var config = GrappletechConfig.Instance;
			if( !config.Get<bool>( nameof(config.GrappleableWoodAndPlatforms) ) ) {
				return;
			}

			string modName = "[c/FFFF88:" + GrappletechMod.Instance.DisplayName + "] - ";
			string text = "Only works on wood and platforms";

			TooltipLine tip = new TooltipLine( this.mod, "Grappletech", modName + text );
			ItemInformationAttributeLibraries.ApplyTooltipAt( tooltips, tip );
		}
	}
}