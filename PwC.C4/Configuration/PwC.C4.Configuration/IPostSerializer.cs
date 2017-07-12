namespace PwC.C4.Configuration
{
    /// <summary>
    /// use to do some special logic after the serialize function is over.
    /// such as special properties/fields need to be calculate after serialization
    /// Pls don't use this for static properties/fields.
    /// </summary>
    public interface IPostSerializer
    {
        void PostSerializer();
    }
}
