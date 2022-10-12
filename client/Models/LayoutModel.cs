using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace client.Models
{
    public class LayoutModel
    {
        public User User { get; }

        public LayoutModel()
        {
        }

        public LayoutModel(User user)
        {
            User = user;
        }

        public bool IsLogin() => User != null;

        public HtmlString SignVisibility()
        {
            if(!IsLogin())
               return new ("block");
            return new ("none");
        }

        public HtmlString ProfileVisibility()
        {
            if(IsLogin())
               return new ("block");
            return new ("none");
        }
        
    }
}