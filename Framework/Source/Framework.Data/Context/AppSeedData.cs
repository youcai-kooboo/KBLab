using System.Data.Entity;

namespace Framework.Data.Context
{
    public class AppSeedData : DropCreateDatabaseIfModelChanges<AppContext>
    {
        protected override void Seed(AppContext context)
        {            
            Seed();
        }

        private void Seed()
        {
          
        } 
    }
}
