using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;


namespace DojoWall.Models
{
    public class Wall
    {
        public User User{get;set;}
        public int UserId{get;set;}
        public int MessageId{get;set;}
        public List<Message> AllMessages{get;set;}
        [Required]
        [MinLength(3, ErrorMessage = "Message Must be 3 characters or longer!")]
        [Display(Name = "Post a Message:")]
        public String NewMessage{get;set;}
        [Required]
        [MinLength(3, ErrorMessage = "Comment Must be 3 characters or longer!")]
        [Display(Name = "Post a Comment:")]
        public String NewComment{get;set;}
        
    }

}