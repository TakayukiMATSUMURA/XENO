namespace XENO.Cards
{
    public class Hero : Card
    {
        public Hero() : base("英雄", 10, false)
        {
        }

        public override void SetRebirth(Card card)
        {
            _rebirth = card;
        }

        public override Card BeReborn()
        {
            return _rebirth;
        }

        private Card _rebirth;
    }
}
