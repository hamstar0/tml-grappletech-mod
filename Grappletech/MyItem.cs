using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsGeneral.Libraries.Items.Attributes;


namespace Grappletech {
	class GrappletechItem : GlobalItem {
		private static void MessageAboutGrapplingChanges() {
			Messages.MessagesAPI.AddMessagesCategoriesInitializeEvent( () => {
				Messages.MessagesAPI.AddMessage(
					title: "Note: Grappling changes are now in effect",
					description: "Grappling hooks must now be used on only wood objects.",
					modOfOrigin: GrappletechMod.Instance,
					id: "GrappletechGrappleChanges",
					parentMessage: Messages.MessagesAPI.ModInfoCategoryMsg
				);
			} );
		}



		////////////////

		public override bool OnPickup( Item item, Player player ) {
			if( ItemAttributeLibraries.IsGrapple( item ) ) {
				if( ModLoader.GetMod( "Messages" ) != null ) {
					GrappletechItem.MessageAboutGrapplingChanges();
				}
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