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
					description: "Grappling hooks must now be used on only wood objects or 1-wide tile formations (e.g. a pole).",
					modOfOrigin: GrappletechMod.Instance,
					id: "GrappletechGrappleChanges",
					alertPlayer: Messages.MessagesAPI.IsUnread("GrappletechGrappleChanges"),
					isImportant: false,
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
			bool allowsWood = config.Get<bool>( nameof(config.GrappleableTileWhitelistWoodAndPlatforms) );
			bool allowsPoles = config.Get<bool>( nameof(config.GrappleableTileWhitelistNarrowFormations) );

			string text;

			if( allowsWood && allowsPoles ) {
				text = "Only works on wood, platforms, and 1-wide tile formations";
			} else if( allowsWood ) {
				text = "Only works on wood and platforms";
			} else if( allowsPoles ) {
				text = "Only works on 1-wide tile formations";
			} else {
				return;
			}

			string modName = "[c/FFFF88:" + GrappletechMod.Instance.DisplayName + "] - ";

			TooltipLine tip = new TooltipLine( this.mod, "Grappletech", modName + text );
			ItemInformationAttributeLibraries.ApplyTooltipAt( tooltips, tip );
		}
	}
}