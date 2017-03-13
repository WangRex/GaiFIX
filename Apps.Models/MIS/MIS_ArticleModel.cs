﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2013-04-23 17:00:56 by App
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.MIS
{

    public partial class MIS_Article
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [Required(ErrorMessage = "{0}必须是一个数字")]
        [Display(Name = "模块ID")]
        public string ChannelId { get; set; }
     

       
        [MaxWordsExpression(50)]
        //[NotNullExpression]
        [Display(Name = "栏目ID")]
        public string CategoryId { get; set; }
        //大类名称
        public  string CategoryName { get; set; }
        //上级ID
        public  string CategoryIdParent { get; set; }
        //[NotNullExpression]
        [MaxWordsExpression(100)]
        [MinWordsExpression(5)]
        [Display(Name = "标题")]
        public string Title { get; set; }

        [MaxWordsExpression(255)]
        [Display(Name = "图片")]
        public string ImgUrl { get; set; }

        //[NotNullExpression]
        [Display(Name = "内容")]
        public string BodyContent { get; set; }

        //[NotNullExpression]
        [IsNumberExpression]//如果填写判断是否是数字
        [Display(Name = "排序号")]
        public  string Sort { get; set; }

        //[NotNullExpression]
        [IsNumberExpression]//如果填写判断是否是数字
        [Display(Name = "点击数")]
        public  string Click { get; set; }


        [Display(Name = "是否审核")]
        public string CheckFlag { get; set; }

        [Display(Name = "审核人")]
        public string Checker { get; set; }
        public  string CheckerName { get; set; }

        [Display(Name = "审核时间")]
        public string CheckDateTime { get; set; }

        [Display(Name = "创建人")]
        public string Creater { get; set; }
        public string CreaterName { get; set; }

        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }

        [MaxWordsExpression(300)]
        [Display(Name = "自定义链接")]
        public string CustomLink { get; set; }

        [Display(Name = "文章类型")]
        public string IsType { get; set; }

        [Display(Name = "任务")]
        public string Task { get; set; }

    }
}

