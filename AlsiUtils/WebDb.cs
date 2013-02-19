namespace AlsiUtils
{
    partial class EmailList
    {
        public override string ToString()
        {
            return Email;
        }
    }

    public partial class WebDbDataContext
    {
        public WebDbDataContext()
            : base(@"Data Source=winsqls01.cpt.wa.co.za;Initial Catalog=AlsiDb;Persist Security Info=True;User ID=Pieter;Password=1Rachelle", mappingSource)
        {
            OnCreated();
        }
    }
}
