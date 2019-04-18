using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DojoWall.Models
{
    public class Message:DataModel
    {
        [Key]
        public int MessageId{get;set;}
        public string MessageContent{get;set;}
        public int UserId{get;set;}
        public User Creator{get;set;}
        public List<Comment>  OwnedComments{get;set;}


    }
}