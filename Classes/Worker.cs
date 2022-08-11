namespace Classes
{
    public abstract class Work<inputModel, processModel, DbContext>
    {
        public processModel _processModel { get; set; }
        public inputModel _inputModel { get; set; }
        public DbContext _dbContext { get; set; }
        protected Work(inputModel inputModel, processModel processModel)
        {
            _processModel = processModel;
            _inputModel = inputModel;
        }




        public virtual async Task job(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}