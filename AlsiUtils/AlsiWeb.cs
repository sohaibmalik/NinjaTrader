namespace AlsiUtils
{

    partial class EmailList
    {
        //public override string ToString()
        //{
        //    return Email;
        //}
    }

    public partial class AlsiWebDataContext
    {
        public AlsiWebDataContext()
            : base(@"Data Source=85.214.244.19;Initial Catalog=AlsiWeb;Persist Security Info=True;User ID=Tradebot;Password=boeboe;MultipleActiveResultSets=True
", mappingSource)
        {
            OnCreated();
        }
    }
   
}
