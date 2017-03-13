namespace Apps.Models.LianTong
{
    public class LianTong_ContractsCntViewModels
    {
        public int NewContracts { get; set; }
        public int ConnectWait { get; set; }
        public int ApproveWait { get; set; }
        public int FullWait { get; set; }
        public int CheckWait { get; set; }
        public int InvoiceWait { get; set; }
        public int PaymentsWait { get; set; }
        public int OverContracts { get; set; }

        public int total
        {
            get
            {
                return this.NewContracts + this.ApproveWait + this.FullWait + this.CheckWait + this.InvoiceWait + this.PaymentsWait + this.OverContracts;
            }
            set
            {

            }
        }
    }
}
