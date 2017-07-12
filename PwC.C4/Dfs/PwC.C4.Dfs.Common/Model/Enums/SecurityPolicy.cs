namespace PwC.C4.Dfs.Common.Model.Enums
{
    public class SecurityPolicy
    {
        public const ulong DefaultMask = 0;
        public const ulong DefaultNMask = ~DefaultMask;

        private ulong _mask; // pm: policy mask
        public ulong Mask
        {
            get { return _mask; }
        }

        private ulong _nmask; // npm: negative policy mask
        public ulong NMask
        {
            get { return _nmask; }
        }

        #region ctor

        public SecurityPolicy()
            : this(DefaultMask, DefaultNMask)
        {
        }

        public SecurityPolicy(ulong mask, ulong nmask)
        {
            _mask = mask;
            _nmask = nmask;
        }

        #endregion

        public override bool Equals(object obj)
        {
            var other = obj as SecurityPolicy;
            return other != null
                && other.Mask == Mask
                && other.NMask == NMask;
        }

        public static bool CaptchaDisabled(SecurityPolicy policy)
        {
            return (Captcha.Mask & policy.NMask) == 0;
        }

        public static bool ExpirationEnabled(SecurityPolicy policy)
        {
            return (Expiration.Mask & policy.Mask) != 0;
        }

        public static SecurityPolicy ProtectedNoCaptcha
        {
            get
            {
                return new SecurityPolicy(Expiration.Mask, NoCaptcha.NMask);
            }
        }

        public static SecurityPolicy Signature
        {
            get
            {
                var value = new SecurityPolicy();
                value._mask |= 1;
                return value;
            }
        }

        public static SecurityPolicy Captcha
        {
            get
            {
                var value = new SecurityPolicy();
                value._mask |= 2;
                return value;
            }
        }

        public static SecurityPolicy Login
        {
            get
            {
                var value = new SecurityPolicy();
                value._mask |= 4;
                return value;
            }
        }

        public static SecurityPolicy Expiration
        {
            get
            {
                var value = new SecurityPolicy();
                value._mask |= 8;
                return value;
            }
        }

        public static SecurityPolicy NoSignature
        {
            get
            {
                var value = new SecurityPolicy();
                value._nmask &= (~(1UL));
                return value;
            }
        }

        public static SecurityPolicy NoCaptcha
        {
            get
            {
                var value = new SecurityPolicy();
                value._nmask &= (~(2UL));
                return value;
            }
        }

        public static SecurityPolicy NoLogin
        {
            get
            {
                var value = new SecurityPolicy();
                value._nmask &= (~(4UL));
                return value;
            }
        }

        public static SecurityPolicy NoExpiration
        {
            get
            {
                var value = new SecurityPolicy();
                value._nmask &= (~(8UL));
                return value;
            }
        }
    }
}
