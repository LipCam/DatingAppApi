using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingAppApi.DAL.Entities
{
    [Table("Users")]
    public class AppUsers : IdentityUser<long>
    {

        //public long Id { get; set; }

        //[MaxLength(50)]
        //public string? UserName { get; set; }

        //public byte[] PasswordHash { get; set; } = [];
        //public byte[] PasswordSalt { get; set; } = [];
        public DateOnly DateOfBirth { get; set; }

        [MaxLength(50)]
        public required string KnownAs { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;

        [MaxLength(10)]
        public required string Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }

        [MaxLength(100)]
        public string? LookingFor { get; set; }

        [MaxLength(100)]
        public required string City { get; set; }

        [MaxLength(100)]
        public required string Country { get; set; }

        public List<Photos> Photos { get; set; } = [];
        public List<UserLikes> LikedByUsers { get; set; } = [];
        public List<UserLikes> LikedUsers { get; set; } = [];
        public List<Messages> MessagesSent { get; set; } = [];
        public List<Messages> MessagesReceived { get; set; } = [];
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}

/*
 [
  '{{repeat(5)}}',
  {
    UserName: '{{firstName("female")}}',
    Gender: 'female',
    DateOfBirth: '{{date(new Date(1970,0,1), new Date(2000, 11, 31), "YYYY-MM-dd")}}',
    KnownAs: function(){ return this. UserName; },
    Created: '{{date(new Date(2020, 0, 1), new Date(2020,5,30), "YYYY-MM-dd")}}',
    LastActive: '{{date(new Date(2021, 4, 1), new Date(2021,5,30), "YYYY-MM-dd")}}',
    Introduction: '{{lorem (1, "paragraphs")}}',
    LookingFor: '{{lorem(1, "sentences").substring(0, 100)}}',
    Interests: '{{lorem (1, "sentences")}}',
    City: '{{city()}}',
    Country: '{{country()}}',
    Photos: [
    	{    
    		Url: function(num) {
    		return 'https://randomuser.me/api/portraits/women/' + num.integer(1,99) + '.jpg';
         },
    	IsMain: true
        }
      ]
    }
]
 */ 