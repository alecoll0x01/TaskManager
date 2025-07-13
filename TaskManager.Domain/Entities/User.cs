using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities
{
    public class User : Entity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public UserRole Role { get; private set; }

        private User() { }

        public User(string name, string email, UserRole role) : base()
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Role = role;
        }

        public bool IsManager() => Role == UserRole.Manager;
    }

}
