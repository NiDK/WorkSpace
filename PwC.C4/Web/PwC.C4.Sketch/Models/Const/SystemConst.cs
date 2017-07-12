namespace PwC.C4.Sketch.Models.Const
{
    public static class SystemConst
    {
        public static string ConnName = System.Configuration.ConfigurationManager.AppSettings["SketchConnName"];

        public static string EntityName = System.Configuration.ConfigurationManager.AppSettings["SketchEntityName"];
    }
}