using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Services.Messages.Inbox;


namespace Grappletech {
	class GrappletechItem : GlobalItem {
		public override bool OnPickup( Item item, Player player ) {
			if( ItemAttributeHelpers.IsGrapple( item ) ) {
				InboxMessages.SetMessage(
					"GrappletechGrappleChanges",
					"Grappletech: Grappling hooks must now be used on only wood objects.",
					false
				);
			}
			return base.OnPickup( item, player );
		}
	}
}