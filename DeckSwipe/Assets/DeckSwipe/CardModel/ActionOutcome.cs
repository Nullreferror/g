using DeckSwipe.CardModel.DrawQueue;

namespace DeckSwipe.CardModel
{
    public class ActionOutcome : IActionOutcome
    {
        private readonly StatsModification statsModification;
        private readonly IFollowup followup;

        public ActionOutcome(StatsModification statsModification, IFollowup followup)
        {
            this.statsModification = statsModification;
            this.followup = followup;
        }

        public void Perform(Game controller)
        {
            statsModification.Perform();
            if (followup != null)
            {
                controller.AddFollowupCard(followup);
            }
            controller.CardActionPerformed();
        }
    }
}
