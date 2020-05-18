//using System;

//namespace Coco.Framework.Models
//{
//    public class IdentityUser<TKey> where TKey : IEquatable<TKey>
//    {
//        /// <summary>
//        /// Initializes a new instance of <see cref="IdentityUser{TKey}"/>.
//        /// </summary>
//        public IdentityUser() { }

//        /// <summary>
//        /// Initializes a new instance of <see cref="IdentityUser{TKey}"/>.
//        /// </summary>
//        /// <param name="userName">The user name.</param>
//        public IdentityUser(string userName) : this()
//        {
//            UserName = userName;
//        }

//        /// <summary>
//        /// Gets or sets the primary key for this user.
//        /// </summary>
//        public virtual TKey Id { get; set; }

//        /// <summary>
//        /// Gets or sets the user name for this user.
//        /// </summary>
//        public virtual string UserName { get; set; }


//        /// <summary>
//        /// Gets or sets the email address for this user.
//        /// </summary>
//        public virtual string Email { get; set; }

//        /// <summary>
//        /// Gets or sets a salted and hashed representation of the password for this user.
//        /// </summary>
//        public virtual string PasswordHash { get; set; }

//        /// <summary>
//        ///     True if the email is confirmed, default is false
//        /// </summary>
//        public virtual bool IsEmailConfirmed { get; set; }

//        /// <summary>
//        /// A random value that should change whenever a users credentials change (password changed, login removed)
//        /// </summary>
//        public virtual string IdentityStamp { get; set; }
//        /// <summary>
//        /// A random value that should change whenever a users credentials change (password changed, login removed)
//        /// </summary>
//        public string ActiveUserStamp { get; set; }

//        public string Password { get; set; }
//        public string PasswordSalt { get; set; }

//        /// <summary>
//        /// Returns the username for this user.
//        /// </summary>
//        public override string ToString()
//            => UserName;
//    }
//}
