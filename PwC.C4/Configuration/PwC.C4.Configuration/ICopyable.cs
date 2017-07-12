namespace PwC.C4.Configuration
{
    /// <summary>
    /// use to Copy from one configuration instance to single instance of the configuration class
    /// by default config handler uses class public property/field to copy, but if the configuration class has some special logic to copy, it need to use this interface
    /// </summary>
    public interface ICopyable
    {
        /// <summary>
        /// copy property to destination object
        /// </summary>
        /// <param name="destObject">destination object</param>
        void CopyTo(object destObject);
    }
}
