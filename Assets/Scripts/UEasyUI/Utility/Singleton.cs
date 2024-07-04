
namespace UEasyUI
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static T sInstance = null;
        private static bool sApplicationIsQuitting = false;
        private static readonly object sysob = new object();

        ~Singleton()
        {
            sApplicationIsQuitting = true;
        }

        public static T Instance
        {
            get
            {
                if (sApplicationIsQuitting)
                {
                    return null;
                }

                if (sInstance == null)
                {
                    lock (sysob)
                    {
                        sInstance = new T();
                    }
                }
                return sInstance;
            }
        }
    }
}