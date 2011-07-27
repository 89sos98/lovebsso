using Module;
using Logic;
namespace Web
{
    public class Home :HttpCustom
    {
        public override bool AllowCache
        {
            get
            {
                return false;
            }
        }
        protected override void Page_Load()
        {
            FillHome home = new FillHome(this);
            home.FillAllUser();
        }
   
    }
}
