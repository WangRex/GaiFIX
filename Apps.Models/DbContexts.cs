using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Apps.Common;
using System.Runtime.Remoting.Messaging;
using Apps.Models.Sys;
using Apps.Models.DEF;
using Apps.Models.Flow;
using Apps.Models.JOB;
using Apps.Models.MIS;
using Apps.Models.Spl;
using Apps.Models.WC;
using Apps.Models.Calendar;
using Apps.Models.FIX;

namespace Apps.Models
{
    /// <summary>
    /// 网站数据上下文
    /// <remarks>
    /// 创建：2016年3月2日
    /// </remarks>
    /// </summary>
    public class DbContexts : DbContext
    {

        //public DbSet<DEF_CaseType> DEF_CaseType { get; set; }
        //public DbSet<DEF_Defect> DEF_Defect { get; set; }
        //public DbSet<DEF_TestCase> DEF_TestCase { get; set; }
        //public DbSet<DEF_TestCaseRelation> DEF_TestCaseRelation { get; set; }
        //public DbSet<DEF_TestCaseSteps> DEF_TestCaseSteps { get; set; }
        //public DbSet<DEF_TestJobs> DEF_TestJobs { get; set; }
        //public DbSet<DEF_TestJobsDetail> DEF_TestJobsDetail { get; set; }
        //public DbSet<DEF_TestJobsDetailItem> DEF_TestJobsDetailItem { get; set; }
        //public DbSet<DEF_TestJobsDetailRelation> DEF_TestJobsDetailRelation { get; set; }
        //public DbSet<DEF_TestJobsDetailSteps> DEF_TestJobsDetailSteps { get; set; }
        public DbSet<SysAreas> SysAreas { get; set; }
        public DbSet<SysException> SysException { get; set; }
        public DbSet<SysLog> SysLog { get; set; }
        public DbSet<SysModule> SysModule { get; set; }
        public DbSet<SysModuleOperate> SysModuleOperate { get; set; }
        public DbSet<SysPosition> SysPosition { get; set; }
        public DbSet<SysRight> SysRight { get; set; }
        public DbSet<SysRightOperate> SysRightOperate { get; set; }
        public DbSet<SysRole> SysRole { get; set; }
        public DbSet<SysSample> SysSample { get; set; }
        public DbSet<SysSettings> SysSettings { get; set; }
        public DbSet<SysStruct> SysStruct { get; set; }
        public DbSet<SysUserConfig> SysUserConfig { get; set; }
        public DbSet<SysUser> SysUser { get; set; }
        public DbSet<MIS_Article> MIS_Article { get; set; }
        public DbSet<MIS_Article_Albums> MIS_Article_Albums { get; set; }
        public DbSet<MIS_Article_Category> MIS_Article_Category { get; set; }
        public DbSet<MIS_Article_Comment> MIS_Article_Comment { get; set; }
        public DbSet<MIS_WebIM_CommonTalk> MIS_WebIM_CommonTalk { get; set; }
        public DbSet<MIS_WebIM_Message> MIS_WebIM_Message { get; set; }
        public DbSet<MIS_WebIM_Message_Rec> MIS_WebIM_Message_Rec { get; set; }
        public DbSet<MIS_WebIM_RecentContact> MIS_WebIM_RecentContact { get; set; }
        public DbSet<Flow_Form> Flow_Form { get; set; }
        public DbSet<Flow_FormAttr> Flow_FormAttr { get; set; }
        public DbSet<Flow_FormContent> Flow_FormContent { get; set; }
        public DbSet<Flow_FormContentStepCheck> Flow_FormContentStepCheck { get; set; }
        public DbSet<Flow_FormContentStepCheckState> Flow_FormContentStepCheckState { get; set; }
        public DbSet<Flow_Seal> Flow_Seal { get; set; }
        public DbSet<Flow_Step> Flow_Step { get; set; }
        public DbSet<Flow_StepRule> Flow_StepRule { get; set; }
        public DbSet<Flow_Type> Flow_Type { get; set; }
        public DbSet<JOB_TASKJOBS> JOB_TASKJOBS { get; set; }
        public DbSet<JOB_TASKJOBS_LOG> JOB_TASKJOBS_LOG { get; set; }
        public DbSet<Spl_Product> Spl_Product { get; set; }
        public DbSet<Spl_ProductCategory> Spl_ProductCategory { get; set; }
        public DbSet<Spl_Person> Spl_Person { get; set; }
        public DbSet<WC_MessageResponse> WC_MessageResponse { get; set; }
        public DbSet<WC_OfficalAccounts> WC_OfficalAccounts { get; set; }
        public DbSet<WC_ResponseLog> WC_ResponseLog { get; set; }
        public DbSet<WC_Group> WC_Group { get; set; }
        public DbSet<WC_User> WC_User { get; set; }

        public DbSet<SysRoleSysUser> SysRoleSysUser { get; set; }


        public DbSet<FIX_AssessmentModel> FIX_AssessmentModel { get; set; }
        public DbSet<FIX_CustomerModel> FIX_CustomerModel { get; set; }
        public DbSet<FIX_MatchingModel> FIX_MatchingModel { get; set; }
        public DbSet<FIX_OrderHistoryModel> FIX_OrderHistoryModel { get; set; }
        public DbSet<FIX_OrderModel> FIX_OrderModel { get; set; }
        public DbSet<FIX_OutsideTroubleManModel> FIX_OutsideTroubleManModel { get; set; }

        public DbSet<Task> Task { get; set; }


        //安装EF命令install-package entityframework
        /* 在程序包管理控制台输入“Enable-Migrations”命令启用迁移。
        * Migrations文件夹下的“Configuration.cs”，将构造函数中的“AutomaticMigrationsEnabled = false;”改为“AutomaticMigrationsEnabled = true;”
        * 再次在程序包管理控制台输入“Update-Database”或者 【add-migration ChildrenInfos】后在执行 “Update-Database” 命令来更新数据库。*/
        // public Configuration()
        //{
        //    AutomaticMigrationsEnabled = true;
        //    AutomaticMigrati再onDataLossAllowed = true;
        //}
        /* 在自动生成的Configuration.cs中把修改上面的代码*/

        public DbContexts()
            : base(ConfigPara.EFDBConnection)
        //: base("DbContexts")
        {
            Database.SetInitializer<DbContexts>(new CreateDatabaseIfNotExists<DbContexts>());
            //Database.SetInitializer<DbContexts>(new DropCreateDatabaseIfModelChanges<DbContexts>());
           //Database.SetInitializer<DbContexts>(new DropCreateDatabaseAlways<DbContexts>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }

    /// <summary>
    /// 数据上下文工厂
    /// </summary>
    public class ContextFactory
    {
        /// <summary>
        /// 获取当前线程的数据上下文
        /// </summary>
        /// <returns>数据上下文</returns>
        public static DbContexts CurrentContext()
        {
            DbContexts _nContext = CallContext.GetData(ConfigPara.EFDBConnection) as DbContexts;
            if (_nContext == null)
            {
                _nContext = new DbContexts();
                CallContext.SetData(ConfigPara.EFDBConnection, _nContext);
            }
            return _nContext;
        }
    }
}
