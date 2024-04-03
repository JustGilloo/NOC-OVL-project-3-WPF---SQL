namespace Groepsproject_Blokken
{
    public partial class Admin //Snap de warning niet zo goed, zorgt voor geen probleem maar alsnog
    {
        public override bool Equals(object obj)
        {
            bool resultaat = false;
            if (obj != null)
            {
                if (GetType() == obj.GetType())
                {
                    Admin g = (Admin)obj;
                    if (this.Name == g.Name)
                    {
                        resultaat = true;
                    }
                }
            }
            return resultaat;
        }
    }
}
