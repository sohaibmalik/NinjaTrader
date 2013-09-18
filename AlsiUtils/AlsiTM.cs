namespace AlsiUtils
{
    //"Data Source=sql.alsitm.com,1444;Initial Catalog=AlsiTM;Persist Security Info=True;User ID=Pieter;Password=1Rachelle"
    //"Data Source=winsqls01.cpt.wa.co.za;Initial Catalog=AlsiTM;Persist Security Info=True;User ID=Pieter;Password=1Rachelle"

    partial class AlsiTMDataContext
    {
        public AlsiTMDataContext()
            : base(@"Data Source=winsqls01.cpt.wa.co.za;Initial Catalog=AlsiTM;Persist Security Info=True;User ID=Pieter;Password=1Rachelle", mappingSource)
        {
            OnCreated();
        }
    }
}
