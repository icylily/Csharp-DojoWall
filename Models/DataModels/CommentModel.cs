using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DojoWall.Models
{
    public class Comment:DataModel
    {
        [Key]
        public int CommentId{get;set;}
        public string CommentContent{get;set;}
        public int UserId{get;set;}
        public User Creator{get;set;}
        public int MessageId{get;set;}
        public Message BelongedMessage{get;set;}
        
    }
}
